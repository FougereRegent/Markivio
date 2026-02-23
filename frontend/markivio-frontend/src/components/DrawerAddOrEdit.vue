<template>
  <Drawer v-model:visible="drawer.drawerState" position="right" :closeOnEscape="false" class="!w-[35rem]">
    <template #container>
      <div class="flex flex-col pt-2 px-5">
        <div class="flex flex-row py-6 border-neutral-300 border-b justify-start">
          <div class="flex items-start">
            <IconField class="ri-pencil-line text-blue-500 text-3xl" />
            <h2 class="text-neutral-600 text-3xl mx-3">Edit Article</h2>
          </div>
          <IconField class="ri-close-line text-neutral-600 text-3xl ml-auto hover:text-neutral-800
                              transition cursor-pointer" v-on:click="drawer.Close" />
        </div>
        <div class="flex flex-col mt-4 gap-4">
          <div class="flex flex-col">
            <label for="source" class="text-neutral-950 text-xl font-medium">Source</label>
            <InputText id="source" v-model.trim="article.source" :invalid="sourceHasError" />
            <Message variant="simple" severity="error" v-show="sourceHasError">{{errors?.source?.[0]?.message}}</Message>
          </div>
          <div class="flex flex-col">
            <label for="title" class="text-neutral-950 text-xl font-medium">Title</label>
            <InputText id="title" v-model.trim="article.title" :invalid="titleHasError" />
            <Message variant="simple" severity="error" v-show="titleHasError">{{errors?.title?.[0]?.message}}</Message>
          </div>
          <div class="flex flex-col">
            <label for="description" class="text-neutral-950 text-xl font-medium">Description</label>
            <Textarea id="description" v-model.trim="article.description" multiple="true" rows="6" />
          </div>
          <div class="flex flex-col">
            <AutoComplete id="tags" />
          </div>
          <div>
            <Button @click="validate">
              Valider
            </Button>
          </div>
        </div>
      </div>
    </template>
  </Drawer>
</template>

<script setup lang="ts">
import { Drawer, IconField, Textarea } from 'primevue';
import { useAddEditDrawer } from '@/stores/add-edit-drawer-store';
import { computed, ref, watch, type Ref } from 'vue';
import { type Article, type Tag, ArticleSchema } from '@/domain/article.models';
import { useZodValidation } from '@/composables/zod.composable';

const article = ref<Article>({
  id: null,
  title: '',
  source: '',
  description: '',
  tags: []
});
const { validate, isValid, errors } = useZodValidation(ArticleSchema, article);
const titleHasError = computed(() => errors.value?.title != undefined)
const sourceHasError = computed(() => errors.value?.source != undefined);
const descriptionHasErorr = computed(() => errors.value?.description != undefined);
const drawer = useAddEditDrawer();
</script>
