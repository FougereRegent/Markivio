<script setup lang="ts">
import { Dialog } from 'primevue'
import { computed } from 'vue'
import MarkdownRender from './MarkdownRender.vue'

const props = defineProps({
  id: String,
  title: String,
  source: String,
  content: String,
})

const visible = defineModel('visible', { required: true, type: Boolean })
const testComputed = computed(() => props.content)

function showSourceArticle() {
  window.open(props.source, '_blank')?.focus()
}
</script>
<template>
  <Dialog
    v-model:visible="visible"
    modal
    :header="props.title"
    :style="{
      width: '110rem',
      height: '55rem',
    }"
  >
    <template #header>
      <div class="inline-flex items-center justify-between gap-2 w-full">
        <span class="font-bold whitespace-nowrap capitalize text-2xl">{{ props.title }}</span>
        <Button
          class="mx-6"
          icon="ri-article-line"
          severity="secondary"
          @click="showSourceArticle"
        />
      </div>
    </template>
    <template #default>
      <MarkdownRender :content="props.content ?? ''" />
    </template>
  </Dialog>
</template>
