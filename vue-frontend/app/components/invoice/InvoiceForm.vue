<script setup lang="ts">
import { toast } from "vue-sonner";
import { CalendarDate, parseDate } from "@internationalized/date"
import { cn } from "@/lib/utils"
import * as z from "zod";
import { toTypedSchema } from '@vee-validate/zod';
import type { SubmissionHandler } from 'vee-validate'
import { emailSchema, currencySchema, requiredStringSchema } from "~/lib/schemas";
import {
    CurrencyCode,
    InvoiceDiscountType,
    InvoicePaymentStatus,
    type BusinessDto,
    type CustomerDto,
    type InvoiceDiscountEditDto,
    type InvoiceEditDto,
    type InvoiceItemEditDto,
} from "~/api-client";
import { CalendarIcon, Copy, Download, Pencil, Plus, RefreshCcw, Save, Trash, Trash2, Undo } from "lucide-vue-next";

const {
    businesses,
    customers,
    onSubmit,
    invoice,
    generateInvoiceNumber,
    validateInvoiceNumber,
} = defineProps<{
    onSubmit: (invoice: InvoiceEditDto) => Promise<void>,
    invoice: InvoiceEditDto,
    businesses: BusinessDto[],
    customers: CustomerDto[],
    generateInvoiceNumber: (date: string) => Promise<string>,
    validateInvoiceNumber: (invoiceNumber: string, invoiceId?: string | null | undefined)
        => Promise<[boolean, string | null]>,
}>();

function makeCustomerDisplayText(customer: CustomerDto): string {
    const sb: string[] = [];
    sb.push(customer.name ?? "");
    if (customer.email) sb.push(`(${customer.email})`);
    return sb.join(" ");
}

const l = useLoading();
const c = useCurrency();
const api = useApi();


const isDownloading = ref<boolean>(false);

async function downloadInvoice() {
    isDownloading.value = true;
    const response = await api.invoices.generateInvoicePdf(invoice.id!, { responseType: 'blob' });
    const filename = `${businesses.find(b => b.id == invoice.businessId)?.name} - ${invoice.invoiceNumber}.pdf`;
    const blob = new Blob([response.data], { type: 'application/pdf ' });
    const url = URL.createObjectURL(blob);
    const a = document.createElement('a');
    a.href = url;
    a.download = filename;
    document.body.appendChild(a);
    a.click();
    a.remove();
    URL.revokeObjectURL(url);
    isDownloading.value = false;
}

const initialValues = ref<InvoiceEditDto>({
    ...invoice,
    invoiceDate: invoice.invoiceDate ? invoice.invoiceDate.split("T")[0] : undefined,
    shouldSaveBusiness: !invoice.businessId,
});

watch(() => invoice, (newVal) => initialValues.value = { ...newVal });

const newEntity = !invoice.id;

const handleInvoiceSubmit: SubmissionHandler = async (values) => {
    const _values = (values as InvoiceEditDto);
    const dto: InvoiceEditDto = { ..._values };
    dto.invoiceDate = new Date(dto.invoiceDate!).toISOString();
    dto.items = [...itemsRef.value];
    dto.discounts = [...discountsRef.value];
    dto.shouldSaveCustomer = _values.shouldSaveCustomer ?? false;
    dto.shouldSaveBusiness = _values.shouldSaveBusiness ?? false;
    if (dto.customerId === BLANKUUID) dto.customerId = null;
    if (dto.businessId === BLANKUUID) dto.businessId = null;
    await onSubmit(dto);
}

async function duplicateInvoice() {
    l.setLoading();
    const { data } = await api.invoices.duplicateInvoice(invoice.id!);
    navigateTo(`/invoices/${data?.data?.id ?? ''}`);
}

const shouldUpdateBusinessCheckboxDisabled = ref<boolean>(!invoice.businessId);

