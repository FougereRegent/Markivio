<script lang="ts" setup>
import { Tag } from 'primevue'
import { contrastColor } from '@/helpers/ui.helpers'
import DialogSource from '@/components/DialogSource.vue'
import { ref } from 'vue'
import { useGetSourceUrl, type UrlSource } from '@/features/article/composables/article.graphql'
import { useAddEditDrawer } from '@/stores/add-edit-drawer-store'

export type ArticleProps = {
  id: string
  title: string
  description?: string
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

const visible = ref(false)

async function showSourceArticle() {
  const result = await runQuery()

  if (!result) return

  urlSource.value = result

  if (result.framable) visible.value = true
  else window.open(result.source, '_blanck')?.focus()
}

function showMarkdownArticle() {}

function editArticle() {
  drawer.open(true, props.id);
}
</script>

<template>
  <div
    class="flex flex-row rounded-md px-4 py-5 border-gray-200 border bg-white hover:bg-gray-50 hover:border-gray-300"
  >
    <div class="flex flex-col flex-4 w-10 h-full">
      <div class="flex flex-row">
        <p class="flex-1 text-3xl mb-1 text-gray-900 font-semibold">{{ props.title }}</p>
        <Button class="mx-1" icon="ri-edit-box-line" severity="secondary" @click="editArticle" />
        <Button
          class="mx-1"
          icon="ri-article-line"
          severity="secondary"
          @click="showSourceArticle"
        />
        <Button
          class="mx-1"
          icon="ri-code-line"
          severity="secondary"
          @click="showMarkdownArticle"
        />
      </div>

      <div class="h-8/12 w-11/12 text-s my-2 text-gray-600">
        <p class="line-clamp-3 text-justify">{{ props.description }}</p>
      </div>
      <div class="flex flex-row flex-2 gap-3">
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
      v-model:visible="visible"
      :title="props.title"
      :id="props.id"
      :source="urlSource?.source ?? ''"
    />
  </div>
</template>
