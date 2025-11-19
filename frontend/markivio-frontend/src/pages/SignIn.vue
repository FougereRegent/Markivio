<template>
  <div class="flex items-center justify-center h-screen bg-neutral-100">
    <div class="flex flex-col p-6 w-5/12 h-6/12 rounded-2xl bg-neutral-50">
      <LogoComponent />
      <div class="flex flex-col items-center justify-center h-full">
        <span class="font-semibold text-4xl mb-2 text-neutral-900 antialiased">Sign In</span>
        <Button class="w-56" @click="authStore.login" label="Continue" size="large" />
        <div class="flex flex-row justify-between w-56 mt-5">
          <img class="size-12" src="../assets/linkedin-icon.svg" alt="linkedin logo" @click="signIn" />
          <img class="size-12" src="../assets/google-icon.svg" alt="google logo" @click="signIn"/>
          <img class="size-12" src="../assets/microsoft-icon.svg" alt="microsoft logo" @click="signIn"/>
          <img class="size-12" src="../assets/github-icon.svg" alt="github logo" @click="signIn"/>
        </div>
        <span class="text-neutral-700 text-center font-extralight mt-1 antialiased">Sign in with different account</span>
      </div>
      <div>
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
import LogoComponent from '@/components/LogoComponent.vue';
import { ref, watch } from 'vue';
import { useAuthStore } from '@/stores/AuthStore';

    const authStore = useAuthStore();
    const isDarkTheme = ref(false);

    watch(isDarkTheme, (current) => {
      if(current)
        document.documentElement.classList.add('app-dark');
      else
        document.documentElement.classList.remove('app-dark')
    });

    const signIn = () => {
      debugger;
      authStore.login();
    };
</script>