// ====================================================================================================================
// Form Schemas =======================================================================================================
// ====================================================================================================================
const invoiceNumberError = ref<string | null>(null);
const formSchema = toTypedSchema(z.object({
    id: z.string().uuid().nullish(),
    invoiceNumber: z.string().min(4, "Must be at least 4 characters").refine(async (val) => {
        const [isValid, message] = await validateInvoiceNumber(val, invoice.id);
        if (isValid) return true;
        invoiceNumberError.value = message;
        return false;
    }, { message: invoiceNumberError.value ?? "An invoice with that number already exists" }),
    invoiceDate: requiredStringSchema,
    currency: z.enum([CurrencyCode.Usd, CurrencyCode.Eur, CurrencyCode.Zar]),
    paymentInstructions: requiredStringSchema,
    customerId: z.string().nullish(),
    customerName: requiredStringSchema,
    customerPhone: requiredStringSchema,
    customerEmail: requiredStringSchema,
    customerAddress: requiredStringSchema,
    businessId: z.string().nullish(),
    shouldSaveCustomer: z.boolean().optional(),
    businessName: requiredStringSchema,
    businessTagline: z.string().nullish(),
    businessEmail: emailSchema,
    businessPhone: requiredStringSchema,
    businessAddress: requiredStringSchema,
    businessWebsite: z.string().nullish(),
    shouldSaveBusiness: z.boolean().optional(),
    paymentStatus: z.enum(['Paid', 'Unpaid']),
}));

const itemFormSchema = toTypedSchema(z.object({
    id: z.string().uuid().nullable(),
    description: z.string(),
    quantity: z.number().multipleOf(0.01).positive("Must be a positive number"),
    rate: currencySchema,
}));

const discountFormSchema = toTypedSchema(z.object({
    id: z.string().uuid().nullable(),
    description: z.string(),
    amount: currencySchema,
    type: z.enum(['Fixed', 'Percentage']),
}));

// ====================================================================================================================
// Calendar Component =================================================================================================
// ====================================================================================================================

const today = new Date();

const getMinCalendarValue = () => new CalendarDate(1900, 1, 1);
const getMaxCalendarValue = () => new CalendarDate(today.getFullYear() + 1, today.getMonth() + 1, today.getDate())

const datePlaceholder = ref()


type SetFieldValues<T> = (path: string, value: T) => void;

async function onGenerate(date: string, setFieldValue: SetFieldValues<string>) {
    const result = await generateInvoiceNumber(date)
    setFieldValue('invoiceNumber', result);
}

const editMode = ref<boolean>(newEntity);

// ====================================================================================================================
// Invoice Items ======================================================================================================
// ====================================================================================================================
const itemsRef = ref<InvoiceItemEditDto[]>([...invoice.items ?? []]);

const itemDialogOpen = ref<boolean>(false);
const editItem = ref<InvoiceItemEditDto | null>(null);
const editItemIndex = ref<number | null>(null);

function openItemDialog(index: number | null) {
    if (index != null) {
        editItemIndex.value = index;
        editItem.value = { ...itemsRef.value[index]! };
    } else {
        editItem.value = { id: blankUuid(), description: undefined, quantity: undefined, rate: undefined };
    }
    itemDialogOpen.value = true;
}

function duplicateItem(idx: number) {
    const newItem = { ...itemsRef.value[idx] };
    newItem.id = null;
    newItem.description = newItem.description + " (copy)";
    itemsRef.value.push(newItem);
}

function removeItem(idx: number) {
    itemsRef.value = itemsRef.value.filter((it, i) => i != idx);
}

async function handleItemDialogSubmit(values: InvoiceItemEditDto) {
    if (editItemIndex.value == null) {
        itemsRef.value.push({ ...values, invoiceId: invoice.id ?? blankUuid() });
    } else {
        itemsRef.value[editItemIndex.value] = { ...values, invoiceId: invoice.id ?? blankUuid() };
    }
    editItem.value = null;
    editItemIndex.value = null;
    itemDialogOpen.value = false;
}

const selectedCurrencyRef = ref<CurrencyCode | undefined>(invoice.currency);

function makeItemText(item: InvoiceItemEditDto) {
    return `${item.quantity ?? 0} @ ${c.format(item.rate ?? 0, selectedCurrencyRef.value)}`;
}

// ====================================================================================================================
// Invoice Discounts ==================================================================================================
// ====================================================================================================================
const discountsRef = ref<InvoiceDiscountEditDto[]>([...invoice.discounts ?? []]);

const discountDialogOpen = ref<boolean>(false);
const editDiscount = ref<InvoiceDiscountEditDto | null>(null);
const editDiscountIndex = ref<number | null>(null);

function openDiscountDialog(index: number | null) {
    if (index != null) {
        editDiscountIndex.value = index;
        editDiscount.value = { ...discountsRef.value[index]! };
    } else {
        editDiscount.value = { id: blankUuid(), description: undefined, amount: undefined, type: undefined };
    }
    discountDialogOpen.value = true;
}

