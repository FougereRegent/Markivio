<script setup lang="ts">
import {
  Drawer,
  IconField,
  Textarea,
  type AutoCompleteCompleteEvent,
  type AutoCompleteOptionSelectEvent,
} from 'primevue'
import TagCreatorComponent from './TagCreatorComponent.vue'
import { useArticleDrawer, useArticleForm, useArticleSubmit } from '@/composables/drawer/article.drawer.composable'
import { useTagAutocomplete } from '@/composables/drawer/tag.drawer.composable'
import { computed, nextTick } from 'vue'
import type { Tag } from '@/domain/tag.models'

const { article, addTag, removeTag, } = useArticleForm();
const { drawer } = useArticleDrawer(article);
const { tagName, refSuggestion, search } = useTagAutocomplete(computed(() => article.value.tags))
const { submit, hasError, fetching } = useArticleSubmit(article, drawer)


async function selectedItems(event: AutoCompleteOptionSelectEvent) {
  const tag = event.value as Tag;
  addTag(tag)
  await nextTick();
}

async function searchTag(event: AutoCompleteCompleteEvent) {
  search(event.query)
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
            <InputText id="source" v-model.trim="article.source" :invalid="hasError.source.hasError" />
            <Message variant="simple" severity="error" v-show="hasError.title.hasError">
              {{ hasError.source.message }}
            </Message>
          </div>
          <div class="flex flex-col">
            <label for="title" class="text-neutral-950 text-xl font-medium">Title</label>
            <InputText id="title" v-model.trim="article.title" :invalid="hasError.title.hasError" />
            <Message variant="simple" severity="error" v-show="hasError.title.hasError">
              {{ hasError.source.message }}
            </Message>
          </div>
          <div class="flex flex-col">
            <label for="description" class="text-neutral-950 text-xl font-medium">Description</label>
            <Textarea id="description" v-model.trim="article.description" multiple="true" rows="6" />
          </div>
          <div class="flex flex-col">
            <div class="flex flex-row gap-1 h-8">
              <template v-for="item of article.tags" :key="item.id ?? ''">
                <Chip :label="item.name" removable @remove="removeTag(item)"
                  style="background-color: var(--color-blue-50)" />
              </template>
            </div>
            <div class="flex flex-row gap-1 justify-center">
              <AutoComplete id="tags" ref="autocompleteRef" v-model="tagName" class="my-1 flex-5"
                placeholder="Ajout tag ..." dropdown optionLabel="name" @option-select="selectedItems"
                @complete="searchTag" :suggestions="refSuggestion" />
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
