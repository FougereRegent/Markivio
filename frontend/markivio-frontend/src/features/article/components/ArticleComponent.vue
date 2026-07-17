<script lang="ts" setup>
import { Tag } from 'primevue'
import { contrastColor } from '@/helpers/ui.helpers'
import DialogSource from '@/components/DialogSource.vue'
import { ref } from 'vue'
import { useGetSourceUrl, useToggleFavorite, type UrlSource } from '@/features/article/composables/article.graphql'
import { useAddEditDrawer } from '@/stores/add-edit-drawer-store'
import DialogContent from '@/components/DialogContent.vue'

export type ArticleProps = {
  id: string
  title: string
  description?: string
  isFavorite?: boolean
  tags?: Array<{
    label: string
    color: string
  }>
}

const props = withDefaults(defineProps<ArticleProps>(), {
  tags: () => [],
})

const urlSource = ref<UrlSource | null>(null)

const { runQuery } = useGetSourceUrl(props.id)
const drawer = useAddEditDrawer();
const { toggleFavorite } = useToggleFavorite();

const favorite = ref(props.isFavorite ?? false)

const dialogUrlVisible = ref(false)
const dialogContentVisible = ref(false);

async function showSourceArticle() {
  const result = await runQuery()

  if (!result) return;

  urlSource.value = result

  if (result.framable) dialogUrlVisible.value = true
  else window.open(result.source, '_blanck')?.focus()
}

async function showMarkdownArticle() {
  const result = await runQuery();
  if (!result) return;

  urlSource.value = result;

  dialogContentVisible.value = true
}

function editArticle() {
  drawer.open(true, props.id);
}

function deleteArticle() {
  console.warn('TODO: implement delete for article', props.id);
}

async function handleToggleFavorite() {
  favorite.value = !favorite.value;
  await toggleFavorite(props.id);
}
</script>

<template>
  <div
    class="group flex flex-row rounded-md px-4 py-5 border-gray-200 border bg-white hover:bg-gray-50 hover:border-gray-300"
  >
    <div class="flex flex-col flex-4 w-10 h-full">
      <div class="flex flex-row items-start">
        <p class="flex-1 text-lg md:text-xl lg:text-2xl 2xl:text-3xl mb-1 text-gray-900 font-semibold break-words">{{ props.title }}</p>
        <button
          class="shrink-0 ml-2 mt-1 text-xl transition-colors duration-150"
          :class="favorite ? 'text-amber-400 hover:text-amber-500' : 'text-gray-300 hover:text-amber-400'"
          @click="handleToggleFavorite"
        >
          <i :class="favorite ? 'ri-star-fill' : 'ri-star-line'" />
        </button>
        <div class="flex shrink-0 items-center gap-1 opacity-0 group-hover:opacity-100 transition-opacity duration-200">
          <Button
            class="w-9 h-9"
            icon="ri-edit-box-line"
            severity="secondary"
            @click="editArticle"
          />
          <Button
            class="w-9 h-9"
            icon="ri-article-line"
            severity="secondary"
            @click="showSourceArticle"
          />
          <Button
            class="w-9 h-9"
            icon="ri-code-line"
            severity="secondary"
            @click="showMarkdownArticle"
          />
          <Button
            class="w-9 h-9"
            icon="ri-delete-bin-line"
            severity="danger"
            @click="deleteArticle"
          />
        </div>
      </div>

      <div class="w-full text-base md:text-lg lg:text-xl my-2 text-gray-600">
        <p class="line-clamp-3 text-justify">{{ props.description }}</p>
      </div>
      <div class="flex flex-row flex-wrap gap-2">
        <template v-for="item of props.tags" :key="item.label">
          <Tag
            :value="item.label"
            :style="{
              backgroundColor: item.color,
              color: contrastColor(item.color),
              opacity: 0.55,
            }"
          />
        </template>
      </div>
    </div>
    <DialogSource
      v-model:visible="dialogUrlVisible"
      :title="props.title"
      :id="props.id"
      :source="urlSource?.source ?? ''"
    />
    <DialogContent
        v-model:visible="dialogContentVisible"
        :title="props.title"
        :id="props.id"
        :content="urlSource?.content ?? ''"
    />
  </div>
</template>
