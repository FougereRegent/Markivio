<script setup lang="ts">
import InputText from 'primevue/inputtext'
import { FloatLabel, Button } from 'primevue'
import { computed } from 'vue'
import { useGetMyUser, useUpdateUserInformation } from '@/features/auth/composables/user.graphql'
import { useZodValidation } from '@/features/auth/composables/zod.composable'
import { UserSchema } from '@/features/auth/models/user.models'
import { useI18n } from 'vue-i18n'

const { t } = useI18n()
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
  <div class="h-full overflow-y-auto">
    <div class="max-w-2xl mx-auto p-4 md:p-6 lg:p-8">
      <div class="flex flex-col sm:flex-row sm:items-center justify-between gap-4 mb-6">
        <h1 class="text-2xl md:text-3xl font-bold text-gray-900">
          {{ t('profile.accountSettings') }}
        </h1>
        <Button
          :label="t('profile.save')"
          icon="ri-check-line"
          size="large"
          class="w-full sm:w-auto"
          @click="submit"
        />
      </div>
      <div class="bg-white rounded-xl border border-gray-200 shadow-sm">
        <div class="p-4 md:p-6 space-y-5">
          <div class="grid grid-cols-1 md:grid-cols-2 gap-4">
            <FloatLabel>
              <label for="firstName">{{ t('profile.firstName') }}</label>
              <InputText
                id="firstName"
                type="text"
                v-model="userInfo.firstName"
                size="large"
                class="w-full"
                :invalid="showErrors.firstName"
              />
            </FloatLabel>
            <FloatLabel>
              <label for="lastName">{{ t('profile.lastName') }}</label>
              <InputText
                id="lastName"
                type="text"
                v-model="userInfo.lastName"
                size="large"
                class="w-full"
                :invalid="showErrors.lastName"
              />
            </FloatLabel>
          </div>
          <FloatLabel>
            <label for="email">{{ t('profile.email') }}</label>
            <InputText
              id="email"
              type="text"
              v-model="userInfo.email"
              disabled
              size="large"
              class="w-full"
            />
          </FloatLabel>
        </div>
      </div>
    </div>
  </div>
</template>
