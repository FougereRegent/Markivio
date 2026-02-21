<template>
  <div ref="articles" class="flex flex-col gap-3 p-4 h-full overflow-y-scroll">
    <template v-for="item in src" :key="item.Id">
      <ArticleComponent v-bind="item" />
    </template>
  </div>
</template>

<script setup lang="ts">
import ArticleComponent, { type ArticleProps } from '@/components/ArticleComponent.vue';
import { onActivated, onMounted, onUnmounted, ref, useTemplateRef, watch } from 'vue';
import { useInfiniteScroll } from '@vueuse/core'
import { getMyArticles } from '@/services/article.service';
import { delay, type Subscription } from 'rxjs';
import { useAuthStore } from '@/stores/auth-store';
import { useLoaderStore } from '@/stores/loader-store';
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
      Id: pre.Id,
      Title: pre.Title,
      Description: "Wikipédia est une encyclopédie en ligne collaborative et multilingue créée par Jimmy Wales et Larry Sanger le 15 janvier 2001. Il s'agit d'une œuvre libre, c'est-à-dire que chacun est libre de l'amender et de la rediffuser. Gérée en wiki dans le site web wikipedia.org grâce au logiciel MediaWiki, elle permet à tous les",
      Tags: pre.Tags.map(tag => ({
        Label: tag.Name,
        Color: tag.Color
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
