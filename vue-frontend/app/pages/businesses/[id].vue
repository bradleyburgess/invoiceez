<script setup lang="ts">
import type { SubmissionHandler } from "vee-validate";
import { toast } from 'vue-sonner'
import { ApiResponseCode, type BusinessEditDto } from '~/api-client';
import { makePageTitle } from "~/lib/utils";

definePageMeta({ layout: 'logged-in' });

const route = useRoute();
const api = useApi().businesses;
const l = useLoading();
l.setLoading();

const editMode = ref<boolean>(false);

const { data: businessData } = await useAsyncData(`business-${route.params.id}`, async () => {
    const businessResponse = await api.getBusiness(route.params.id as string);
    if (businessResponse.data.code == ApiResponseCode.NotFound) {
        throw createError({ statusCode: 404, statusMessage: "Business not found" });
    }
    if (businessResponse.data.code == ApiResponseCode.Unauthorized) {
        throw createError({ statusCode: 403, statusMessage: "You do not have access to this business" });
    }
    const business = businessResponse.data?.data ?? {};
    if (!business) {
        throw createError({ statusCode: 404, statusMessage: "Business not found" });
    }
    return business as BusinessEditDto;
});

useHead({
    title: makePageTitle(businessData?.value?.name ?? "View Business"),
});

const businessEditDto: BusinessEditDto = reactive({
    id: businessData?.value?.id,
    name: businessData?.value?.name ?? null,
    tagline: businessData?.value?.tagline ?? null,
    address: businessData?.value?.address ?? null,
    email: businessData?.value?.email ?? null,
    phone: businessData?.value?.phone ?? null,
    website: businessData?.value?.website,
    defaultCurrency: businessData?.value?.defaultCurrency,
    defaultPaymentInstructions: businessData?.value?.defaultPaymentInstructions,
})

let initialValues: BusinessEditDto = {
    name: businessEditDto.name,
    tagline: businessEditDto.tagline,
    address: businessEditDto.address,
    email: businessEditDto.email,
    phone: businessEditDto.phone,
    website: businessEditDto.website,
    defaultCurrency: businessEditDto.defaultCurrency,
}

const onDestroy = async () => {
    l.setLoading();
    await api.deleteBusiness(businessEditDto.id!);
    navigateTo('/businesses');
}

const onSubmit: SubmissionHandler = async (values, actions) => {
    l.setLoading();
    const _values = (values as BusinessEditDto);
    const dto: BusinessEditDto = { ...businessEditDto, ..._values }
    const { data } = await api.updateBusiness(dto.id!, dto);
    if (data.code != ApiResponseCode.Ok) {
        toast.error('Save Failed', {
            description: data.message?.toString(),
            onAutoClose() {
                l.setIdle();
            }
        })

        if (data.formErrors) {
            Object.keys(data.formErrors).forEach((key) => {
                actions.setFieldError(key as keyof BusinessEditDto, data.formErrors![key]);
            });
        }
    }
    else {
        businessEditDto.name = values.name;
        initialValues = { ..._values }
        editMode.value = false;
        toast.success('Business Updated Successfully')
    }
    l.setIdle();
};
l.setIdle();
</script>

<template>
    <div class="h-full">
        <LoadingSpinner v-if="l.isLoading()" />
        <BusinessForm v-else :initial-values="initialValues" :on-submit="onSubmit" :new-entity="false"
            :on-destroy="onDestroy" />
    </div>
</template>