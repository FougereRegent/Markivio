<template>
  <div ref="articles" class="flex flex-col gap-2 p-4">
    <template v-for="item in src" :key="item.Id">
      <ArticleComponent v-bind="item" />
    </template>
  </div>
</template>

<script setup lang="ts">
import ArticleComponent, { type ArticleProps } from '@/components/ArticleComponent.vue';
import { ref, useTemplateRef } from 'vue';
import { useInfiniteScroll } from '@vueuse/core'
import { getMyArticles } from '@/services/article.service';
import type { ArticleInformation } from "@/domain/article.models";
import type { OffsetPagination } from "@/domain/pagination.models";

const { subject, observable } = getMyArticles();

subject.next({ skip: 0, take: 10 });
const articles = useTemplateRef("articles");
const src = ref<Array<ArticleProps>>([]);

observable.subscribe(val => {
  debugger;
  const v = val as OffsetPagination<ArticleInformation>;
  src.value.push(...v.Data);
});
const { reset } = useInfiniteScroll(articles,
  () => {
    subject.next({ skip: 0, take: 10 });
  },
  {
    distance: 5,
    canLoadMore: () => true,
  }
)
</script>
