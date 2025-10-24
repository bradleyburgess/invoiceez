<script setup lang="ts">
import { ChevronRight } from "lucide-vue-next";
import type { FunctionalComponent } from "vue";
const { link, title, badge = null } = defineProps<{
    title: string,
    link: string,
    badge?: Component | FunctionalComponent | null,
}>();
</script>
<template>
    <Item class="transition-colors, hover:bg-secondary cursor-pointer" @click="() => navigateTo(link)">
        <ItemContent>
            <ItemTitle>
                <NuxtLink :to="link">
                    {{ title }}
                </NuxtLink>
                <component :is="badge" v-if="badge" />
            </ItemTitle>
            <ItemDescription>
                <slot name="description" />
            </ItemDescription>
        </ItemContent>
        <ItemActions>
            <ChevronRight class="text-zinc-300" :size="20" />
        </ItemActions>
    </Item>
</template>