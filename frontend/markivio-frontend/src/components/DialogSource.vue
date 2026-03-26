<script setup lang="ts">
import { Dialog } from 'primevue';

const props = defineProps({
  title: String,
  url: String
});

const visible = defineModel("visible", {required: true, type: Boolean});

</script>
<template>
  <Dialog v-model:visible="visible" modal :header="props.title" :style="{ width: '110rem', height:
    '55rem'}">
    <template #header>
        <div class="inline-flex items-center justify-center gap-2">
          <span class="font-bold whitespace-nowrap">{{props.title}}</span>
        </div>
    </template>
    <template #default>
      <div class="h-full w-full flex justify-center">
        <iframe
          id="inlineFrameExample"
          loading="lazy"
          title="Exemple de cadre intégré"
          class="h-11/12 w-11/12 border border-gray-300 rounded-xl"
          src="https://fr.wikipedia.org/wiki/Blog">
        </iframe>
      </div>
    </template>
  </Dialog>
</template>