function duplicateDiscount(idx: number) {
    const newDiscount = { ...discountsRef.value[idx] };
    newDiscount.id = null;
    newDiscount.description = newDiscount.description + " (copy)";
    discountsRef.value.push(newDiscount);
}

function removeDiscount(idx: number) {
    discountsRef.value = discountsRef.value.filter((it, i) => i != idx);
}

async function handleDiscountDialogSubmit(values: InvoiceDiscountEditDto) {
    if (editDiscountIndex.value == null) {
        discountsRef.value.push({ ...values, invoiceId: invoice.id ?? blankUuid() });
    } else {
        discountsRef.value[editDiscountIndex.value] = { ...values, invoiceId: invoice.id ?? blankUuid() };
    }
    editDiscount.value = null;
    editDiscountIndex.value = null;
    discountDialogOpen.value = false;
}

function makeDiscountText(discount: InvoiceDiscountEditDto) {
    if (discount.type == InvoiceDiscountType.Percentage)
        return `${discount.amount?.toFixed(2)}%`;
    return c.format(discount.amount ?? 0, selectedCurrencyRef.value);
}

async function deleteInvoice() {
    if (!newEntity) {
        await api.invoices.deleteInvoice(invoice.id!);
        toast.success('Invoice deleted successfully');
    }
    return navigateTo('/invoices');
}
</script>

