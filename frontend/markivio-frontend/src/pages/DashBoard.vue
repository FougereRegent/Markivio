<script setup lang="ts">
import ArticleComponent, { type ArticleProps } from '@/components/ArticleComponent.vue';
import { ref, useTemplateRef, watch } from 'vue';
import { useInfiniteScroll } from '@vueuse/core';
import DrawerAddOrEdit from '@/components/DrawerAddOrEdit.vue';
import { useAddEditDrawer } from '@/stores/add-edit-drawer-store';
import { useGetArticles } from '@/composables/artciles.graphql';

let articlesProps = ref<ArticleProps[]>([]);
const drawer = useAddEditDrawer();
const articlesRef = useTemplateRef('articles');
const offset = ref(0);

const { articles, hasNext} = useGetArticles(offset, 15);

const { reset } = useInfiniteScroll(
  articlesRef,
  () => {
    offset.value = articlesProps.value.length;
  },
  {
    distance: 25,
    canLoadMore: () => hasNext.value,
  },
);

watch(articles, (newData) => {
  if (newData) {
    articlesProps.value.push(...newData);
  }
});

watch(
  () => drawer.drawerState,
  (newState, oldState) => {
    if(!newState && oldState) {
    }
  },
  { immediate: true },
);

</script>

<template>
  <div ref="articles" class="flex flex-col gap-3 p-4 h-full overflow-y-scroll">
    <template v-for="item in articlesProps" :key="item.id">
      <ArticleComponent v-bind="item" />
    </template>
  </div>
  <DrawerAddOrEdit />
</template>
