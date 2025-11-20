<script setup lang="ts">
import HeaderComponent from './components/HeaderComponent.vue';
import { useAuthStore } from '@/stores/AuthStore';
import NavBarComponent from './components/NavBarComponent.vue';
const authStore = useAuthStore();
authStore.init();
</script>

<template>
  <div class="h-screen flex flex-col">
    <header class="p-3">
      <HeaderComponent />
    </header>
    <div class="p-2 h-full">
      <Splitter class="h-full" layout="horizontal">
        <SplitterPanel :min-size=10 :size="15" class="bg-neutral-50">
          <NavBarComponent />
        </SplitterPanel>
        <SplitterPanel :min-size=65 :size="85">
          <RouterView />
        </SplitterPanel>
      </Splitter>
    </div>
  </div>
</template>

<style></style>
