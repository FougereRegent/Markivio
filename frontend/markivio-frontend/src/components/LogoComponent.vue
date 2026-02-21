<template>
  <div class="flex flex-row items-center" v-on:click="onClick">
    <img class="size-10" src="../assets/logo.svg" alt="markivio logo" />
    <h2 class="font-semibold text-4xl text-neutral-950 text-center mx-1 antialiased">Markivio</h2>
  </div>
</template>
<script setup lang="ts">
import router from '@/router';

const onClick = () => {
  router.push({ name: "home"});
};
</script>
