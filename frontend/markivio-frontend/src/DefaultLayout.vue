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
          <SplitterPanel :size="15" :min-size="10">
            <NavBarComponent />
          </SplitterPanel>
          <SplitterPanel :min-size="55">
            <RouterView />
          </SplitterPanel>
        </Splitter>
      </div>
  </div>
</template>

<style></style>
