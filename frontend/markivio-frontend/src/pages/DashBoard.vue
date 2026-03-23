<script setup lang="ts">
import ArticleComponent, { type ArticleProps } from '@/components/ArticleComponent.vue';
import { ref, useTemplateRef, watch } from 'vue';
import { useInfiniteScroll } from '@vueuse/core';
import DrawerAddOrEdit from '@/components/DrawerAddOrEdit.vue';
import { useAddEditDrawer } from '@/stores/add-edit-drawer-store';

const drawer = useAddEditDrawer();
const articlesRef = useTemplateRef('articles');
const articlesSrc = ref<ArticleProps[]>([]);

const { reset } = useInfiniteScroll(
  articlesRef,
  () => {
  },
  {
    distance: 15,
  },
);

watch(
  () => drawer.drawerState,
  (newState, oldState) => {
    if(!newState && oldState) {
      articlesSrc.value = [];
    }
  },
  { immediate: true },
);

</script>

<template>
  <div ref="articles" class="flex flex-col gap-3 p-4 h-full overflow-y-scroll">
    <template v-for="item in articlesSrc" :key="item.id">
      <ArticleComponent v-bind="item" />
    </template>
  </div>
  <DrawerAddOrEdit />
</template>
