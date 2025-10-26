<script setup lang="ts">
import { ApiResponseCode, InvoicePaymentStatus, type InvoiceEditDto } from '~/api-client';
import { toast } from "vue-sonner";

definePageMeta({ layout: 'logged-in' });
const l = useLoading();
l.setLoading();
const api = useApi();

const { data: businessData } = await useAsyncData('business-list', async () => {
    const businessResponse = await api.businesses.getBusinesses();
    if (businessResponse.data.code == ApiResponseCode.NotFound) {
        throw createError({ statusCode: 404, statusMessage: "Error getting businesses" });
    }
    if (businessResponse.data.code == ApiResponseCode.Unauthorized) {
        throw createError({ statusCode: 403, statusMessage: "You do not have access to make this request" });
    }
    const businesses = businessResponse.data?.data?.businesses;
    return businesses ?? [];
});

const { data: customersData } = await useAsyncData('customers-list', async () => {
    const customerResponse = await api.customers.getCustomers();
    if (customerResponse.data.code == ApiResponseCode.NotFound) {
        throw createError({ statusCode: 404, statusMessage: "Error getting customers" });
    }
    if (customerResponse.data.code == ApiResponseCode.Unauthorized) {
        throw createError({ statusCode: 403, statusMessage: "You do not have access to make this request" });
    }
    const customers = customerResponse.data?.data;
    return customers ?? [];
});

const initialBusiness = computed(() => businessData.value?.length ? businessData.value[0]! : {
    id: undefined,
    name: '',
    address: '',
    email: '',
    phone: '',
    defaultCurrency: undefined,
    defaultPaymentInstructions: undefined,
});

const initialValues: InvoiceEditDto = {
    invoiceNumber: '',
    invoiceDate: new Date().toISOString().split("T")[0],
    currency: initialBusiness.value.defaultCurrency,
    paymentInstructions: initialBusiness.value.defaultPaymentInstructions,
    customerId: undefined,
    customerName: '',
    customerEmail: '',
    customerPhone: '',
    customerAddress: '',
    paymentStatus: InvoicePaymentStatus.Unpaid,
    businessId: initialBusiness.value.id,
    businessName: initialBusiness.value.name,
    businessEmail: initialBusiness.value.email,
    businessPhone: initialBusiness.value.phone,
    businessAddress: initialBusiness.value.address,
    businessWebsite: initialBusiness.value.website,
    businessTagline: initialBusiness.value.tagline,
    items: [],
    discounts: [],
};

async function onSubmit(invoice: InvoiceEditDto) {
    l.setLoading();
    const { data: response } = await api.invoices.createInvoice(invoice);
    if (response.code != ApiResponseCode.Ok) {
        toast.error('Invoice creation failed', {
            description: response.message?.toString(),
            onAutoClose() {
                l.setIdle();
            }
        })
        return;
    }
    l.setIdle();
    navigateTo(`/invoices/${response.data?.id ?? ''}`);
    toast.success('Invoice created successfully!');
}



l.setIdle();
</script>

<template>
    <div class="h-full">
        <LoadingSpinner v-if="l.isLoading()" />
        <template v-else>
            <AppContainer>
                <SpacedColumn>
                    <PageTitle>New Invoice</PageTitle>
                    <InvoiceForm :invoice="initialValues" :on-submit="onSubmit" :businesses="businessData ?? []"
                        :customers="customersData ?? []" :generate-invoice-number="generateInvoiceNumber"
                        :validate-invoice-number="validateInvoiceNumber" />
                </SpacedColumn>
            </AppContainer>
        </template>
    </div>
</template>