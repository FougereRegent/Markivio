<template>
  <Menu :model="items">
    <template #item="{ item, props }">
      <a v-ripple v-bind="props.action">
        <i :class="item.icon" class="text-neutral-700" />
        <p class="text-neutral-700">{{ item.label }}</p>
      </a>
    </template>
  </Menu>
</template>
<script setup lang="ts">
import router from '@/router';
import { useAuthStore } from '@/stores/AuthStore';
import { ref } from 'vue';
const authStore = useAuthStore();

const items = ref([
  { label: "Edit Profile", icon: "ri-edit-line", command: () => router.push({ name: "updateUser" }) },
  { label: "Logout", icon: "ri-logout-box-line", command: async () => await authStore.logout() },
])
</script>
