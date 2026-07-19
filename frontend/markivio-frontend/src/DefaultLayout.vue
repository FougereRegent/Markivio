<script setup lang="ts">
import HeaderComponent from '@/components/HeaderComponent.vue'
import { useAuthStore } from '@/features/auth/stores/auth-store'
import NavBarComponent from '@/components/NavBarComponent.vue'
import { useLoaderStore } from '@/stores/loader-store'
import { ref, watch } from 'vue'

const authStore = useAuthStore()
const loadingStore = useLoaderStore()

const sidebarOpen = ref(false)

function toggleSidebar() {
  sidebarOpen.value = !sidebarOpen.value
}

function closeSidebar() {
  sidebarOpen.value = false
}

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
    <header class="p-3 shrink-0">
      <HeaderComponent @toggle-sidebar="toggleSidebar" />
    </header>
    <div class="flex-1 min-h-0 flex flex-col">
      <Toast position="top-right" group="tr" />
      <Toast position="bottom-right" group="br" />
      <div class="flex flex-row flex-1 min-h-0 w-full pb-2 border-gray-300 border-t-2">
        <aside :class="sidebarOpen
          ? 'fixed inset-0 z-40 transition-transform duration-300 ease-in-out sm:static sm:inset-auto sm:z-auto sm:block sm:transition-none'
          : 'fixed inset-0 z-40 -translate-x-full pointer-events-none transition-transform duration-300 ease-in-out sm:static sm:inset-auto sm:z-auto sm:block sm:translate-x-0 sm:pointer-events-auto sm:transition-none'">
          <div class="fixed inset-0 bg-black/50 sm:hidden transition-opacity duration-300"
               :class="sidebarOpen ? 'opacity-100' : 'opacity-0'"
               @click="closeSidebar" />
          <div class="relative z-10 h-full w-72 bg-white sm:bg-transparent shadow-xl sm:shadow-none flex flex-col">
            <div class="sm:hidden flex justify-end px-3 py-2 border-b border-neutral-100">
              <Button icon="ri-close-line" severity="secondary" text rounded @click="closeSidebar" />
            </div>
            <div class="flex-1 min-h-0">
              <NavBarComponent @close-sidebar="closeSidebar" />
            </div>
          </div>
        </aside>
        <main class="flex-1 min-w-0">
          <ScrollPanel class="h-full w-full bg-gray-100" v-show="!loadingStore.isLoading">
            <RouterView />
          </ScrollPanel>
          <div class="p-5 h-full flex flex-col justify-center" v-show="loadingStore.isLoading">
            <ProgressSpinner />
          </div>
        </main>
      </div>
    </div>
  </div>
</template>
