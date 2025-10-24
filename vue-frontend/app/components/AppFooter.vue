<script setup lang="ts">
const year = new Date().getFullYear();
</script>

<template>
    <footer>
        <div class="container mx-auto px-2 py-6">
            <p class="text-zinc-300 text-sm text-center">
                Copyright © {{ year }} Invoiceez.
            </p>
        </div>
    </footer>
</template>