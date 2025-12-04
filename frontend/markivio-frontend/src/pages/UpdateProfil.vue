<template>
  <div class="p-5 h-full flex flex-col justify-center" v-show="loading">
    <ProgressSpinner />
  </div>
  <div class="p-5 h-full" v-show="!loading">
    <div class="flex flex-row justify-between">
      <h1 class="text-4xl text-gray-900">Account Settings</h1>
      <Button size="large" label="Save" class="w-2/12" @click="savedClick.next();" />
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
import { onMounted, onUnmounted, ref } from 'vue';
import type { UserInformation } from '@/domain/user.models';
import { getMe, updateUser } from '@/services/user.service';
import { concatMap, debounce, debounceTime, sampleTime, Subject, type Subscription } from 'rxjs';
import { CONST } from '@/config/constante.config';

const user = ref<UserInformation>({ FirstName: "", LastName: "", Email: "", Id: "" } as UserInformation);
const loading = ref(true);
const savedClick = new Subject<void>();

let subscribe: Subscription;
let clickSubscribe: Subscription;

onMounted(() => {
  const onNext = (src: UserInformation) => {
    user.value = src;
    loading.value = false;
  }

  subscribe = getMe()
    .subscribe({
      next: onNext,
      error: (err) => { console.error(err) }
    });

  clickSubscribe = savedClick
    .pipe(
      debounceTime(CONST.debounceTime.buttonTime),
      concatMap(_ => updateUser(user.value))
    ).subscribe(pre => {
      debugger;
      if (!pre.isResult) {
        console.log(pre.error[0])
        return;
      }

      user.value = pre.value ?? user.value;
    })
});

onUnmounted(() => {
  subscribe?.unsubscribe();
  clickSubscribe?.unsubscribe();
});

</script>
