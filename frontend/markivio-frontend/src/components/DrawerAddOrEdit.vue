<script setup lang="ts">
import { Drawer, IconField, Textarea, type AutoCompleteOptionSelectEvent, useToast } from 'primevue';
import { useAddEditDrawer } from '@/stores/add-edit-drawer-store';
import { computed, onUnmounted, ref, toValue, watch } from 'vue';
import { type Article, type Tag, ArticleSchema } from '@/domain/article.models';
import { useZodValidation } from '@/composables/zod.composable';
import { getTags } from '@/services/tags.service';
import { createArticle } from '@/services/article.service';
import type { Subscription } from 'rxjs';
import { mapGraphqlError } from '@/errors/errors';
import { CONST } from '@/config/constante.config';
import type { CombinedGraphQLErrors } from '@apollo/client';

const toast = useToast();

const article = ref<Article>({
  id: null,
  title: '',
  source: '',
  description: '',
  tags: [],
});

const { validate, errors } = useZodValidation(ArticleSchema, article);

const titleHasError = computed(() => errors.value?.title != undefined);
const sourceHasError = computed(() => errors.value?.source != undefined);
const drawer = useAddEditDrawer();

const refSuggestion = ref([] as Tag[]);

const { subject, observable } = getTags();
const subscribe = observable.subscribe((page) => {
  refSuggestion.value = page.data;
});

const search = () => {
  subject.next({ skip: 0, take: 15 });
};

const selectedItems = (event: AutoCompleteOptionSelectEvent) => {
  const selectedElement = event.value as Tag;
  article.value.tags.push(selectedElement);
};

const removeChip = (tag: Tag) => {
  article.value.tags = article.value.tags.filter((item) => item != tag);
};

const validateAndSend = () => {
  let sub: Subscription | null = null;
  if (validate()) {
    sub = createArticle(toValue(article)).subscribe({
      next: () => {
        drawer.close();
        toast.add({
          severity: 'success',
          summary: 'Success',
          life: CONST.toastTime,
          group: 'tl',
        });
        sub?.unsubscribe();
      },
      error: (err) => {
        const errs = err as CombinedGraphQLErrors;
        for (const error of errs.errors) {
          const mapped = mapGraphqlError(error.extensions?.code as string);
          toast.add({
            severity: 'error',
            summary: 'Error',
            detail: mapped.message,
            life: CONST.toastTime,
            group: 'tl',
          });
        }
        sub?.unsubscribe();
      },
    });
  }
};

watch(
  () => drawer.drawerState,
  (current) => {
    if (!current) return;

    refSuggestion.value = [];
    article.value = {
      id: null,
      tags: [],
      source: '',
      title: '',
      description: '',
    };
  },
  { immediate: true, deep: true },
);

onUnmounted(() => {
  subscribe.unsubscribe();
});
</script>

<template>
  <Drawer v-model:visible="drawer.drawerState" position="right" :closeOnEscape="false" class="!w-[35rem]">
    <template #container>
      <div class="flex flex-col pt-2 px-5">
        <div class="flex flex-row py-6 border-neutral-300 border-b justify-start">
          <div class="flex items-start">
            <IconField class="ri-pencil-line text-blue-500 text-3xl" />
            <h2 class="text-neutral-600 text-3xl mx-3">Edit Article</h2>
          </div>
          <IconField
            class="ri-close-line text-neutral-600 text-3xl ml-auto hover:text-neutral-800 transition cursor-pointer"
            @click="drawer.close"
          />
        </div>
        <div class="flex flex-col mt-4 gap-4">
          <div class="flex flex-col">
            <label for="source" class="text-neutral-950 text-xl font-medium">Source</label>
            <InputText id="source" v-model.trim="article.source" :invalid="sourceHasError" />
            <Message variant="simple" severity="error" v-show="sourceHasError">
              {{ errors?.source?.[0]?.message }}
            </Message>
          </div>
          <div class="flex flex-col">
            <label for="title" class="text-neutral-950 text-xl font-medium">Title</label>
            <InputText id="title" v-model.trim="article.title" :invalid="titleHasError" />
            <Message variant="simple" severity="error" v-show="titleHasError">
              {{ errors?.title?.[0]?.message }}
            </Message>
          </div>
          <div class="flex flex-col">
            <label for="description" class="text-neutral-950 text-xl font-medium">Description</label>
            <Textarea id="description" v-model.trim="article.description" multiple="true" rows="6" />
          </div>
          <div class="flex flex-col">
            <div class="flex flex-row gap-1 h-8">
              <template v-for="item of article.tags">
                <Chip
                  :label="item.name"
                  removable
                  @remove="removeChip(item)"
                  style="background-color: var(--color-blue-50)"
                />
              </template>
            </div>
            <div class="flex flex-row gap-1 justify-center">
              <AutoComplete
                class="my-1 flex-5"
                fluid
                id="tags"
                placeholder="Ajout tag ..."
                @complete="search"
                optionLabel="name"
                @option-select="selectedItems"
                :suggestions="refSuggestion"
              />
            </div>
          </div>
          <div>
            <Button @click="validateAndSend"> Valider </Button>
          </div>
        </div>
      </div>
    </template>
  </Drawer>
</template>
