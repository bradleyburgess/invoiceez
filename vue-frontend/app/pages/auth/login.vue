<script setup lang="ts">
import { toast } from 'vue-sonner'
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
import { emailSchema, passwordSchema } from '~/lib/schemas'

definePageMeta({
    layout: 'centered',
});
useHead({
    title: "Login | Invoiceez",
});

const auth = useAuth();
if (auth.isLoggedIn()) {
    navigateTo('/');
}

const allowRegistration = useRegistrationAllowed();

const l = useLoading();

const formSchema = toTypedSchema(z.object({
    email: emailSchema,
    password: passwordSchema,
}));

const form = useForm({
    validationSchema: formSchema,
});

const onSubmit = form.handleSubmit(async (values) => {
    l.setLoading();
    try {
        const response = await auth.login({ email: values.email, password: values.password });
        if (auth.isLoggedIn()) {
            return navigateTo('/');
        }
        toast.error('Login Failed', {
            description: response.message?.toString(),
            onAutoClose() {
                l.setIdle();
            }
        })
    } catch (e) {
        toast.error('Login Failed', {
            description: (e as Error)?.message?.toString(),
            onAutoClose() { l.setIdle(); }
        })
    }
    l.setIdle();
});
</script>

<template>
    <div class="w-full max-w-sm p-2 space-y-8">
        <div class="px-16">
            <InvoiceezLogo />
        </div>
        <Card>
            <CardHeader>
                <CardTitle>Login</CardTitle>
                <CardDescription>Login to your Invoiceez account</CardDescription>
            </CardHeader>
            <CardContent>
                <form class="space-y-8" @submit.prevent="onSubmit">
                    <FormField v-slot="{ field }" name="email">
                        <FormItem>
                            <FormLabel>Email</FormLabel>
                            <FormControl>
                                <Input autofocus v-bind="field" placeholder="Enter your email" />
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
                    <div class="grid">
                        <Button type="submit" :disabled="l.isLoading()">
                            <CircleArrowRight v-if="!l.isLoading()" />
                            <Spinner v-else />
                            Login
                        </Button>
                    </div>
                    <div v-if="allowRegistration">
                        <p class="text-center text-sm text-zinc-400">Don't have an account? <NuxtLink
                                class=" text-primary hover:underline" to="/auth/register">Register</NuxtLink>
                        </p>
                    </div>
                </form>
            </CardContent>
        </Card>
    </div>
</template>