import {cacheExchange, Client, fetchExchange} from '@urql/vue';
import { authExchange, type AuthConfig } from '@urql/exchange-auth';
import { useAuthStore } from '@/stores/auth-store';

function getToken(): string | null {
  return "eyJhbGciOiJSUzI1NiIsInR5cCI6IkpXVCIsImtpZCI6IkpQZzA0RXJvajJGVEg1MmVURWJqTCJ9.eyJpc3MiOiJodHRwczovL21hcmtpdmlvLmV1LmF1dGgwLmNvbS8iLCJzdWIiOiJnb29nbGUtb2F1dGgyfDExMjExMDYzMTEwNDcxNDAyMTg3NyIsImF1ZCI6WyJodHRwczovL2xvY2FsaG9zdDo4MDgwLyIsImh0dHBzOi8vbWFya2l2aW8uZXUuYXV0aDAuY29tL3VzZXJpbmZvIl0sImlhdCI6MTc3NDUxNTAzNywiZXhwIjoxNzc0NjAxNDM3LCJzY29wZSI6Im9wZW5pZCBwcm9maWxlIGVtYWlsIiwiYXpwIjoiVFdsYlZBZ25Pem5FVVJHekZidkFydTE1aUdZMkNiQVcifQ.WgxJPVTjI2I8Q26ZmSZPEfiNNYF9L8vAz69cMiZgqKnggqZlO6_U3VYIV2yzOcaxtNXktKU4jG7w-i_H6G21kQmwRKwXsNvRSfIwJ_gxSArVrscHUOhItI0ZD93KZipmxTW7O-w5rD-NyargRASxMBD89NTXUiiWjc7f0VZJqnumV3q-s3azCltPMH-49-N4hogz6ZVBgsAKUd9URDIntDQAYDUL5UgYHJgBvuNo6ji1hYsC5hdADvtVYEOLgl5UHP8ipp60lpY-0YiAVQHuWzTJ11aCAv4hy6JvIZv4K1d3q67_OF1CAJnDRQoib4Uf3n_kitX9UcG9j9OYl1XGow";
}


async function initializeAuthState() {
  const auth = useAuthStore();
  debugger;
  await auth.init();
  const token = auth.token;
  return token;
}

export const httpClient = new Client({
  url: import.meta.env.VITE_MARKIVIO_GRAPHQL_API,
  exchanges: [cacheExchange, authExchange(async utils => {
    let token = await initializeAuthState();
    return {
      addAuthToOperation(operation) {
          if(!token) return operation;
          return utils.appendHeaders(operation, {
            Authorization: `Bearer ${token}`
          });
      },
    } as AuthConfig;
  }), fetchExchange],
})
