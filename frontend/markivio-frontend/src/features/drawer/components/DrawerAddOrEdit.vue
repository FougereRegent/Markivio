<script setup lang="ts">
import {
  Drawer,
  Textarea,
  type AutoCompleteCompleteEvent,
  type AutoCompleteOptionSelectEvent,
} from 'primevue'
import TagCreatorComponent from '@/features/tag/components/TagCreatorComponent.vue'
import { useArticleDrawer, useArticleForm, useArticleSubmit } from '@/features/drawer/composables/article.drawer.composable'
import { useTagAutocomplete } from '@/features/drawer/composables/tag.drawer.composable'
import { computed, nextTick } from 'vue'
import type { Tag } from '@/features/tag/models/tag.models'
import { useI18n } from 'vue-i18n'
import { ActionDrawer } from '@/stores/add-edit-drawer-store'

const { t } = useI18n()
const { article, addTag, removeTag, } = useArticleForm();
const { drawer } = useArticleDrawer(article);
const { tagName, refSuggestion, search } = useTagAutocomplete(computed(() => article.value.tags));
const { submit, hasError, fetching } = useArticleSubmit(article);

const headerTitle = computed(() =>
  drawer.drawerType === ActionDrawer.Edit ? t('drawer.edit') : t('drawer.create'),
)

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
  <Drawer
    v-model:visible="drawer.drawerState"
    position="right"
    :closeOnEscape="false"
    :pt="{
      root: { style: { width: 'clamp(300px, 100vw, 35rem)' } }
    }"
  >
    <template #container>
      <div class="flex flex-col h-full overflow-y-auto">
        <div class="flex flex-row items-center px-3 md:px-5 py-4 border-neutral-300 border-b gap-2">
          <i class="ri-pencil-line text-blue-500 text-xl md:text-2xl"></i>
          <h2 class="text-neutral-600 text-lg md:text-xl font-semibold">{{ headerTitle }}</h2>
          <i
            class="ri-close-line text-neutral-500 text-xl md:text-2xl ml-auto hover:text-neutral-800 transition cursor-pointer"
            @click="drawer.close"
          ></i>
        </div>
        <div class="flex flex-col p-3 md:p-5 gap-3 md:gap-4">
          <div class="flex flex-col gap-1">
            <label for="source" class="text-neutral-950 text-sm md:text-base font-medium">{{ t('drawer.source') }}</label>
            <InputText id="source" v-model.trim="article.source" :invalid="hasError.source.hasError" />
            <Message variant="simple" severity="error" v-show="hasError.title.hasError">
              {{ hasError.source.message }}
            </Message>
          </div>
          <div class="flex flex-col gap-1">
            <label for="title" class="text-neutral-950 text-sm md:text-base font-medium">{{ t('drawer.title') }}</label>
            <InputText id="title" v-model.trim="article.title" :invalid="hasError.title.hasError" />
            <Message variant="simple" severity="error" v-show="hasError.title.hasError">
              {{ hasError.source.message }}
            </Message>
          </div>
          <div class="flex flex-col gap-1">
            <label for="description" class="text-neutral-950 text-sm md:text-base font-medium">{{ t('drawer.description') }}</label>
            <Textarea id="description" v-model.trim="article.description" multiple="true" rows="6" />
          </div>
          <div class="flex flex-col gap-1">
            <div class="flex flex-row flex-wrap gap-1">
              <template v-for="item of article.tags" :key="item.id ?? ''">
                <Chip :label="item.name" removable @remove="removeTag(item)"
                  style="background-color: var(--color-blue-50)" />
              </template>
            </div>
            <div class="flex flex-row gap-1 justify-center">
              <AutoComplete id="tags" ref="autocompleteRef" v-model="tagName" class="my-1 flex-1"
                :placeholder="t('drawer.addTag')" dropdown optionLabel="name" @option-select="selectedItems"
                @complete="searchTag" :suggestions="refSuggestion" />
              <TagCreatorComponent />
            </div>
          </div>
          <Button @click="submit" :disabled="fetching" :loading="fetching" class="w-full md:w-auto">
            {{ t('drawer.submit') }}
          </Button>
        </div>
      </div>
    </template>
  </Drawer>
</template>
