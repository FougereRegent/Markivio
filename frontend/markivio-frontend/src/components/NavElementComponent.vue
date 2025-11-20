<template>
  <div class="flex flex-row py-3 px-4 rounded-md hover:bg-neutral-100 hover:cursor-pointer
    hover:nth-last-
    hover:text-neutral-900
    items-center" @click="$emit('click-element-nav')">
    <i :class="iconClass" class="text-2xl text-neutral-700 content-center" :hidden="!dispayIcon" />
    <div class="size-4 rounded-full opacity-65" :style="styleTags" :hidden="!displayTags"></div>
    <p class="col-start-2 col-span-2 text-1xl font-semibold ml-3.5 text-neutral-700 content-center">
      {{ name }}
    </p>
  </div>
</template>

<script setup lang="ts">
import { computed, reactive } from 'vue';


interface NavElementProps {
  name?: string,
  iconClass?: string,
  tagColor?: string,
};

const { name, iconClass, tagColor: tagsColor } = defineProps<NavElementProps>();
const dispayIcon = computed(() => typeof iconClass != 'undefined' && iconClass != "");
const displayTags = computed(() => typeof tagsColor != 'undefined' && tagsColor != "");
const styleTags = reactive({
  backgroundColor: tagsColor
})
</script>
