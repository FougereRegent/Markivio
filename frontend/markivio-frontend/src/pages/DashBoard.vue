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

const val = getMyArticles();

const articles = useTemplateRef("articles");
const src = ref<Array<ArticleProps>>([
  {
    Id: "10",
    Description: "Tes",
    Title: "Test",
    Tags: [
      {
        Label: "Info",
        Color: "#FF00FF"
      },
      {
        Label: "Math",
        Color: "#FFFF00"
      },
      {
        Label: "Français",
        Color: "#000000"
      }
    ]
  },
]);

const { reset } = useInfiniteScroll(articles,
  () => {
    src.value.push(src.value[0] as ArticleProps);
  },
  {
    distance: 5,
    canLoadMore: () => true,
  }
)
</script>
