import { useAuthStore } from "@/stores/AuthStore";
import { Client, cacheExchange, fetchExchange } from "@urql/vue";

export const client = new Client({
  exchanges: [cacheExchange, fetchExchange],
  url: "http://localhost:8082/graphql/",
  fetchOptions: () => {
    const authStore = useAuthStore();
    const token = authStore.token;
    return {
      headers: { 'Authorization': `Bearer ${token}`, }
    };
  },
});

