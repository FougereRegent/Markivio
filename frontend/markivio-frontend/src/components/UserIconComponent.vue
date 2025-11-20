<template>
  <div class="size-12 cursor-pointer" :class="defaultClass" @click="$emit('clickIcon')">
    <img :src="defaultUser" alt="User icon" class="rounded-full" />
  </div>
</template>

<script setup lang="ts">
import { useAuthStore } from '@/stores/AuthStore';
import { computed } from 'vue';
const store = useAuthStore();

const defaultUser = computed(() => {
  if ((store.getUser.accountPicture ?? "") === "") {
    return "/src/assets/default-user.svg";
  }
  return store.getUser?.accountPicture;
})

const defaultClass = computed(() => {
  if ((store.getUser.accountPicture ?? "") === "") {
    return "p-1 bg-blue-500";
  }
  return "";
})
</script>
