<script setup lang="ts">
import HeaderComponent from './components/HeaderComponent.vue';
import { useAuthStore } from '@/stores/AuthStore';
const authStore = useAuthStore();
authStore.init();
</script>

<template>
  <header class="p-3">
    <HeaderComponent />
  </header>
  <main class="p-2">
    <RouterView />
  </main>
</template>

<style></style>
