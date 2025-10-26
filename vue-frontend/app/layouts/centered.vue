<template>
    <div class="relative">
        <main class="min-h-screen flex items-center justify-center py-12">
            <slot />
        </main>
        <div class="absolute bottom-0 w-full">
            <AppFooter />
        </div>
    </div>
</template>