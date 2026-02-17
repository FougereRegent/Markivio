<script setup lang="ts">
import HeaderComponent from './components/HeaderComponent.vue';
import { useAuthStore } from '@/stores/auth-store';
import NavBarComponent from './components/NavBarComponent.vue';
import { useLoaderStore } from './stores/loader-store';
import { watch } from 'vue';

const authStore = useAuthStore();
const loadingStore = useLoaderStore();

watch(() => authStore.token, (token) => {
if(!token) {
return;
}
console.log("init");
}, {immediate: true});

authStore.init();


</script>

<template>
  <div class="h-screen flex flex-col">
    <header class="p-3 h-3/32">
      <HeaderComponent />
    </header>
    <div class="px-2 h-29/32">
      <Splitter class="h-full pb-2" layout="horizontal">
        <SplitterPanel :min-size=10 :size="15" class="bg-white">
          <NavBarComponent />
        </SplitterPanel>
        <SplitterPanel :min-size=65 :size="85" class="bg-neutral-100 h-full">
          <ScrollPanel class="h-full" v-show="!loadingStore.isLoading">
            <RouterView />
          </ScrollPanel>
          <div class="p-5 h-full flex flex-col justify-center" v-show="loadingStore.isLoading">
            <ProgressSpinner />
          </div>
        </SplitterPanel>
      </Splitter>
    </div>
  </div>
</template>

<style></style>
