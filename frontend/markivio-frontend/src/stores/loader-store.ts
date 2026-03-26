import { CONST } from '@/config/constante.config';
import { defineStore } from 'pinia';
import { ref } from 'vue';

export const useLoaderStore = defineStore('loader', () => {
  const nbrequest = ref(0);
  const isLoading = ref(false);
  let delayTimeout = 0;
  let startTime: number;

  function start() {
    nbrequest.value++;
    if (nbrequest.value === 1) {
      delayTimeout = setTimeout(() => {
        isLoading.value = true;
        startTime = Date.now();
      }, CONST.flickerTime) as any;
    }
  }

  function stop() {
    if (nbrequest.value > 0) nbrequest.value--;

    if (nbrequest.value > 0) return;

    clearTimeout(delayTimeout);

    const elapsedTime = CONST.flickerTime - (Date.now() - startTime);
    if (elapsedTime <= 0) {
      isLoading.value = false;
    } else {
      setTimeout(() => {
        isLoading.value = false;
      }, elapsedTime);
    }
  }

  function reset() {
    nbrequest.value = 0;
  }

  return { isLoading, start, stop, reset };
});
