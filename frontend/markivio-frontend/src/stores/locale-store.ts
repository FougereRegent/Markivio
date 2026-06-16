import { defineStore } from 'pinia'
import { ref } from 'vue'
import { i18n, type SupportedLocale } from '@/i18n'

const STORAGE_KEY = 'markivio-locale'

export const useLocaleStore = defineStore('locale', () => {
  const saved = (localStorage.getItem(STORAGE_KEY) ?? 'en') as SupportedLocale
  const locale = ref<SupportedLocale>(saved)

  i18n.global.locale.value = saved

  function setLocale(lang: SupportedLocale) {
    locale.value = lang
    localStorage.setItem(STORAGE_KEY, lang)
    i18n.global.locale.value = lang
  }

  return { locale, setLocale }
})
