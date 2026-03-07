<script lang="ts" setup>
import { Tag } from 'primevue';
import { contrastColor } from '@/helpers/ui.helpers';

export type ArticleProps = {
  id: string;
  title: string;
  description?: string;
  tags?: Array<{
    label: string;
    color: string;
  }>;
};

const props = withDefaults(defineProps<ArticleProps>(), {
  tags: () => [],
});
</script>

<template>
  <div
    class="flex flex-row rounded-md px-4 py-5 border-gray-200 border bg-white hover:bg-gray-50 hover:border-gray-300"
  >
    <div class="flex flex-col flex-4 w-10 h-full">
      <p class="flex-1 text-3xl mb-1 text-gray-900 font-semibold">{{ props.title }}</p>
      <div class="h-8/12 w-11/12 text-s my-2 text-gray-600">
        <p class="line-clamp-3 text-justify">{{ props.description }}</p>
      </div>
      <div class="flex flex-row flex-2 gap-3">
        <template v-for="item of props.tags" :key="item.label">
          <Tag
            :value="item.label"
            :style="{ backgroundColor: item.color, color: contrastColor(item.color), opacity: 0.55 }"
          />
        </template>
      </div>
    </div>
  </div>
</template>
