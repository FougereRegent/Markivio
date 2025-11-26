<template>
  <div>
    <p>{{ data?.me?.id }}</p>
    <p>{{ data?.me?.firstName }}</p>
    <p>{{ data?.me?.lastName }}</p>
    <p>{{ data?.me?.email }}</p>
  </div>
</template>

<script setup lang="ts">
import userService from '@/services/user.service';

const { data } = userService.getMe();
</script>
