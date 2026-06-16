<script setup lang="ts">
import { useLocaleStore } from '@/stores/locale-store'
import type { SupportedLocale } from '@/i18n'

const localeStore = useLocaleStore()

const locales: { label: string; value: SupportedLocale }[] = [
  { label: 'EN', value: 'en' },
  { label: 'FR', value: 'fr' },
  { label: 'ES', value: 'es' },
]

function onChange(value: SupportedLocale) {
  localeStore.setLocale(value)
}
</script>

<template>
  <SelectButton
    :modelValue="localeStore.locale"
    @update:modelValue="onChange"
    :options="locales"
    optionLabel="label"
    optionValue="value"
    size="small"
  />
</template>
