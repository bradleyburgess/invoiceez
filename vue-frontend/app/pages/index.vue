<script setup lang="ts">
import { ClipboardList, Plus } from "lucide-vue-next"
import { InvoicePaymentStatus, type InvoiceSummaryDto } from "~/api-client";
import PaidBadge from "~/components/PaidBadge.vue";
import UnpaidBadge from "~/components/UnpaidBadge.vue";
definePageMeta({
    layout: 'logged-in',
});

const api = useApi();
const l = useLoading();
const c = useCurrency();

l.setLoading();

const invoicesPromise = api.invoices.getUserInvoices();
const businessesPromise = api.businesses.getBusinesses();

const [invoicesResponse, businessesResponse] = await Promise.all([invoicesPromise, businessesPromise]);
const invoices = (invoicesResponse.data?.data?.invoices)?.sort(sortInvoiceSummariesByModified).slice(0, 5);
const businesses = businessesResponse.data?.data?.businesses;

const numBusinesses = businesses?.length || 0;
const numInvoices = invoices?.length || 0;

const getInvoiceBadge = (invoice: InvoiceSummaryDto): Component | null =>
    invoice.paymentStatus == InvoicePaymentStatus.Paid ? PaidBadge : UnpaidBadge;

l.setIdle();
</script>

<template>
    <LoadingSpinner v-if="l.isLoading()" />
    <AppContainer v-else>
        <SpacedColumn>
            <PageTitle>Dashboard</PageTitle>
            <div class="flex flex-wrap justify-center items-start gap-12">
                <Card class="w-full max-w-md">
                    <CardHeader>
                        <CardTitle class="text-lg">Your Businesses</CardTitle>
                    </CardHeader>
                    <CardContent>
                        <div v-if="numBusinesses > 0">
                            <DashboardItem v-for="business in businesses" :key="business.id"
                                :title="business.name ?? ''" :link="`/businesses/${business.id}`">
                                <template #description>{{ business.email }}</template>
                            </DashboardItem>
                        </div>
                        <div v-else>
                            <p>You do not have any businesses yet.</p>
                        </div>
                    </CardContent>
                    <CardFooter>
                        <ButtonGroup class="w-full">
                            <Button v-if="numBusinesses > 0" variant="outline" class="flex-1 cursor-pointer"
                                @click="navigateTo('/businesses')">
                                <ClipboardList />
                                <NuxtLink to="/businesses">
                                    Manage Businesses
                                </NuxtLink>
                            </Button>
                            <Button variant="outline" class="cursor-pointer flex-1"
                                @click="navigateTo('/businesses/new')">
                                <Plus />
                                <NuxtLink to="/businesses/new">
                                    Add Business
                                </NuxtLink>
                            </Button>
                        </ButtonGroup>
                    </CardFooter>
                </Card>
                <Card class="w-full max-w-md">
                    <CardHeader>
                        <CardTitle class="text-lg">Your Recent Invoices</CardTitle>
                    </CardHeader>
                    <CardContent>
                        <div v-if="numInvoices > 0">
                            <DashboardItem v-for="invoice in invoices" :key="invoice.id"
                                :title="`${invoice.invoiceNumber}`" :link="`/invoices/${invoice.id}`"
                                :badge="getInvoiceBadge(invoice)">
                                <template #description>
                                    {{ invoice.customerName }}
                                    <InlineSeparator />
                                    {{ dateString(invoice.invoiceDate) }}
                                    <InlineSeparator />
                                    {{ c.format(invoice.totalAmount ?? 0, invoice.currency) }}
                                </template>
                            </DashboardItem>
                        </div>
                        <div v-else>
                            <p>You do not have any invoices yet.</p>
                        </div>
                    </CardContent>
                    <CardFooter>
                        <ButtonGroup class="w-full">
                            <Button v-if="numInvoices > 0" variant="outline" class="flex-1 cursor-pointer"
                                @click="navigateTo('/invoices')">
                                <ClipboardList />
                                <NuxtLink to="/invoices">
                                    Manage Invoices
                                </NuxtLink>
                            </Button>
                            <Button variant="outline" class="cursor-pointer flex-1"
                                @click="navigateTo('/invoices/new')">
                                <Plus />
                                <NuxtLink to="/invoices/new">
                                    Add Invoice
                                </NuxtLink>
                            </Button>
                        </ButtonGroup>
                    </CardFooter>
                </Card>
            </div>
        </SpacedColumn>
    </AppContainer>
</template>