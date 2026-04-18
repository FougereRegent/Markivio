<script setup lang="ts">
import { Dialog } from 'primevue'

const props = defineProps({
  id: String,
  title: String,
  source: String,
})

const visible = defineModel('visible', { required: true, type: Boolean })

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
      <div class="h-full w-full flex justify-center">
        <iframe
          ref="iframeRef"
          id="inlineFrameExample"
          title="Exemple de cadre intégré"
          class="h-11/12 w-11/12 border border-gray-300 rounded-xl"
          loading="lazy"
          :src="props.source"
        >
        </iframe>
      </div>
    </template>
  </Dialog>
</template>
