<template>
  <div class="p-5 h-full flex flex-col justify-center" hidden>
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
        <InputText id="firstName" type="text" v-model="user.FirstName" size="large" class="w-6/12" />
      </FloatLabel>
      <FloatLabel>
        <label for="lastName">Last Name</label>
        <InputText id="lastName" type="text" v-model="user.LastName" class="w-6/12" />
      </FloatLabel>
      <FloatLabel>
        <label for="email">Email</label>
        <InputText id="email" type="text" v-model="user.Email" disabled size="large" class="w-6/12" />
      </FloatLabel>
    </form>
  </div>
</template>

<script setup lang="ts">
import InputText from 'primevue/inputtext';
import { FloatLabel, ProgressSpinner, Button } from 'primevue';
import { onMounted, ref } from 'vue';
import type { UserInformation } from '@/domain/user.models';
import { getMe } from '@/services/user.service';

const user = ref<UserInformation>({ FirstName: "", LastName: "", Email: "", Id: "" } as UserInformation);

onMounted(async () => {
  const data = await getMe();
  user.value = data;
})
</script>
