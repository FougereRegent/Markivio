<template>
  <div>
    <Button @click="login" label="Log in" severity="secondary" />
    <Button @click="showToken" v-if="isAuthentificated" label="Show Token In Console" />
    <span>{{ user }}</span>
  </div>
</template>
<script lang="ts">
import { useAuth0 } from '@auth0/auth0-vue';
import { Button } from 'primevue';

export default {
  setup() {
    const { loginWithRedirect, isAuthenticated, getAccessTokenSilently, user } = useAuth0();

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
    };
  }
};
</script>
