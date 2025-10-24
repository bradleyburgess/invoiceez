<script setup lang="ts">
import { Pencil } from 'lucide-vue-next';
import { ApiResponseCode, type BusinessEditDto } from '~/api-client';
import type { DetailItem } from '~/lib/types';
import { makePageTitle } from '~/lib/utils';

definePageMeta({ layout: 'logged-in' });
useHead({
    title: makePageTitle("Businesses"),
});

const api = useApi().businesses;
const l = useLoading();

l.setLoading();

const { data: response } = await api.getBusinesses();
if (response.code != ApiResponseCode.Ok) {
    throw createError({ statusCode: 500, statusMessage: 'Failed to load businesses' });
}
const businesses = response.data?.businesses;
l.setIdle();

const makeDetails = (business: BusinessEditDto): DetailItem[] => [
    { heading: "Name", content: business.name ?? "" },
    { heading: "Address", content: business.address ?? "" },
    { heading: "Email", content: business.email ?? "" },
    { heading: "Phone", content: business.phone ?? "" },
    { heading: "Website", content: business.website ?? "" }
];
</script>

<template>
    <LoadingSpinner v-if="l.isLoading()" />
    <AppContainer v-else>
        <SpacedColumn>
            <PageTitle>Your Businesses</PageTitle>
            <div v-if="businesses?.length" class="flex flex-wrap gap-8 items-center justify-center">
                <EntityCard v-for="business in businesses" :key="business.id!" :title="business.name!"
                    :button-icon="Pencil" :button-link="`/businesses/${business.id}`" button-text="Manage Business"
                    :details="makeDetails(business)" />
                <EntityAddCard name="business" link="/businesses/new" />
            </div>
            <div v-else class="flex flex-col items-center">
                <p>You do not have any businesses yet.</p>
                <EntityAddCard link="/businesses/new" name="business" />
            </div>
        </SpacedColumn>
    </AppContainer>
</template>