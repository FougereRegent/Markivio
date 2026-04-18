<script setup lang="ts">
import router from '@/router'
import logoSvg from '@/assets/logo.svg'

const onClick = () => {
  router.push({ name: 'home' })
}
</script>

<template>
  <div class="flex flex-row items-center" @click="onClick">
    <img class="size-10" :src="logoSvg" alt="markivio logo" />
    <h2 class="font-semibold text-4xl text-neutral-950 text-center mx-1 antialiased">Markivio</h2>
  </div>
</template>
