<script setup>
import { Toaster } from '@/components/ui/sonner'
import 'vue-sonner/style.css';
import { useHead } from "@unhead/vue";

const colorMode = useColorMode();

useHead({
    title: "Invoiceez",
    htmlAttrs: {
        lang: "en",
        class: 'h-screen min-h-screen',
    },
    bodyAttrs: {
        class: "h-screen min-h-screen",
    },
    link: [{ rel: "icon", type: "image/svg+xml", href: "/favicon.svg" }],
    meta: [
        { name: 'description', value: 'Invoiceez - simple invoicing made simple' },
    ]
});

useSeoMeta({
    title: 'Invoiceez',
    ogTitle: 'Invoiceez',
    description: 'Simple invoicing made simple',
    ogDescription: 'Simple invoicing made simple',
    // ogImage: 'https://invoiceez.bombarde32.net/',
    twitterCard: 'summary_large_image',
})
</script>

<template>
    <NuxtRouteAnnouncer />
    <Toaster :theme="colorMode.value" :toast-options="{
        closeButton: true,
        closeButtonPosition: 'top-right',
    }" />
    <NuxtLayout>
        <NuxtPage />
    </NuxtLayout>
</template>
