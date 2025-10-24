<script setup lang="ts">
import { toast } from 'vue-sonner';
import { ApiResponseCode, type InvoiceEditDto } from '~/api-client';

definePageMeta({ layout: 'logged-in' });
const l = useLoading();
l.setLoading();
const route = useRoute();
const api = useApi();
const id = route.params.id as string;
const response = await api.invoices.getInvoiceById(id);
const invoice = response.data?.data;
if (!invoice) {
    throw createError({ statusCode: 404, statusMessage: 'Invoice not found' });
}

const { data: businessData } = await useAsyncData('business-list', async () => {
    const businessResponse = await api.businesses.getBusinesses();
    if (businessResponse.data.code == ApiResponseCode.NotFound) {
        throw createError({ statusCode: 404, statusMessage: "Error getting businesses" });
    }
    if (businessResponse.data.code == ApiResponseCode.Unauthorized) {
        throw createError({ statusCode: 403, statusMessage: "You do not have access to make this request" });
    }
    const businesses = businessResponse.data?.data?.businesses;
    if (!businesses) {
        throw createError({ statusCode: 404, statusMessage: "Error getting businesses" });
    }
    return businesses;
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

const initialValues: InvoiceEditDto = { ...invoice };

async function onSubmit(invoice: InvoiceEditDto) {
    l.setLoading();
    const { data: response } = await api.invoices.updateInvoice(invoice.id!, invoice);
    if (response.code != ApiResponseCode.Ok) {
        toast.error('Invoice saving failed', {
            description: response.message?.toString(),
            onAutoClose() {
                l.setIdle();
            }
        })
        return;
    }
    l.setIdle();
    navigateTo('/invoices');
    toast.success('Invoice updated successfully!');
}

l.setIdle();
</script>
<template>
    <div class="h-full">
        <LoadingSpinner v-if="l.isLoading()" />
        <template v-else>
            <AppContainer>
                <SpacedColumn>
                    <PageTitle>{{ invoice.invoiceNumber }}</PageTitle>
                    <InvoiceForm :invoice="initialValues" :on-submit="onSubmit" :businesses="businessData ?? []"
                        :customers="customersData ?? []" :generate-invoice-number="generateInvoiceNumber"
                        :validate-invoice-number="validateInvoiceNumber" />
                </SpacedColumn>
            </AppContainer>
        </template>
    </div>
</template>