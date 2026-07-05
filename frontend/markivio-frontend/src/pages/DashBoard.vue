<script setup lang="ts">
import ArticleComponent, { type ArticleProps } from '@/features/article/components/ArticleComponent.vue'
import { onMounted, ref, useTemplateRef, watch } from 'vue'
import { useInfiniteScroll } from '@vueuse/core'
import DrawerAddOrEdit from '@/features/drawer/components/DrawerAddOrEdit.vue'
import { useAddEditDrawer } from '@/stores/add-edit-drawer-store'
import { useArticleStore } from '@/stores/article-store'
import { storeToRefs } from 'pinia'

const articlesProps = ref<ArticleProps[]>([])
const drawer = useAddEditDrawer()
const articlesRef = useTemplateRef('articles')
const articleStore = useArticleStore();
const { executeQuery } = useArticleStore();
const { offset, articles, hasNext } = storeToRefs(articleStore);

const { reset } = useInfiniteScroll(
  articlesRef,
  () => {
    offset.value = articlesProps.value.length;
  },
  {
    distance: 10,
    canLoadMore: () => hasNext.value,
  },
)

watch(
  articles,
  (newData) => {
    if (!newData) return

    if (offset.value === 0) {
      articlesProps.value = newData
    } else {
      articlesProps.value.push(...newData)
    }
  },
  { immediate: true },
)

watch(
  () => drawer.drawerState,
  (newState, oldState) => {
    if (!newState && oldState) {
      articlesProps.value = []
      reset()
      offset.value = 0;
      executeQuery();
    }
  },
  { immediate: true },
)

onMounted(() => {
  offset.value = 0;
})
</script>

<template>
  <div ref="articles" class="flex flex-col gap-3 p-4 h-full overflow-y-scroll">
    <template v-for="item in articlesProps" :key="item.id">
      <ArticleComponent v-bind="item" />
    </template>
  </div>
  <DrawerAddOrEdit />
</template>
