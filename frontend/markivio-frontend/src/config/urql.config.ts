import {cacheExchange, Client, fetchExchange} from '@urql/vue';

function getToken(): string | null {
  return "eyJhbGciOiJSUzI1NiIsInR5cCI6IkpXVCIsImtpZCI6IkpQZzA0RXJvajJGVEg1MmVURWJqTCJ9.eyJpc3MiOiJodHRwczovL21hcmtpdmlvLmV1LmF1dGgwLmNvbS8iLCJzdWIiOiJnb29nbGUtb2F1dGgyfDExMjExMDYzMTEwNDcxNDAyMTg3NyIsImF1ZCI6WyJodHRwczovL2xvY2FsaG9zdDo4MDgwLyIsImh0dHBzOi8vbWFya2l2aW8uZXUuYXV0aDAuY29tL3VzZXJpbmZvIl0sImlhdCI6MTc3NDQ0MzE0NSwiZXhwIjoxNzc0NTI5NTQ1LCJzY29wZSI6Im9wZW5pZCBwcm9maWxlIGVtYWlsIiwiYXpwIjoiVFdsYlZBZ25Pem5FVVJHekZidkFydTE1aUdZMkNiQVcifQ.h64-jr8LaCrkdiIK8gJ0bVnKgnngPt4O3Y0m8RmDgCBZ7__-hRoimDpXCDwo0C56wTlAPp9GiklYM-mmnVJptxkAKBKV7jHa1DmXENY15J4xBFort6OF3m0jTEQU2f2SSvRmdDyfegS4KTzHTrzpxMRlnZd0D83mUBf3GLXngZbx6GBsh08jcKKQ4rOrpiCiTic3UQ6rYrMUR1LKtqk7gVbvxnorXdSMHxYKxRtL8ZWmmml7OBlDkOIC0WkTrh49fVrHOXLa0DDz8MMIXPX6o8LFYXWYKtmfEc0K9r9SaRwVPFuKMfSHWKIZJutjG9DIofyLuRPRO7XTBAS_a_D52Q";
}

export const httpClient = new Client({
  url: import.meta.env.VITE_MARKIVIO_GRAPHQL_API,
  exchanges: [cacheExchange, fetchExchange],
  fetchOptions: () => {
    const token = getToken();
    return {
      headers: {
        authorization: token ? `Bearer ${token}` : ''
      },
    }
  }
})
