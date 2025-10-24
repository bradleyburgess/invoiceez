<script setup lang="ts">
import type { FunctionalComponent } from "vue";
import type { DetailItem } from "~/lib/types";
const { title, details, buttonIcon, buttonText, buttonLink } = defineProps<{
    title: string,
    details: DetailItem[],
    buttonIcon: FunctionalComponent,
    buttonText: string,
    buttonLink: string,
}>();
</script>
<template>
    <Card class="w-full max-w-sm">
        <CardHeader>
            <CardTitle>{{ title }}</CardTitle>
        </CardHeader>
        <CardContent>
            <div class="grid grid-cols-[7rem_1fr]">
                <template v-for="item in details" :key="`${title}-${item.heading}-${item.content}`">
                    <SmallHeading>{{ item.heading }}</SmallHeading>
                    <p v-if="typeof item.content == 'string'">{{ item.content }}</p>
                    <component :is="item.content" v-else />
                </template>
            </div>
        </CardContent>
        <CardFooter>
            <div class="grid w-full">
                <NuxtLink :to="buttonLink" class="w-full">
                    <Button variant="outline" class="w-full cursor-pointer">
                        <component :is="buttonIcon" /> <span>{{ buttonText }}</span>
                    </Button>
                </NuxtLink>
            </div>
        </CardFooter>
    </Card>

</template>