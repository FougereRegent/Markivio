import { defineStore } from "pinia";
import { computed } from "vue";

export const useVersionStore = defineStore('version', () => {
  const appVersion = computed(() => {
    return "1.1.10";
  })

  return {appVersion};
});
