<script setup lang="ts">
import InputText from 'primevue/inputtext'
import { FloatLabel, Button } from 'primevue'
import { computed } from 'vue'
import { useGetMyUser, useUpdateUserInformation } from '@/features/auth/composables/user.graphql'
import { useZodValidation } from '@/features/auth/composables/zod.composable'
import { UserSchema } from '@/features/auth/models/user.models'

const { userInfo } = useGetMyUser()
const { updateUser } = useUpdateUserInformation(userInfo)
const { validate, errors } = useZodValidation(UserSchema, userInfo)

const showErrors = computed(() => ({
  firstName: errors.value?.firstName != undefined,
  lastName: errors.value?.lastName != undefined,
  email: errors.value?.email != undefined,
}))

async function submit() {
  if (validate()) {
    await updateUser()
  }
}
</script>

<template>
  <div class="p-5 h-full">
    <div class="flex flex-row justify-between">
      <h1 class="text-4xl text-gray-900">Account Settings</h1>
      <Button size="large" label="Save" class="w-2/12" @click="submit" />
    </div>
    <form class="flex flex-col mt-2 h-5/24 justify-around">
      <FloatLabel>
        <label for="firstName">First Name</label>
        <InputText
          id="firstName"
          type="text"
          v-model="userInfo.firstName"
          size="large"
          class="w-6/12"
          :invalid="showErrors.firstName"
        />
      </FloatLabel>
      <FloatLabel>
        <label for="lastName">Last Name</label>
        <InputText
          id="lastName"
          type="text"
          v-model="userInfo.lastName"
          class="w-6/12"
          :invalid="showErrors.lastName"
        />
      </FloatLabel>
      <FloatLabel>
        <label for="email">Email</label>
        <InputText
          id="email"
          type="text"
          v-model="userInfo.email"
          disabled
          size="large"
          class="w-6/12"
        />
      </FloatLabel>
    </form>
  </div>
</template>
