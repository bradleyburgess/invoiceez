<script setup lang="ts">
import { Pencil } from 'lucide-vue-next';
import { ApiResponseCode, InvoicePaymentStatus, type InvoiceSummaryDto } from '~/api-client';
import PaidBadge from '~/components/PaidBadge.vue';
import UnpaidBadge from '~/components/UnpaidBadge.vue';
import type { DetailItem } from '~/lib/types';
import { makePageTitle } from '~/lib/utils';

definePageMeta({ layout: 'logged-in' });
useHead({
    title: makePageTitle("Invoices"),
});

const api = useApi().invoices;
const l = useLoading();
const c = useCurrency();

l.setLoading();

const { data: response } = await api.getUserInvoices();
if (response.code != ApiResponseCode.Ok) {
    throw createError({ statusCode: 500, statusMessage: 'Failed to load invoices' });
}
const invoices = response.data?.invoices?.sort(sortInvoiceSummariesByModified);
l.setIdle();

const makeDetails = (invoice: InvoiceSummaryDto): DetailItem[] => [
    { heading: "Invoice #", content: invoice.invoiceNumber ?? "" },
    { heading: "Date", content: displayDate(invoice.invoiceDate) ?? "" },
    { heading: "Customer", content: invoice.customerName ?? "" },
    { heading: "Amount", content: c.format(invoice.totalAmount ?? 0, invoice.currency) },
    { heading: "Status", content: invoice.paymentStatus === InvoicePaymentStatus.Paid ? PaidBadge : UnpaidBadge },
];
</script>

<template>
    <LoadingSpinner v-if="l.isLoading()" />
    <AppContainer v-else>
        <SpacedColumn>
            <PageTitle>Your Invoices</PageTitle>
            <div v-if="invoices?.length" class="flex flex-wrap gap-8 items-center justify-center">
                <EntityCard v-for="invoice in invoices" :key="invoice.id!" :title="invoice.invoiceNumber!"
                    :button-icon="Pencil" :button-link="`/invoices/${invoice.id}`" button-text="Manage Invoice"
                    :details="makeDetails(invoice)" />
                <EntityAddCard name="invoice" link="/invoices/new" />
            </div>
            <div v-else class="flex flex-col items-center">
                <p>You do not have any invoices yet.</p>
                <EntityAddCard link="/invoices/new" name="invoice" />
            </div>
        </SpacedColumn>
    </AppContainer>
</template>