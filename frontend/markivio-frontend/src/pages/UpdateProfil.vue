<template>
  <div class="p-5 h-full flex flex-col justify-center">
    <ProgressSpinner />
  </div>
  <div class="p-5 h-full">
    <div class="flex flex-row justify-between">
      <h1 class="text-4xl text-gray-900">Account Settings</h1>
      <Button size="large" label="Save" class="w-2/12" />
    </div>
    <form class="flex flex-col mt-2 h-5/24 justify-around">
      <FloatLabel>
        <label for="firstName">First Name</label>
        <InputText id="firstName" type="text" size="large" class="w-6/12" />
      </FloatLabel>
      <FloatLabel>
        <label for="lastName">Last Name</label>
        <InputText id="lastName" type="text" size="large" class="w-6/12" />
      </FloatLabel>
      <FloatLabel>
        <label for="email">Email</label>
        <InputText id="email" type="text" disabled size="large" class="w-6/12" />
      </FloatLabel>
    </form>
  </div>
</template>

<script setup lang="ts">
import InputText from 'primevue/inputtext';
import { FloatLabel, ProgressSpinner, Button } from 'primevue';
</script>
