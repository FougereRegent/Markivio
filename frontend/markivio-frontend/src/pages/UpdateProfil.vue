<template>
  <div>
    <p>{{ user?.Email }}</p>
    <p>{{ user?.FirstName }}</p>
    <p>{{ user?.LastName }}</p>
  </div>
</template>

<script setup lang="ts">
import userService from '@/services/user.service';
import { ref } from 'vue';

const user = userService.getMe();
</script>
