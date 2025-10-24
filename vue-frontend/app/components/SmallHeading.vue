<script setup lang="ts">
import type { PropType } from 'vue';

const classList = "text-small text-zinc-400";

type HeadingLevel = 'h1' | 'h2' | 'h3' | 'h4' | 'h5' | 'h6';
const { level } = defineProps({
    level: {
        type: String as PropType<HeadingLevel>,
        required: false,
        default: 'h2'
    }
})
</script>
<template>
    <h1 v-if="level == 'h1'" :class="classList">
        <slot />
    </h1>
    <h2 v-if="level == 'h2'" :class="classList">
        <slot />
    </h2>
    <h3 v-if="level == 'h3'" :class="classList">
        <slot />
    </h3>
    <h4 v-if="level == 'h4'" :class="classList">
        <slot />
    </h4>
    <h5 v-if="level == 'h5'" :class="classList">
        <slot />
    </h5>
    <h6 v-if="level == 'h6'" :class="classList">
        <slot />
    </h6>
</template>

<style scoped>
h1,
h2,
h3,
h4,
h5,
h6 {
    text-transform: uppercase;
    font-size: 0.75rem;
    display: flex;
    align-items: end;
    font-weight: 600;
}
</style>