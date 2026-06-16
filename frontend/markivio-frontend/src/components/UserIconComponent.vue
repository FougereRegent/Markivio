<template>
  <div class="size-12 cursor-pointer" :class="defaultClass" @click="$emit('clickIcon')">
    <img :src="defaultUser" :alt="t('common.userIcon')" class="rounded-full" />
  </div>
</template>

<script setup lang="ts">
import { useAuthStore } from '@/features/auth/stores/auth-store'
import { computed } from 'vue'
import { useI18n } from 'vue-i18n'

const { t } = useI18n()
const store = useAuthStore()

const defaultUser = computed(() => {
  if ((store.getUser?.accountPicture ?? '') === '') {
    return '/src/assets/default-user.svg'
  }
  return store.getUser?.accountPicture
})

const defaultClass = computed(() => {
  if ((store.getUser?.accountPicture ?? '') === '') {
    return 'p-1 bg-blue-500'
  }
  return ''
})
</script>
