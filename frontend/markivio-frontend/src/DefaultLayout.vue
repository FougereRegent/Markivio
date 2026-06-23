<script setup lang="ts">
import HeaderComponent from '@/components/HeaderComponent.vue'
import { useAuthStore } from '@/features/auth/stores/auth-store'
import NavBarComponent from '@/components/NavBarComponent.vue'
import { useLoaderStore } from '@/stores/loader-store'
import { watch } from 'vue'

const authStore = useAuthStore()
const loadingStore = useLoaderStore()

watch(
  () => authStore.token,
  (token) => {
    if (!token) return
    loadingStore.stop()
  },
  { immediate: true },
)
</script>

<template>
  <div class="h-screen flex flex-col">
    <header class="p-3 h-2/32">
      <HeaderComponent />
    </header>
    <div class="h-30/32">
      <Toast position="top-right" group="tr" />
      <Toast position="bottom-right" group="br" />
      <div class="flex flex-row h-full w-full pb-2 border-gray-300 border-t-2">
        <div class="p-1">
          <NavBarComponent />
        </div>
        <div class="flex flex-2">
          <ScrollPanel class="h-full w-full bg-gray-100" v-show="!loadingStore.isLoading">
            <RouterView />
          </ScrollPanel>
          <div class="p-5 h-full flex flex-col justify-center" v-show="loadingStore.isLoading">
            <ProgressSpinner />
          </div>
        </div>
      </div>
    </div>
  </div>
</template>
