<script setup lang="ts">
import LogoComponent from './LogoComponent.vue';
import { ref } from 'vue';
import 'remixicon/fonts/remixicon.css';
import UserIconComponent from './UserIconComponent.vue';
import { useAuthStore } from '@/stores/AuthStore';
import UserService, { } from '../services/UserService.ts';

const store = useAuthStore();
const data = ref(["Test1", "Test2", "Test3"]);
const search = () => {
  return [
    "Test1",
    "Test2",
    "Test3",
    "Test4",
  ]
};
const clickIcon = async () => {
  UserService.getMe().subscribe(val => {
    console.log(val);
  })
};



</script>

<template>
  <div class="flex flex-row w-full items-center justify-between">
    <LogoComponent class="mb-2" />
    <IconField class="2xl:w-9/12">
      <InputIcon class="w-full">
        <i class="ri-search-line text-neutral-700"></i>
      </InputIcon>
      <AutoComplete size="small" placeholder="Search ..." @complete="search" :suggestions="data" class="w-full"
        input-class="w-full" />
    </IconField>
    <Button label="Add Link" />
    <div class="h-7 border border-neutral-300"></div>
    <UserIconComponent @click-icon="clickIcon" class="mr-4" />
  </div>
</template>
