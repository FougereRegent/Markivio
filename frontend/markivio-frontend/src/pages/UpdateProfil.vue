<script setup lang="ts">
import InputText from 'primevue/inputtext';
import { FloatLabel, Button } from 'primevue';
import { onMounted, onUnmounted, ref } from 'vue';
import type { UserInformation } from '@/domain/user.models';
import { getMe, updateUser } from '@/services/user.service';
import { concatMap, debounceTime, Subject, tap, type Subscription } from 'rxjs';
import { CONST } from '@/config/constante.config';
import { useLoaderStore } from '@/stores/loader-store';
import { useToast } from 'primevue';

const user = ref<UserInformation>({
  firstName: '',
  lastName: '',
  email: '',
  id: '',
});
const savedClick = new Subject<void>();
const loadingStore = useLoaderStore();
const toast = useToast();

const invalidField = ref({
  invalidFirstNameField: false,
  invalidLastNameField: false,
});

const fieldValidationHandlers: Record<string, (state: boolean) => void> = {
  firstName: (state: boolean) => (invalidField.value.invalidFirstNameField = state),
  lastName: (state: boolean) => (invalidField.value.invalidLastNameField = state),
};

let subscribe: Subscription;
let clickSubscribe: Subscription;

onMounted(() => {
  loadingStore.start();
  subscribe = getMe().subscribe({
    next: (src) => {
      loadingStore.stop();
      user.value = src;
    },
    error: (err) => {
      console.error(err);
      loadingStore.stop();
    },
  });

  clickSubscribe = savedClick
    .pipe(
      tap(() => loadingStore.start()),
      debounceTime(CONST.debounceTime.buttonTime),
      concatMap(() => updateUser(user.value)),
    )
    .subscribe((result) => {
      if (!result.ok) {
        const error = result.error;
        if (error && 'issues' in error) {
          for (const issue of error.issues) {
            const fieldName = issue.path?.[0] as string;
            fieldValidationHandlers[fieldName]?.call(null, true);
          }
        }
        loadingStore.stop();
        return;
      }

      fieldValidationHandlers['firstName']?.call(null, false);
      fieldValidationHandlers['lastName']?.call(null, false);
      user.value = result.value ?? user.value;
      loadingStore.stop();
      toast.add({
        severity: 'success',
        summary: 'Profile updated',
        life: CONST.toastTime,
        group: 'tr',
      });
    });
});

onUnmounted(() => {
  subscribe?.unsubscribe();
  clickSubscribe?.unsubscribe();
});
</script>

<template>
  <div class="p-5 h-full">
    <div class="flex flex-row justify-between">
      <h1 class="text-4xl text-gray-900">Account Settings</h1>
      <Button size="large" label="Save" class="w-2/12" @click="savedClick.next()" />
    </div>
    <form class="flex flex-col mt-2 h-5/24 justify-around">
      <FloatLabel>
        <label for="firstName">First Name</label>
        <InputText
          id="firstName"
          type="text"
          v-model="user.firstName"
          size="large"
          class="w-6/12"
          :invalid="invalidField.invalidFirstNameField"
        />
      </FloatLabel>
      <FloatLabel>
        <label for="lastName">Last Name</label>
        <InputText
          id="lastName"
          type="text"
          v-model="user.lastName"
          class="w-6/12"
          :invalid="invalidField.invalidLastNameField"
        />
      </FloatLabel>
      <FloatLabel>
        <label for="email">Email</label>
        <InputText id="email" type="text" v-model="user.email" disabled size="large" class="w-6/12" />
      </FloatLabel>
    </form>
  </div>
</template>
