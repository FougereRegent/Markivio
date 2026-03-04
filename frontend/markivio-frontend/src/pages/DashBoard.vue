<script setup lang="ts">
import ArticleComponent, { type ArticleProps } from '@/components/ArticleComponent.vue';
import { onMounted, onUnmounted, ref, useTemplateRef, watch } from 'vue';
import { useInfiniteScroll } from '@vueuse/core';
import { getMyArticles } from '@/services/article.service';
import type { Subscription } from 'rxjs';
import { useAuthStore } from '@/stores/auth-store';
import { useLoaderStore } from '@/stores/loader-store';
import DrawerAddOrEdit from '@/components/DrawerAddOrEdit.vue';
import { useAddEditDrawer } from '@/stores/add-edit-drawer-store';

const { subject, observable } = getMyArticles();

const auth = useAuthStore();
const loader = useLoaderStore();
const drawer = useAddEditDrawer();
const articlesRef = useTemplateRef('articles');
const articles = ref<ArticleProps[]>([]);
const hasNext = ref(true);

const take = 25;
let page = 0;

let subscription: Subscription | undefined;

watch(
  () => auth.token,
  (token) => {
    if (!token) return;
    subject.next({ skip: 0, take });
    page++;
  },
  { immediate: true },
);

onMounted(() => {
  subscription = observable.subscribe((val) => {
    loader.stop();
    const result = val.data.map((item) => ({
      id: item.id,
      title: item.title,
      description: item.description,
      tags: item.tags.map((tag) => ({
        label: tag.name,
        color: tag.color,
      })),
    }));
    hasNext.value = val.hasNextPage;
    articles.value.push(...result);
  });
});

onUnmounted(() => {
  subscription?.unsubscribe();
});

const { reset } = useInfiniteScroll(
  articlesRef,
  () => {
    if (!hasNext.value) return;
    subject.next({ skip: page * take, take });
    loader.start();
    page++;
  },
  {
    distance: 15,
    canLoadMore: () => hasNext.value,
  },
);

watch(
  () => drawer.drawerState,
  () => {
    articles.value = [];
    page = 0;
    hasNext.value = true;
    reset();
  },
  { immediate: true },
);
</script>

<template>
  <div ref="articles" class="flex flex-col gap-3 p-4 h-full overflow-y-scroll">
    <template v-for="item in articles" :key="item.id">
      <ArticleComponent v-bind="item" />
    </template>
  </div>
  <DrawerAddOrEdit />
</template>
