<template>
  <main>
    <RouterView />
  </main>
</template>

<script setup lang="ts">
import { watch } from 'vue'
import { useLocaleStore } from '@/stores/locale-store'

const localeStore = useLocaleStore()

watch(
  () => localeStore.locale,
  (lang) => {
    document.documentElement.lang = lang
    document.title = 'Markivio'
  },
  { immediate: true },
)
</script>
