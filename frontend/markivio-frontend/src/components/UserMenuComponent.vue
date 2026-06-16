<script setup lang="ts">
import router from '@/router'
import { useAuthStore } from '@/features/auth/stores/auth-store'
import { useI18n } from 'vue-i18n'
import { computed } from 'vue'

const authStore = useAuthStore()
const { t } = useI18n()

const items = computed(() => [
  {
    label: t('userMenu.editProfile'),
    icon: 'ri-edit-line',
    command: () => router.push({ name: 'updateUser' }),
  },
  { label: t('userMenu.logout'), icon: 'ri-logout-box-line', command: async () => await authStore.logout() },
])
</script>

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
