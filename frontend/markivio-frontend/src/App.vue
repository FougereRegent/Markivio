<template>
  <div>
    <ToggleSwitch v-model="isDarkTheme" />
    <ToggleSwitch />
    <Button @click="login" label="Log in" />
    <Button @click="showToken" v-if="isAuthentificated" label="Show Token In Console" />
    <span>{{ user }}</span>
  </div>
</template>
<script lang="ts">

import { useAuth0 } from '@auth0/auth0-vue';
import { ref, watch } from 'vue';

export default {
  setup() {
    const { loginWithRedirect, isAuthenticated, getAccessTokenSilently, user } = useAuth0();
    const isDarkTheme = ref(false);

    watch(isDarkTheme, (current) => {
      if(current)
        document.documentElement.classList.add('app-dark');
      else
        document.documentElement.classList.remove('app-dark')
    });

    return {
      login: () => {
        loginWithRedirect();
      },
      showToken: async () => {
        const result = await getAccessTokenSilently();
        console.log(result);
      },
      isAuthentificated: isAuthenticated,
      user: user,
      isDarkTheme: isDarkTheme
    };
  }
};
</script>
