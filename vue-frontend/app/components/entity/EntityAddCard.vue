<script setup lang="ts">
import { Plus } from "lucide-vue-next";

const { name, link } = defineProps<{
    name: string,
    link: string,
}>();
</script>

<template>
    <div class="w-full max-w-sm flex justify-center items-center p-12">
        <NuxtLink :to="link">
            <div class="flex flex-col gap-2">
                <Button variant="outline" class="rounded-full p-8 aspect-square cursor-pointer">
                    <Plus class="size-8 text-zinc-300" />
                </Button>
                <span class="text-xs text-zinc-300">Add a {{ name }}</span>
            </div>
        </NuxtLink>
    </div>
</template>