<template>
    <!-- ============================================================================================= -->
    <!-- Template ==================================================================================== -->
    <!-- ============================================================================================= -->
    <Form v-slot="{ resetForm, setFieldValue, values: invoiceValues }" class="h-full flex justify-center"
        :initial-values="initialValues" :validation-schema="formSchema" @submit="handleInvoiceSubmit">
        <FormField name="id">
            <FormItem>
                <FormControl>
                    <Input hidden />
                </FormControl>
            </FormItem>
        </FormField>
        <Card class="w-full max-w-md mx-auto">
            <CardHeader class="flex justify-between items-center">
                <CardTitle>
                    <span v-if="newEntity">New Invoice</span>
                    <span v-if="editMode && !newEntity">Edit </span>
                    <span v-if="!newEntity">{{ initialValues.invoiceNumber }}</span>
                </CardTitle>
                <div class="space-x-2">
                    <AlertDialog>
                        <AlertDialogTrigger as-child>
                            <Button type="button" size="icon-sm" variant="outline" class="rounded-full cursor-pointer"
                                :aria-label="`${getDestructiveWord(newEntity, true)} Invoice`">
                                <Trash2 />
                            </Button>
                        </AlertDialogTrigger>
                        <AlertDialogContent>
                            <AlertDialogHeader>
                                <AlertDialogTitle>{{ getDestructiveWord(newEntity, true) }} Invoice</AlertDialogTitle>
                                <AlertDialogDescription>Are you sure you want to {{ getDestructiveWord(newEntity) }}
                                    this invoice?
                                </AlertDialogDescription>
                            </AlertDialogHeader>
                            <AlertDialogFooter>
                                <AlertDialogCancel class="cursor-pointer">Cancel</AlertDialogCancel>
                                <AlertDialogAction
                                    class="cursor-pointer bg-destructive text-white shadow-xs hover:bg-destructive/90 focus-visible:ring-destructive/20 dark:focus-visible:ring-destructive/40 dark:bg-destructive/60"
                                    @click="deleteInvoice()">
                                    <Trash2 /> {{ getDestructiveWord(newEntity, true) }}
                                </AlertDialogAction>
                            </AlertDialogFooter>
                        </AlertDialogContent>
                    </AlertDialog>
                    <template v-if="editMode">
                        <Button type="submit" size="icon-sm" variant="outline" class="rounded-full cursor-pointer"
                            aria-label="Save Invoice">
                            <Save />
                        </Button>
                    </template>
                    <template v-if="editMode && !newEntity">
                        <Button type="button" size="icon-sm" variant="outline" class="rounded-full cursor-pointer"
                            aria-label="Cancel Edit"
                            @click="() => { resetForm({ values: initialValues }); editMode = false }">
                            <Undo />
                        </Button>
                    </template>
                    <template v-if="!newEntity && !editMode">
                        <Button type="button" size="icon-sm" variant="outline" class="rounded-full cursor-pointer"
                            aria-label="Duplicate Invoice" @click="duplicateInvoice()">
                            <Copy />
                        </Button>
                        <Button type="button" size="icon-sm" variant="outline" class="rounded-full cursor-pointer"
                            aria-label="Edit Invoice" @click="editMode = true">
                            <Pencil />
                        </Button>
                    </template>
                </div>
            </CardHeader>
            <CardContent class="space-y-6">
                <FormField v-slot="{ componentField }" name="businessId">
                    <FormItem>
                        <FormLabel>Business</FormLabel>
                        <Select :disabled="!editMode" v-bind="componentField" @update:model-value="(v) => {
                            if (v) {
                                const business = businesses.find(b => b.id == v);
                                setFieldValue('businessId', v);
                                setFieldValue('currency', business?.defaultCurrency);
                                setFieldValue('paymentInstructions', business?.defaultPaymentInstructions ?? '');
                                setFieldValue('businessName', business?.name ?? '');
                                setFieldValue('businessEmail', business?.email ?? '');
                                setFieldValue('businessPhone', business?.phone ?? '');
                                setFieldValue('businessAddress', business?.address ?? '');
                                setFieldValue('shouldSaveBusiness', !business);
                                shouldUpdateBusinessCheckboxDisabled = !business
                            }
                            else {
                                setFieldValue('businessId', undefined)
                                setFieldValue('shouldSaveBusiness', true);
                                shouldUpdateBusinessCheckboxDisabled = true;
                            }
                        }">
                            <FormControl>
                                <SelectTrigger class="w-full">
                                    <SelectValue placeholder="Select the business of the invoice" />
                                </SelectTrigger>
                            </FormControl>
                            <SelectContent>
                                <SelectGroup>
                                    <SelectGroupLabel v-if="businesses.length">Saved Businesses</SelectGroupLabel>
                                    <SelectItem v-for="business in businesses" :key="`business-${business.id}`"
                                        :value="business.id!">
                                        {{ business.name }}
                                    </SelectItem>
                                </SelectGroup>
                                <SelectSeparator />
                                <SelectItem key="business-blank" :value="BLANKUUID">New Business</SelectItem>
                            </SelectContent>
                        </Select>
                        <FormMessage />
                    </FormItem>
                </FormField>

                <FormField v-slot="{ componentField }" name="businessName">
                    <FormItem>
                        <FormLabel>Business Name</FormLabel>
                        <FormControl>
                            <Input v-bind="componentField" placeholder="Enter business name" :disabled="!editMode" />
                        </FormControl>
                        <FormMessage />
                    </FormItem>
                </FormField>

                <FormField v-slot="{ componentField }" name="businessTagline">
                    <FormItem>
                        <FormLabel>Business Tagline</FormLabel>
                        <FormControl>
                            <Input v-bind="componentField" placeholder="Enter business tagline" :disabled="!editMode" />
                        </FormControl>
                        <FormMessage />
                    </FormItem>
                </FormField>

                <FormField v-slot="{ componentField }" name="businessEmail">
                    <FormItem>
                        <FormLabel>Business Email</FormLabel>
                        <FormControl>
                            <Input v-bind="componentField" placeholder="Enter business email" :disabled="!editMode" />
                        </FormControl>
                        <FormMessage />
                    </FormItem>
                </FormField>

                <FormField v-slot="{ componentField }" name="businessPhone">
                    <FormItem>
                        <FormLabel>Business Phone</FormLabel>
                        <FormControl>
                            <Input v-bind="componentField" placeholder="Enter business phone" :disabled="!editMode" />
                        </FormControl>
                        <FormMessage />
                    </FormItem>
                </FormField>

                <FormField v-slot="{ componentField }" name="businessWebsite">
                    <FormItem>
                        <FormLabel>Business Website</FormLabel>
                        <FormControl>
                            <Input v-bind="componentField" placeholder="Enter business website" :disabled="!editMode" />
                        </FormControl>
                        <FormMessage />
                    </FormItem>
                </FormField>

                <FormField v-slot="{ componentField }" name="businessAddress">
                    <FormItem>
                        <FormLabel>Business Address</FormLabel>
                        <FormControl>
                            <Textarea class="resize-none h-25" v-bind="componentField"
                                placeholder="Enter business address" :disabled="!editMode" />
                        </FormControl>
                        <FormMessage />
                    </FormItem>
                </FormField>

                <template v-if="editMode">
                    <FormField v-if="editMode" v-slot="{ value, handleChange }" type="checkbox"
                        name="shouldSaveBusiness">
                        <FormItem class="flex flex-row items-start gap-x-3 space-y-0 rounded-md border p-4">
                            <FormControl>
                                <Checkbox :disabled="!editMode || shouldUpdateBusinessCheckboxDisabled"
                                    :model-value="value" class="cursor-pointer" @update:model-value="handleChange" />
                            </FormControl>
                            <div class="space-y-1 leading-none">
                                <FormLabel :class="{
                                    'cursor-pointer': !shouldUpdateBusinessCheckboxDisabled,
                                    'cursor-not-allowed': shouldUpdateBusinessCheckboxDisabled,
                                    'text-muted-foreground': shouldUpdateBusinessCheckboxDisabled
                                }">
                                    Save Business</FormLabel>
                                <FormDescription>
                                    This will create new business or update an existing business with new details.
                                </FormDescription>
                                <FormMessage />
                            </div>
                        </FormItem>
                    </FormField>
                </template>

                <FormField v-slot="{ componentField }" v-model="selectedCurrencyRef" name="currency">
                    <FormItem>
                        <FormLabel>Currency</FormLabel>
                        <Select :disabled="!editMode" v-bind="componentField">
                            <FormControl>
                                <SelectTrigger class="w-full">
                                    <SelectValue placeholder="Select a currency for the invoice" />
                                </SelectTrigger>
                            </FormControl>
                            <SelectContent>
                                <SelectGroup>
                                    <SelectItem v-for="currency in Currencies" :key="`currency_${currency.code}`"
                                        :value="currency.code">
                                        {{ `${currency.code} - ${currency.display}` }}
                                    </SelectItem>
                                </SelectGroup>
                            </SelectContent>
                        </Select>
                        <FormMessage />
                    </FormItem>
                </FormField>

                <!-- ============================================================================================= -->
                <!-- Invoice Details ============================================================================= -->
                <!-- ============================================================================================= -->
                <div class="flex justify-between">
                    <FormField name="invoiceDate">
                        <FormItem>
                            <FormLabel>Invoice Date</FormLabel>
                            <Popover>
                                <PopoverTrigger as-child>
                                    <FormControl>
                                        <Button :disabled="!editMode" variant="outline" :class="cn(
                                            'w-[240px] ps-3 text-start font-normal',
                                            !invoiceValues.invoiceDate && 'text-muted-foreground',
                                        )">
                                            <span>
                                                {{ displayDate(invoiceValues.invoiceDate) }}
                                            </span>
                                            <CalendarIcon class="ms-auto h-4 w-4 opacity-50" />
                                        </Button>
                                        <input hidden>
                                    </FormControl>
                                </PopoverTrigger>
                                <PopoverContent class="w-auto p-0">
                                    <Calendar v-model:placeholder="datePlaceholder" class="pointer-events-auto"
                                        :min-value="getMinCalendarValue()" :max-value="getMaxCalendarValue()"
                                        :model-value="invoiceValues.invoiceDate ? parseDate(invoiceValues.invoiceDate) : undefined"
                                        calendar-label="Invoice date" initial-focus :week-starts-on="1"
                                        :prevent-deselect="true" @update:model-value="(v) => {
                                            if (v) {
                                                setFieldValue('invoiceDate', v.toString())
                                            }
                                            else {
                                                setFieldValue('invoiceDate', undefined)
                                            }
                                        }" />
                                </PopoverContent>
                            </Popover>
                            <FormMessage />
                        </FormItem>
                    </FormField>

                    <FormField v-if="!newEntity" name="paymentStatus">
                        <FormItem>
                            <FormLabel>Paid</FormLabel>
                            <FormControl>
                                <Switch :disabled="!editMode"
                                    :model-value="invoiceValues.paymentStatus === InvoicePaymentStatus.Paid ? true : false"
                                    @update:model-value="(v) => setFieldValue('paymentStatus', v ? InvoicePaymentStatus.Paid : InvoicePaymentStatus.Unpaid)" />
                            </FormControl>
                        </FormItem>
                    </FormField>

                </div>

                <FormField v-slot="{ componentField }" name="invoiceNumber">
                    <FormItem class="flex-grow-1">
                        <FormLabel>Invoice Number</FormLabel>
                        <div class="flex justify-between itens-center gap-8">
                            <FormControl>
                                <Input v-bind="componentField" placeholder="Enter the invoice number"
                                    :disabled="!editMode" />
                            </FormControl>
                            <Button v-if="editMode" type="button" class="cursor-pointer"
                                @click="onGenerate(invoiceValues.invoiceDate, setFieldValue)">
                                <RefreshCcw />
                                Generate
                            </Button>
                        </div>
                        <FormMessage />
                    </FormItem>
                </FormField>

                <FormField v-slot="{ componentField }" name="paymentInstructions">
                    <FormItem>
                        <FormLabel>Payment Instructions</FormLabel>
                        <FormControl>
                            <Textarea class="resize-none h-25" v-bind="componentField"
                                placeholder="Enter payment instructions" :disabled="!editMode" />
                        </FormControl>
                        <FormMessage />
                    </FormItem>
                </FormField>

                <Separator />

                <!-- ============================================================================================= -->
                <!-- Customer Details ============================================================================ -->
                <!-- ============================================================================================= -->
                <FormField v-slot="{ componentField }" name="customerId">
                    <FormItem>
                        <FormLabel>Customer</FormLabel>
                        <Select :disabled="!editMode" v-bind="componentField" @update:model-value="(v) => {
                            if (v) {
                                const customer = customers.find(c => c.id == v);
                                setFieldValue('customerId', v);
                                setFieldValue('customerName', customer?.name ?? '');
                                setFieldValue('customerEmail', customer?.email ?? '');
                                setFieldValue('customerPhone', customer?.phone ?? '');
                                setFieldValue('customerAddress', customer?.address ?? '');
                            }
                            else {
                                setFieldValue('customerId', null)
                            }
                        }">
                            <FormControl>
                                <SelectTrigger class="w-full">
                                    <SelectValue placeholder="Select a saved customer for the invoice" />
                                </SelectTrigger>
                            </FormControl>
                            <SelectContent>
                                <SelectGroup>
                                    <SelectGroupLabel v-if="customers.length">Saved Customers</SelectGroupLabel>
                                    <SelectItem v-for="customer in customers" :key="`customer-${customer.id}`"
                                        :value="customer.id!">
                                        {{ makeCustomerDisplayText(customer) }}
                                    </SelectItem>
                                </SelectGroup>
                                <SelectSeparator />
                                <SelectItem key="customer-blank" :value="BLANKUUID">New Customer</SelectItem>
                            </SelectContent>
                        </Select>
                        <FormMessage />
                    </FormItem>
                </FormField>

                <FormField v-slot="{ componentField }" name="customerName">
                    <FormItem>
                        <FormLabel>Customer Name</FormLabel>
                        <FormControl>
                            <Input v-bind="componentField" placeholder="Enter customer name" :disabled="!editMode" />
                        </FormControl>
                        <FormMessage />
                    </FormItem>
                </FormField>

                <FormField v-slot="{ componentField }" name="customerEmail">
                    <FormItem>
                        <FormLabel>Customer Email</FormLabel>
                        <FormControl>
                            <Input v-bind="componentField" placeholder="Enter customer email" :disabled="!editMode" />
                        </FormControl>
                        <FormMessage />
                    </FormItem>
                </FormField>

                <FormField v-slot="{ componentField }" name="customerPhone">
                    <FormItem>
                        <FormLabel>Customer Phone</FormLabel>
                        <FormControl>
                            <Input v-bind="componentField" placeholder="Enter customer phone" :disabled="!editMode" />
                        </FormControl>
                        <FormMessage />
                    </FormItem>
                </FormField>

                <FormField v-slot="{ componentField }" name="customerAddress">
                    <FormItem>
                        <FormLabel>Customer Address</FormLabel>
                        <FormControl>
                            <Textarea class="resize-none h-25" v-bind="componentField"
                                placeholder="Enter customer address" :disabled="!editMode" />
                        </FormControl>
                        <FormMessage />
                    </FormItem>
                </FormField>

                <template v-if="editMode">
                    <FormField v-slot="{ value, handleChange }" type="checkbox" name="shouldSaveCustomer">
                        <FormItem class="flex flex-row items-start gap-x-3 space-y-0 rounded-md border p-4">
                            <FormControl>
                                <Checkbox :model-value="value" class="cursor-pointer"
                                    @update:model-value="handleChange" />
                            </FormControl>
                            <div class="space-y-1 leading-none">
                                <FormLabel class="cursor-pointer">Save Customer</FormLabel>
                                <FormDescription>
                                    This will create new customers or update existing customers with new details.
                                </FormDescription>
                                <FormMessage />
                            </div>
                        </FormItem>
                    </FormField>
                </template>

                <Separator />

                <!-- ============================================================================================= -->
                <!-- Invoice Items =============================================================================== -->
                <!-- ============================================================================================= -->
                <h2 class="text-lg font-bold">Invoice Items</h2>
                <Dialog v-model:open="itemDialogOpen" :modal="true">
                    <ItemGroup>
                        <Item v-for="(it, idx) in itemsRef" :key="`item_${it.id}`">
                            <ItemContent>
                                <ItemHeader>{{ it.description }}</ItemHeader>
                                <ItemDescription>{{ makeItemText(it) }}</ItemDescription>
                            </ItemContent>
                            <ItemActions v-if="editMode">
                                <Button size="icon-sm" type="button" variant="ghost" aria-label="Edit"
                                    class="cursor-pointer rounded-full" @click="openItemDialog(idx)">
                                    <Pencil />
                                </Button>
                                <Button size="icon-sm" type="button" variant="ghost" aria-label="Duplicate Item"
                                    class="cursor-pointer rounded-full" @click="duplicateItem(idx)">
                                    <Copy />
                                </Button>
                                <Button size="icon-sm" type="button" variant="destructive" aria-label="Remove"
                                    class="cursor-pointer rounded-full" @click="removeItem(idx)">
                                    <Trash />
                                </Button>
                            </ItemActions>
                        </Item>
                    </ItemGroup>
                    <DialogContent :disable-outside-pointer-events="true">
                        <Form v-if="editItem" :initial-values="editItem!" :validation-schema="itemFormSchema"
                            @submit="handleItemDialogSubmit">
                            <DialogHeader>
                                <DialogTitle>
                                    {{ editItemIndex != null ? "Edit" : "Add" }} Item
                                </DialogTitle>
                            </DialogHeader>
                            <div class="space-y-8">
                                <FormField name="id">
                                    <FormItem>
                                        <FormControl>
                                            <Input hidden />
                                        </FormControl>
                                    </FormItem>
                                </FormField>

                                <FormField v-slot="{ componentField }" name="description">
                                    <FormItem>
                                        <FormLabel>Description</FormLabel>
                                        <FormControl>
                                            <Input autofocus v-bind="componentField"
                                                placeholder="Enter item description" />
                                        </FormControl>
                                        <FormMessage />
                                    </FormItem>
                                </FormField>
                                <FormField v-slot="{ componentField }" name="quantity">
                                    <FormItem>
                                        <FormLabel>Quantity</FormLabel>
                                        <FormControl>
                                            <Input type="number" v-bind="componentField"
                                                placeholder="Enter item quantity" />
                                        </FormControl>
                                        <FormMessage />
                                    </FormItem>
                                </FormField>
                                <FormField v-slot="{ componentField }" name="rate">
                                    <FormItem>
                                        <FormLabel>Rate</FormLabel>
                                        <FormControl>
                                            <Input type="number" v-bind="componentField"
                                                placeholder="Enter item rate / unit price" />
                                        </FormControl>
                                        <FormMessage />
                                    </FormItem>
                                </FormField>
                            </div>
                            <DialogFooter class="mt-6">
                                <Button type="submit" class="cursor-pointer">
                                    <Save /> Save
                                </Button>
                            </DialogFooter>
                        </Form>
                    </DialogContent>
                </Dialog>
                <div v-if="editMode" class="grid">
                    <Button type="button" variant="secondary" class="cursor-pointer" @click="openItemDialog(null)">
                        <Plus /> Add Item
                    </Button>
                </div>

                <!-- ============================================================================================= -->
                <!-- Invoice Discounts =========================================================================== -->
                <!-- ============================================================================================= -->
                <h2 class="text-lg font-bold">Invoice Discounts</h2>
                <Dialog v-model:open="discountDialogOpen" :modal="true">
                    <ItemGroup>
                        <Item v-for="(it, idx) in discountsRef" :key="`discount_${it.id}`">
                            <ItemContent>
                                <ItemHeader>{{ it.description }}</ItemHeader>
                                <ItemDescription>{{ makeDiscountText(it) }}</ItemDescription>
                            </ItemContent>
                            <ItemActions v-if="editMode">
                                <Button size="icon-sm" type="button" variant="ghost" aria-label="Edit"
                                    class="cursor-pointer rounded-full" @click="openDiscountDialog(idx)">
                                    <Pencil />
                                </Button>
                                <Button size="icon-sm" type="button" variant="ghost" aria-label="Duplicate Discount"
                                    class="cursor-pointer rounded-full" @click="duplicateDiscount(idx)">
                                    <Copy />
                                </Button>
                                <Button size="icon-sm" type="button" variant="destructive" aria-label="Remove"
                                    class="cursor-pointer rounded-full" @click="removeDiscount(idx)">
                                    <Trash />
                                </Button>
                            </ItemActions>
                        </Item>
                    </ItemGroup>
                    <DialogContent :disable-outside-pointer-events="true">
                        <Form v-if="editDiscount" :initial-values="editDiscount!"
                            :validation-schema="discountFormSchema" @submit="handleDiscountDialogSubmit">
                            <DialogHeader>
                                <DialogTitle>
                                    {{ editDiscountIndex != null ? "Edit" : "Add" }} Item
                                </DialogTitle>
                            </DialogHeader>
                            <div class="space-y-8">
                                <FormField name="id">
                                    <FormItem>
                                        <FormControl>
                                            <Input hidden />
                                        </FormControl>
                                    </FormItem>

                                </FormField>
                                <FormField v-slot="{ componentField }" name="description">
                                    <FormItem>
                                        <FormLabel>Description</FormLabel>
                                        <FormControl>
                                            <Input autofocus v-bind="componentField"
                                                placeholder="Enter discount description" />
                                        </FormControl>
                                        <FormMessage />
                                    </FormItem>
                                </FormField>

                                <FormField v-slot="{ componentField }" name="amount">
                                    <FormItem>
                                        <FormLabel>Amount</FormLabel>
                                        <FormControl>
                                            <Input type="number" v-bind="componentField"
                                                placeholder="Enter discount amount" />
                                        </FormControl>
                                        <FormMessage />
                                    </FormItem>
                                </FormField>

                                <FormField v-slot="{ componentField }" name="type">
                                    <FormItem>
                                        <FormLabel>Type</FormLabel>
                                        <FormControl>
                                            <Select v-bind="componentField">
                                                <FormControl>
                                                    <SelectTrigger class="w-full">
                                                        <SelectValue placeholder="Select a discount type" />
                                                    </SelectTrigger>
                                                </FormControl>
                                                <SelectContent>
                                                    <SelectGroup>
                                                        <SelectItem :value="InvoiceDiscountType.Fixed">
                                                            {{ InvoiceDiscountType.Fixed }}
                                                        </SelectItem>
                                                        <SelectItem :value="InvoiceDiscountType.Percentage">
                                                            {{ InvoiceDiscountType.Percentage }}
                                                        </SelectItem>
                                                    </SelectGroup>
                                                </SelectContent>
                                            </Select>
                                        </FormControl>
                                        <FormMessage />
                                    </FormItem>
                                </FormField>
                            </div>
                            <DialogFooter class="mt-6">
                                <Button type="submit" class="cursor-pointer">
                                    <Save /> Save
                                </Button>
                            </DialogFooter>
                        </Form>
                    </DialogContent>
                </Dialog>

                <div v-if="editMode" class="grid">
                    <Button type="button" variant="secondary" class="cursor-pointer" @click="openDiscountDialog(null)">
                        <Plus /> Add Discount
                    </Button>
                </div>

                <Separator />

                <div class="w-full flex justify-between flex-wrap">
                    <h2 class="text-2xl font-bold">Total:</h2>
                    <p class="text-2xl font-bold">{{ c.format(calculateInvoiceTotal(itemsRef, discountsRef),
                        invoice.currency) }}
                    </p>
                </div>

                <ButtonGroup v-if="editMode" class="w-full" orientation="vertical">
                    <Button type="submit" class="cursor-pointer" :disabled="l.isLoading()">
                        <Spinner v-if="l.isLoading()" />
                        <Save v-else />
                        Save
                    </Button>
                    <Button type="button" class="cursor-pointer" variant="outline"
                        @click="() => { resetForm({ values: initialValues }); editMode = false }">
                        <Undo /> Cancel Edit
                    </Button>
                </ButtonGroup>
                <ButtonGroup v-else class="w-full" orientation="vertical">
                    <Button type="button" class="cursor-pointer" :disabled="isDownloading" @click="downloadInvoice">
                        <template v-if="isDownloading">
                            <Spinner /> Downloading…
                        </template>
                        <template v-else>
                            <Download /> Download
                        </template>
                    </Button>
                    <Button type="button" variant="outline" class="cursor-pointer" @click="editMode = true">
                        <Pencil /> Edit
                    </Button>
                </ButtonGroup>
            </CardContent>
        </Card>
    </Form>

</template>
