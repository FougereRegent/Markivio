import { defineStore } from "pinia";
import { computed, ref } from "vue";
import { Subject } from "rxjs";

export const useLoaderStore = defineStore('loader', () => {
  const nbrequest = ref(0);
  let delayTimeout = 0;
  const isLoading = ref(false);

  function start() {
    nbrequest.value++;
    if(nbrequest.value === 1){
      delayTimeout = setTimeout(() => {
        isLoading.value = true;
      }, 300);
    }
    setTimeout(() => nbrequest.value++, 300);
  }

  function stop() {
    if(nbrequest.value > 0)
      nbrequest.value--;

    if(nbrequest.value === 0) {
      clearTimeout(delayTimeout);
    }
  }

  function reset() {
    nbrequest.value = 0;
  }

  return {isLoading, start, stop, reset};
});
