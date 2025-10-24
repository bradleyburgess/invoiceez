<script setup lang="ts">
import * as z from "zod";
import { toTypedSchema } from '@vee-validate/zod';
import type { SubmissionHandler } from 'vee-validate'
import { emailSchema } from "~/lib/schemas";
import type { BusinessEditDto } from "~/api-client";
import { Pencil, Save, Trash2, Undo } from "lucide-vue-next";

const formSchema = toTypedSchema(z.object({
    name: z.string().min(1, "Required").max(128, "Name must be less than 128 characters"),
    tagline: z.string().nullish(),
    address: z.string().max(512, "Address must be less than 512 characters"),
    email: emailSchema,
    phone: z.string().max(32, "Phone must be less than 32 characters"),
    website: z.string().max(128, "Website must be less than 128 characters").nullish(),
    defaultCurrency: z.enum(["USD", "EUR", "ZAR"]),
    defaultPaymentInstructions: z.string().nullish(),
}));

const l = useLoading();

const { onSubmit, onDestroy, initialValues, newEntity } = defineProps<{
    onSubmit: SubmissionHandler,
    onDestroy: () => Promise<void>,
    initialValues: BusinessEditDto,
    newEntity: boolean
}>();

const editMode = ref<boolean>(newEntity ? true : false);
</script>

<template>
    <Form v-slot="{ resetForm }" class="h-full flex items-center justify-center" :initial-values="initialValues"
        :validation-schema="formSchema" @submit="onSubmit">
        <Card class="w-full max-w-md mx-auto">
            <CardHeader class="flex justify-between items-center">
                <CardTitle>
                    <span v-if="newEntity">New Business</span>
                    <span v-if="editMode && !newEntity">Edit </span>
                    <span v-if="!newEntity">{{ initialValues.name }}</span>
                </CardTitle>
                <div class="space-x-2">
                    <AlertDialog>
                        <AlertDialogTrigger as-child>
                            <Button type="button" size="icon-sm" variant="outline" class="rounded-full cursor-pointer"
                                :aria-label="`${getDestructiveWord(newEntity, true)} Business`">
                                <Trash2 />
                            </Button>
                        </AlertDialogTrigger>
                        <AlertDialogContent>
                            <AlertDialogHeader>
                                <AlertDialogTitle>{{ getDestructiveWord(newEntity, true) }} Business</AlertDialogTitle>
                                <AlertDialogDescription>Are you sure you want to {{ getDestructiveWord(newEntity) }}
                                    this business?
                                </AlertDialogDescription>
                            </AlertDialogHeader>
                            <AlertDialogFooter>
                                <AlertDialogCancel class="cursor-pointer">Cancel</AlertDialogCancel>
                                <AlertDialogAction
                                    class="cursor-pointer bg-destructive text-white shadow-xs hover:bg-destructive/90 focus-visible:ring-destructive/20 dark:focus-visible:ring-destructive/40 dark:bg-destructive/60"
                                    @click="onDestroy()">
                                    <Trash2 /> {{ getDestructiveWord(newEntity, true) }}
                                </AlertDialogAction>
                            </AlertDialogFooter>
                        </AlertDialogContent>
                    </AlertDialog>
                    <template v-if="editMode && !newEntity">
                        <Button size="icon-sm" variant="outline" class="rounded-full cursor-pointer"
                            aria-label="Cancel Edit"
                            @click="() => { resetForm({ values: initialValues }); editMode = false }">
                            <Undo />
                        </Button>
                    </template>
                    <template v-if="!newEntity && !editMode">
                        <Button size="icon-sm" variant="outline" class="rounded-full cursor-pointer"
                            aria-label="Edit Business" @click="editMode = true">
                            <Pencil />
                        </Button>
                    </template>
                </div>
            </CardHeader>
            <CardContent class="space-y-6">
                <FormField v-slot="{ componentField }" name="name">
                    <FormItem>
                        <FormLabel>Name</FormLabel>
                        <FormControl>
                            <Input v-bind="componentField" placeholder="Enter your business name"
                                :disabled="!editMode" />
                        </FormControl>
                        <FormMessage />
                    </FormItem>
                </FormField>

                <FormField v-slot="{ componentField }" name="tagline">
                    <FormItem>
                        <FormLabel>Tagline</FormLabel>
                        <FormControl>
                            <Input v-bind="componentField" placeholder="Enter your business tagline"
                                :disabled="!editMode" />
                        </FormControl>
                        <FormDescription>Optional</FormDescription>
                        <FormMessage />
                    </FormItem>
                </FormField>

                <FormField v-slot="{ componentField }" name="defaultCurrency">
                    <FormItem>
                        <FormLabel>Default Currency</FormLabel>
                        <Select :disabled="!editMode" v-bind="componentField">
                            <FormControl>
                                <SelectTrigger class="w-full">
                                    <SelectValue placeholder="Select a default currency for the business" />
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

                <FormField v-slot="{ componentField }" name="address">
                    <FormItem>
                        <FormLabel>Address</FormLabel>
                        <FormControl>
                            <Textarea class="resize-none" v-bind="componentField"
                                placeholder="Enter your business address" :disabled="!editMode" />
                        </FormControl>
                        <FormMessage />
                    </FormItem>
                </FormField>

                <FormField v-slot="{ componentField }" name="email">
                    <FormItem>
                        <FormLabel>Email</FormLabel>
                        <FormControl>
                            <Input v-bind="componentField" placeholder="Enter your business email"
                                :disabled="!editMode" />
                        </FormControl>
                        <FormMessage />
                    </FormItem>
                </FormField>

                <FormField v-slot="{ componentField }" name="phone">
                    <FormItem>
                        <FormLabel>Phone</FormLabel>
                        <FormControl>
                            <Input v-bind="componentField" placeholder="Enter your business phone"
                                :disabled="!editMode" />
                        </FormControl>
                        <FormMessage />
                    </FormItem>
                </FormField>

                <FormField v-slot="{ componentField }" name="website">
                    <FormItem>
                        <FormLabel>Website</FormLabel>
                        <FormControl>
                            <Input v-bind="componentField" placeholder="Enter your business website"
                                :disabled="!editMode" />
                        </FormControl>
                        <FormMessage />
                    </FormItem>
                </FormField>

                <FormField v-slot="{ componentField }" name="defaultPaymentInstructions">
                    <FormItem>
                        <FormLabel>Default Payment Instructions</FormLabel>
                        <FormControl>
                            <Textarea class="resize-none h-25" v-bind="componentField"
                                placeholder="Enter your default payment instructions" :disabled="!editMode" />
                        </FormControl>
                        <FormDescription>You can include bank information, PayPal, etc.</FormDescription>
                        <FormMessage />
                    </FormItem>
                </FormField>

                <div v-if="editMode" class="grid">
                    <Button type="submit" class="cursor-pointer" :disabled="l.isLoading()">
                        <Spinner v-if="l.isLoading()" />
                        <Save v-else />
                        Save
                    </Button>
                </div>
            </CardContent>
        </Card>
    </Form>

</template>