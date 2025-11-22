<script setup lang="ts">
import LogoComponent from './LogoComponent.vue';
import { ref } from 'vue';
import 'remixicon/fonts/remixicon.css';
import UserIconComponent from './UserIconComponent.vue';
import UserMenuComponent from './UserMenuComponent.vue';

const op = ref();
const data = ref(["Test1", "Test2", "Test3"]);
const search = () => {
  return [
    "Test1",
    "Test2",
    "Test3",
    "Test4",
  ]
};
const clickIcon = () => {
  op.value.toggle(event);
};

</script>

<template>
  <div class="flex flex-row w-full items-center justify-between">
    <LogoComponent class="pb-2" />
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
    <Popover ref="op">
      <UserMenuComponent />
    </Popover>
  </div>
</template>
