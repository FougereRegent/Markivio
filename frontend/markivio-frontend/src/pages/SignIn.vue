<script setup lang="ts">
import LogoComponent from '@/components/LogoComponent.vue'
import LanguageSelector from '@/components/LanguageSelector.vue'
import { useAuthStore } from '@/features/auth/stores/auth-store'
import { useI18n } from 'vue-i18n'

const authStore = useAuthStore()
const { t } = useI18n()
</script>

<template>
  <div class="flex items-center justify-center h-screen bg-neutral-100">
    <div class="flex flex-col p-6 w-5/12 h-6/12 rounded-2xl bg-neutral-50">
      <div class="flex justify-end">
        <LanguageSelector />
      </div>
      <LogoComponent />
      <div class="flex flex-col items-center justify-center h-full">
        <span class="font-semibold text-4xl mb-2 text-neutral-900 antialiased">{{ t('signIn.title') }}</span>
        <Button class="w-56" @click="authStore.login" :label="t('signIn.continue')" size="large" />
        <div class="flex flex-row justify-between w-56 mt-5">
          <img
            class="size-12"
            src="@/assets/linkedin-icon.svg"
            alt="linkedin logo"
            @click="authStore.login"
          />
          <img
            class="size-12"
            src="@/assets/google-icon.svg"
            alt="google logo"
            @click="authStore.login"
          />
          <img
            class="size-12"
            src="@/assets/microsoft-icon.svg"
            alt="microsoft logo"
            @click="authStore.login"
          />
          <img
            class="size-12"
            src="@/assets/github-icon.svg"
            alt="github logo"
            @click="authStore.login"
          />
        </div>
        <span class="text-neutral-700 text-center font-extralight mt-1 antialiased"
          >{{ t('signIn.subtitle') }}</span
        >
      </div>
    </div>
  </div>
</template>
