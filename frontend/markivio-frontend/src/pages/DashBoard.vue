<template>
  <div ref="articles" class="flex flex-col gap-3 p-4 h-full overflow-y-scroll">
    <template v-for="item in src" :key="item.Id">
      <ArticleComponent v-bind="item" />
    </template>
  </div>
  <DrawerAddOrEdit />
</template>

<script setup lang="ts">
import ArticleComponent, { type ArticleProps } from '@/components/ArticleComponent.vue';
import { onActivated, onMounted, onUnmounted, ref, useTemplateRef, watch } from 'vue';
import { useInfiniteScroll } from '@vueuse/core'
import { getMyArticles } from '@/services/article.service';
import { type Subscription } from 'rxjs';
import { useAuthStore } from '@/stores/auth-store';
import { useLoaderStore } from '@/stores/loader-store';
import DrawerAddOrEdit from '@/components/DrawerAddOrEdit.vue';
const { subject, observable } = getMyArticles();

const auth = useAuthStore();
const loader = useLoaderStore();
const articles = useTemplateRef("articles");
const src = ref<ArticleProps[]>([]);
const hasNext = ref(true);

const take = 25;
let page = 0;

let subscription: Subscription | undefined;

watch(() => auth.token, (token) => {
  if (!token) return;

  subject.next({ skip: 0, take });
  page++;
}, { immediate: true });

onMounted(() => {
  subscription = observable
    .subscribe(val => {
      loader.stop();
      const result = val.Data.map(pre => ({
        Id: pre.id,
        Title: pre.title,
        Description: pre.description,
        Tags: pre.tags.map(tag => ({
          Label: tag.name,
          Color: tag.color
        }))
      }));
      hasNext.value = val.HasNextPage;
      src.value.push(...result);
    });
});

onActivated(() => {

})

onUnmounted(() => {
  subscription?.unsubscribe();
});

useInfiniteScroll(
  articles,
  () => {
    if (!hasNext.value) return;

    subject.next({ skip: page * take, take });
    loader.start();
    page++;
  },
  {
    distance: 15,
    canLoadMore: () => hasNext.value,
  }
);
</script>
