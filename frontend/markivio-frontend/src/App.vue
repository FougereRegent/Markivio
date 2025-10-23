<template>
  <div>
    <button @click="login">Log in</button>
    <button @click="showToken" v-if="isAuthentificated">Show Token In Console</button>
    <span>{{user}}</span>
  </div>
</template>
<script lang="ts">
  import { useAuth0 } from '@auth0/auth0-vue';

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
