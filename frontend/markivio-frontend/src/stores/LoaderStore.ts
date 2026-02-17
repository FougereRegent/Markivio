import { defineStore } from "pinia";
import { computed, ref } from "vue";
import { Subject } from "rxjs";

export const useLoaderStore = defineStore('loader', () => {
  const nbrequest = ref(0);
  let delayTimeout = 0;
  let startTime: number;
  const isLoading = ref(false);

  function start() {
    nbrequest.value++;
    if(nbrequest.value === 1){
      delayTimeout = setTimeout(() => {
        isLoading.value = true;
        startTime = Date.now();
      }, 300);
    }
  }

  function stop() {
    if(nbrequest.value > 0)
      nbrequest.value--;

    if(nbrequest.value > 0)
      return;
    else
      clearTimeout(delayTimeout);

    const elapsedTime = 300 - (Date.now() - startTime);
    if(elapsedTime <= 0)
    {
      isLoading.value = false;
    }else {
      setTimeout(() => isLoading.value = false, elapsedTime);
    }

  }

  function reset() {
    nbrequest.value = 0;
  }

  return {isLoading, start, stop, reset};
});
