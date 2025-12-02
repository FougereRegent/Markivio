<template>
  <div class="p-5">
    <h1 class="text-4xl text-gray-900" >Account Settings</h1>
    <form class="flex flex-col mt-6">
      <FloatLabel>
        <label for="firstName">First Name</label>
        <InputText id="firstName" type="text" v-model="firstName" />
      </FloatLabel>
      <FloatLabel>
        <label for="firstName">First Name</label>
        <InputText id="firstName" type="text" v-model="firstName" />
      </FloatLabel>
      <FloatLabel>
        <label for="firstName">First Name</label>
        <InputText id="firstName" type="text" v-model="firstName" />
      </FloatLabel>
    </form>
  </div>
</template>

<script setup lang="ts">
import { getMe } from '@/services/user.service';
import { ref, watch } from 'vue';
import InputText from 'primevue/inputtext';
import { FloatLabel } from 'primevue';

const { data } = getMe();

const firstName = ref("");
const lastName = ref("");
const email = ref("");

watch(data, (dataNew) => {
  firstName.value = dataNew?.me?.firstName ?? "";
  lastName.value = dataNew?.me?.lastName ?? "";
  email.value = dataNew?.me?.email ?? "";
});
</script>
