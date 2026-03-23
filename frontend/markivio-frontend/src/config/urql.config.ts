import {cacheExchange, Client, fetchExchange} from '@urql/vue';

function getToken(): string | null {
  return "eyJhbGciOiJSUzI1NiIsInR5cCI6IkpXVCIsImtpZCI6IkpQZzA0RXJvajJGVEg1MmVURWJqTCJ9.eyJpc3MiOiJodHRwczovL21hcmtpdmlvLmV1LmF1dGgwLmNvbS8iLCJzdWIiOiJnb29nbGUtb2F1dGgyfDExMjExMDYzMTEwNDcxNDAyMTg3NyIsImF1ZCI6WyJodHRwczovL2xvY2FsaG9zdDo4MDgwLyIsImh0dHBzOi8vbWFya2l2aW8uZXUuYXV0aDAuY29tL3VzZXJpbmZvIl0sImlhdCI6MTc3NDI3ODkxMCwiZXhwIjoxNzc0MzY1MzEwLCJzY29wZSI6Im9wZW5pZCBwcm9maWxlIGVtYWlsIiwiYXpwIjoiVFdsYlZBZ25Pem5FVVJHekZidkFydTE1aUdZMkNiQVcifQ.IgN1ImkBgDJAclCvoPOsQ6N7h-knMybQdsrzc_vjVM2ZOyRnNBxCyH2FS2YB9f_xVsLVUrp0G8peiPGe5Fnew8lLINUs9pKYBABH23w95gpKmyO9G0A8PK0arZWk4YRNEW87t76LaSXJCe4FKecLX9AZmPUmuJ__X_b-h3oVpXS9oYLz6ezwKbywEQ-x6NAVbAYSWlgA5vKnBwKbcQgRlP_pfJQlliRdVBMSfdBVAUrlIgEiTl9QMu5leuHJ5PPU887GvDHd74C02hippng7DWP0FBMyrso9PYVPEEToxwdo2Bxr8KWGF4rJhkhawgNyzg-xbM8hMNwMYcblE7UCSA";
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
