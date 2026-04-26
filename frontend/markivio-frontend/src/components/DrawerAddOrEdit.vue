<script setup lang="ts">
import {
  Drawer,
  IconField,
  Textarea,
  type AutoCompleteCompleteEvent,
  type AutoCompleteOptionSelectEvent,
} from 'primevue'
import { ActionDrawer, useAddEditDrawer } from '@/stores/add-edit-drawer-store'
import { computed, nextTick, ref, watch } from 'vue'
import { type Article, ArticleSchema } from '@/domain/article.models'
import { type Tag } from '@/domain/tag.models'
import { useZodValidation } from '@/composables/zod.composable'
import TagCreatorComponent from './TagCreatorComponent.vue'
import { useCreateArticle, useGetArticleById } from '@/composables/article.graphql'
import { useGetAllTags } from '@/composables/tag.graphql'
import { useDebounce } from '@vueuse/core'
import { CONST } from '@/config/constante.config'

const article = ref<Article>({
  id: null,
  title: '',
  source: '',
  description: '',
  tags: [],
});
const tagName = ref<string | null>('');
const offset = ref(0)
const refSuggestion = ref<Tag[]>([]);

const drawer = useAddEditDrawer();
const debounceTagName = useDebounce(tagName, CONST.debounceTime.inputTime);
const { validate, errors } = useZodValidation(ArticleSchema, article);
const { createArticle, fetching } = useCreateArticle(article);
const { tags, executeQuery } = useGetAllTags(debounceTagName)
const { article: art, executeQuery: fetchArticle } =
  useGetArticleById(computed(() => drawer.drawerArticleId), { pause: computed(() => {
  debugger;
  const result = drawer.drawerArticleId
  == null || drawer.drawerType === ActionDrawer.Edit;
  return result;
  })});

const titleHasError = computed(() => errors.value?.title != undefined);
const sourceHasError = computed(() => errors.value?.source != undefined);

watch(tags, (newData) => {
  refSuggestion.value = newData ?? []
})

watch(
  () => drawer.drawerState,
  async (open) => {
    if (!open) return

    article.value = {
      id: null,
      tags: [],
      source: '',
      title: '',
      description: '',
    }

    errors.value = null;
    tagName.value = ''
    offset.value = 0
    refSuggestion.value = []

    if(drawer.drawerType === ActionDrawer.Edit)
      await fetchArticle({requestPolicy: 'network-only'})
  },
  { immediate: true },
)

watch(() => art.value, (newArticle) => {
  article.value = newArticle;
});

async function selectedItems(event: AutoCompleteOptionSelectEvent) {
  const selected = event.value as Tag

  if (!article.value.tags.find((t) => t.id === selected.id)) {
    article.value.tags.push(selected)
  }

  tagName.value = null
  await nextTick()
}

function removeChip(tag: Tag) {
  article.value.tags = article.value.tags.filter((item) => item.id !== tag.id)
}

const search = async (event: AutoCompleteCompleteEvent) => {
  offset.value = 0
  tagName.value = event.query ?? ''

  if (!event.query) {
    await executeQuery({ requestPolicy: 'network-only' })
  }
}

async function submit() {
  if (validate()) {
    await createArticle()
    drawer.close()
  }
}

</script>

<template>
  <Drawer v-model:visible="drawer.drawerState" position="right" :closeOnEscape="false" class="w-140!">
    <template #container>
      <div class="flex flex-col pt-2 px-5">
        <div class="flex flex-row py-6 border-neutral-300 border-b justify-start">
          <div class="flex items-start">
            <IconField class="ri-pencil-line text-blue-500 text-3xl" />
            <h2 class="text-neutral-600 text-3xl mx-3">{{ drawer.drawerTitle }}</h2>
          </div>
          <IconField
            class="ri-close-line text-neutral-600 text-3xl ml-auto hover:text-neutral-800 transition cursor-pointer"
            @click="drawer.close" />
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
              <template v-for="item of article.tags" :key="item.id ?? ''">
                <Chip :label="item.name" removable @remove="removeChip(item)"
                  style="background-color: var(--color-blue-50)" />
              </template>
            </div>
            <div class="flex flex-row gap-1 justify-center">
              <AutoComplete id="tags" ref="autocompleteRef" v-model="tagName" class="my-1 flex-5"
                placeholder="Ajout tag ..." dropdown optionLabel="name" @option-select="selectedItems"
                @complete="search" :suggestions="refSuggestion" />
              <TagCreatorComponent />
            </div>
          </div>
          <div>
            <Button @click="submit" :disabled="fetching" :loading="fetching"> Submit </Button>
          </div>
        </div>
      </div>
    </template>
  </Drawer>
</template>
