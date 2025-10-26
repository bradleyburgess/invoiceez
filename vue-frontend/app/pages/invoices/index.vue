<script setup lang="ts">
import type { Row } from '@tanstack/vue-table';
import { ApiResponseCode, type InvoiceSummaryDto } from '~/api-client';
import DataTable from '~/components/DataTable.vue';
import { invoiceTableColumns } from '~/components/invoice/columns';
import { makePageTitle } from '~/lib/utils';

definePageMeta({ layout: 'logged-in' });
useHead({
    title: makePageTitle("Invoices"),
});

const api = useApi().invoices;
const l = useLoading();

l.setLoading();

const { data: response } = await api.getUserInvoices();
if (response.code != ApiResponseCode.Ok) {
    throw createError({ statusCode: 500, statusMessage: 'Failed to load invoices' });
}
const invoices = response.data?.invoices?.sort(sortInvoiceSummariesByModified);
l.setIdle();

function rowClickHandler(row: Row<InvoiceSummaryDto>) {
    navigateTo(`/invoices/${row.original.id}`);
}
</script>

<template>
    <LoadingSpinner v-if="l.isLoading()" />
    <AppContainer v-else>
        <SpacedColumn>
            <PageTitle>Your Invoices</PageTitle>
            <div v-if="invoices?.length" class="max-w-6xl mx-auto">
                <DataTable :columns="invoiceTableColumns" :data="invoices ?? []" :row-click-handler="rowClickHandler"
                    :show-new="true" new-link="/invoices/new" />
            </div>
            <div v-else class="flex flex-col items-center">
                <p>You do not have any invoices yet.</p>
                <EntityAddCard link="/invoices/new" name="invoice" />
            </div>
        </SpacedColumn>
    </AppContainer>
</template>