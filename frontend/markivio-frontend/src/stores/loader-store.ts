import { CONST } from "@/config/constante.config";
import { defineStore } from "pinia";
import { ref } from "vue";

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
      }, CONST.flickerTime);
    }
  }

  function stop() {
    if(nbrequest.value > 0)
      nbrequest.value--;

    if(nbrequest.value > 0)
      return;
    else
      clearTimeout(delayTimeout);

    const elapsedTime = CONST.flickerTime - (Date.now() - startTime);
    if(elapsedTime <= 0)
    {
      isLoading.value = false;
    }else {
      let idTimeout: number;
      idTimeout = setTimeout(() => {isLoading.value = false; clearTimeout(idTimeout)}, elapsedTime);
    }

  }

  function reset() {
    nbrequest.value = 0;
  }

  return {isLoading, start, stop, reset};
});
