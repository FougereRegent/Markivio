import { defineStore } from "pinia";
import { computed } from "vue";

export const useVersionStore = defineStore('version', () => {
  const version = computed(() => {
    return import.meta.env?.VITE_APP_VERSION ?? "1.0.0-undef"
  })

  return {version}
});
