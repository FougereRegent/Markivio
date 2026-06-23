<script setup lang="ts">
import { useVersionStore } from '@/stores/version-store';
import { useI18n } from 'vue-i18n'

const { t } = useI18n()
const { version } = useVersionStore()
</script>

<template>
  <div class="flex flex-col justify-between h-11/24 px-3">
    <div class="flex flex-col py-2 mt-4">
      <NavElementComponent :name="t('nav.allArticles')" icon-class="ri-list-view" />
      <NavElementComponent :name="t('nav.favorites')" icon-class="ri-star-line" />
      <NavElementComponent :name="t('nav.tags')" icon-class="ri-bookmark-line" />
    </div>
    <div>
      <h2 class="text-neutral-900 font-semibold text-2xl">{{ t('nav.tags') }}</h2>
      <NavElementComponent :name="t('nav.allArticles')" tag-color="red" />
      <NavElementComponent :name="t('nav.favorites')" tag-color="red" />
      <NavElementComponent :name="t('nav.tags')" tag-color="red" />
    </div>
    <div>
      <h2 class="text-neutral-900 font-semibold text-2xl">{{ t('nav.folder') }}</h2>
      <NavElementComponent :name="t('nav.allArticles')" icon-class="ri-folder-line" />
      <NavElementComponent :name="t('nav.favorites')" icon-class="ri-folder-line" />
      <NavElementComponent :name="t('nav.tags')" icon-class="ri-folder-line" />
    </div>
  </div>
  <div class="px-3">
    <p>{{version}}</p>
  </div>
</template>
