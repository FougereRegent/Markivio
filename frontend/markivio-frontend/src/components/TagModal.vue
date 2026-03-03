<template>
  <Popover ref="op">

  </Popover>
</template>

<script setup lang="ts">
import { useTemplateRef } from 'vue';

const emit = defineEmits<{(e: 'toggle'): void}>();

const op = useTemplateRef("op");

const toggle = (event) => {
  op.value?.toggle(event);
}
</script>
