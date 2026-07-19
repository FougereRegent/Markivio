<script setup lang="ts">
import { toValue } from 'vue';

const props = defineProps<{
  counter: number
}>()

function min99(value?: number): string | number {
 return (value ?? 0) > 99 ? "99+" : value ?? "0";
}
</script>

<template>
  <span class="ml-auto text-sm font-semibold text-neutral-600 bg-neutral-100 rounded-full px-2.5 py-0.5 min-w-7 text-center">
    {{ min99(toValue(props.counter)) }}
  </span>
</template>
