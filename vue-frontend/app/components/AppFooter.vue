<script setup lang="ts">
const year = new Date().getFullYear();
const { appVersion } = useRuntimeConfig().public;
</script>

<template>
    <footer>
        <div class="container mx-auto px-2 py-6">
            <p class="text-zinc-400 text-sm text-center">
                Copyright © {{ year }} Invoiceez <InlineSeparator /> v{{ appVersion }}
            </p>
        </div>
    </footer>
</template>