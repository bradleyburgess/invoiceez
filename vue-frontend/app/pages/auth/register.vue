<script setup lang="ts">
import { CircleArrowRight } from 'lucide-vue-next'
import { useForm } from 'vee-validate'
import { toTypedSchema } from '@vee-validate/zod'
import * as z from 'zod'
import {
    FormControl,
    FormField,
    FormItem,
    FormLabel,
    FormMessage
} from '@/components/ui/form'
import * as schemas from '~/lib/schemas'
import { makePageTitle } from '~/lib/utils'
import { ApiResponseCode } from '~/api-client'
import { toast } from 'vue-sonner'

definePageMeta({
    layout: 'centered',
});
useHead({
    title: makePageTitle("Register"),
});

const auth = useAuth();
const allowRegistration = await auth.checkRegistrationAccepted();
if (!allowRegistration) navigateTo('/login');

const formSchema = toTypedSchema(z.object({
    email: schemas.emailSchema,
    password: schemas.strongPasswordSchema,
    confirmPassword: schemas.passwordSchema,
    firstName: schemas.nameSchema,
    lastName: schemas.nameSchema,
}).refine((data) => data.password === data.confirmPassword, {
    message: "Passwords do not match",
    path: ["confirmPassword"],
}));

const form = useForm({
    validationSchema: formSchema,
});

const registerLoading = ref<boolean>(false);
const registerDisabled = ref<boolean>(false);

const onSubmit = form.handleSubmit(async (values) => {
    registerLoading.value = true;
    registerDisabled.value = true;
    const response = await auth.register({
        email: values.email,
        password: values.password,
        confirmPassword: values.confirmPassword,
        firstName: values.firstName,
        lastName: values.lastName,
    });
    registerLoading.value = false;
    if (auth.isLoggedIn()) {
        return navigateTo('/');
    }
    if (response && response.code !== ApiResponseCode.Ok) {
        toast.error('Registration failed', {
            description: response.message?.toString(),
            onAutoClose() {
                registerDisabled.value = false;
            }
        })
    }
});
</script>

<template>
    <div class="w-full max-w-sm p-2 space-y-8">
        <div class="px-16">
            <InvoiceezLogo />
        </div>
        <Card>
            <CardHeader>
                <CardTitle>Register</CardTitle>
                <CardDescription>Create an Invoiceez account</CardDescription>
            </CardHeader>
            <CardContent>
                <form class="space-y-8" @submit.prevent="onSubmit">
                    <FormField v-slot="{ field }" name="email">
                        <FormItem>
                            <FormLabel>Email</FormLabel>
                            <FormControl>
                                <Input v-bind="field" placeholder="Enter your email" />
                            </FormControl>
                            <FormMessage />
                        </FormItem>
                    </FormField>
                    <FormField v-slot="{ field }" name="password">
                        <FormItem>
                            <FormLabel>Password</FormLabel>
                            <FormControl>
                                <Input type="password" v-bind="field" placeholder="Enter your password" />
                            </FormControl>
                            <FormMessage />
                        </FormItem>
                    </FormField>
                    <FormField v-slot="{ field }" name="confirmPassword">
                        <FormItem>
                            <FormLabel>Confirm Password</FormLabel>
                            <FormControl>
                                <Input type="password" v-bind="field" placeholder="Enter your password again" />
                            </FormControl>
                            <FormMessage />
                        </FormItem>
                    </FormField>
                    <FormField v-slot="{ field }" name="firstName">
                        <FormItem>
                            <FormLabel>First Name</FormLabel>
                            <FormControl>
                                <Input v-bind="field" placeholder="Enter your first name" />
                            </FormControl>
                            <FormMessage />
                        </FormItem>
                    </FormField>
                    <FormField v-slot="{ field }" name="lastName">
                        <FormItem>
                            <FormLabel>Last Name</FormLabel>
                            <FormControl>
                                <Input v-bind="field" placeholder="Enter your last name" />
                            </FormControl>
                            <FormMessage />
                        </FormItem>
                    </FormField>
                    <div class="grid">
                        <Button type="submit" :disabled="registerDisabled || registerLoading">
                            <Spinner v-if="registerLoading" />
                            <CircleArrowRight v-else />
                            Register
                        </Button>
                    </div>
                    <div>
                        <p class="text-center text-sm text-zinc-400">Already have an account? <NuxtLink
                                class=" text-primary hover:underline" to="/auth/login">Login</NuxtLink>
                        </p>
                    </div>
                </form>
            </CardContent>
        </Card>
    </div>
</template>