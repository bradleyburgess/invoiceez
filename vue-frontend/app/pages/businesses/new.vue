<script setup lang="ts">
import type { SubmissionHandler } from "vee-validate";
import { toast } from 'vue-sonner'
import { ApiResponseCode, type BusinessEditDto } from '~/api-client';
import { makePageTitle } from "~/lib/utils";

definePageMeta({ layout: 'logged-in' });
useHead({
    title: makePageTitle("New Business"),
});

const api = useApi().businesses;
const l = useLoading();
l.setLoading();

const editMode = ref<boolean>(false);

const businessEditDto: BusinessEditDto = reactive({
    id: null,
    name: '',
    address: '',
    email: '',
    phone: '',
    website: '',
    defaultPaymentInstructions: '',
})

let initialValues: BusinessEditDto = {
    name: businessEditDto.name,
    tagline: businessEditDto.tagline,
    address: businessEditDto.address,
    email: businessEditDto.email,
    phone: businessEditDto.phone,
    website: businessEditDto.website,
    defaultPaymentInstructions: businessEditDto.defaultPaymentInstructions,
}

const onDestroy = async () => { navigateTo('/businesses'); }

const onSubmit: SubmissionHandler = async (values, actions) => {
    l.setLoading();
    const _values: BusinessEditDto = (values as BusinessEditDto);
    const dto: BusinessEditDto = { ..._values }
    const { data } = await api.createBusiness(dto);
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
        history.replaceState({}, '', `/businesses/${data.data?.id}`)
        toast.success('Business Created Successfully')
    }
    l.setIdle();
};
l.setIdle();
</script>

<template>
    <div class="h-full">
        <LoadingSpinner v-if="l.isLoading()" />
        <BusinessForm v-else :initial-values="initialValues" :on-submit="onSubmit" :on-destroy="onDestroy"
            :new-entity="true" />
    </div>
</template>
