<template>
  <div class="flex items-center justify-center h-screen bg-neutral-100">
    <div class="flex flex-col p-6 w-5/12 h-6/12 rounded-2xl bg-neutral-50">
      <span class="flex flex-row w-50 items-center">
        <img class="size-9" src="../assets/logo.svg" alt="markivio logo" />
        <h2 class="font-semibold text-4xl text-neutral-950 text-center mx-1 antialiased">Markivio</h2>
      </span>
      <div class="flex flex-col items-center justify-center h-full">
        <span class="font-semibold text-4xl mb-2 text-neutral-900 antialiased">Sign In</span>
        <Button class="w-56" @click="signin" label="Continue" size="large" />
        <div class="flex flex-row justify-between w-56 mt-5">
          <img class="size-12" src="../assets/linkedin-icon.svg" alt="linkedin logo" @click="signin" />
          <img class="size-12" src="../assets/google-icon.svg" alt="google logo" @click="signin"/>
          <img class="size-12" src="../assets/microsoft-icon.svg" alt="microsoft logo" @click="signin"/>
          <img class="size-12" src="../assets/github-icon.svg" alt="github logo" @click="signin"/>
        </div>
        <span class="text-neutral-700 text-center font-extralight mt-1 antialiased">Sign in with different account</span>
        <Button @click="showToken" label="show token" hidden/>
      </div>
      <div>
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
import { useAuth0 } from '@auth0/auth0-vue';
import { ref, watch } from 'vue';

    const { loginWithRedirect, getAccessTokenSilently } = useAuth0();
    const isDarkTheme = ref(false);

    watch(isDarkTheme, (current) => {
      if(current)
        document.documentElement.classList.add('app-dark');
      else
        document.documentElement.classList.remove('app-dark')
    });
    const signin = () => {
        loginWithRedirect();
    };
    const showToken = async () => {
        const result = await getAccessTokenSilently();
        console.log(result);
    };
</script>