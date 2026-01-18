<script setup lang="ts">
import HeaderComponent from './components/HeaderComponent.vue';
import { useAuthStore } from '@/stores/AuthStore';
import NavBarComponent from './components/NavBarComponent.vue';
const authStore = useAuthStore();
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
          <ScrollPanel class="h-full">
            <RouterView />
          </ScrollPanel>
        </SplitterPanel>
      </Splitter>
    </div>
  </div>
</template>

<style></style>
