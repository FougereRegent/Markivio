import { cacheExchange, fetchExchange, type Exchange } from '@urql/core'
import { authExchange } from '@urql/exchange-auth';
import { useAuthStore } from '@/stores/AuthStore';



export interface UrqlConfig {
    Url: string,
    Exchanges: Exchange[],
}

const config: UrqlConfig = {
        Url: import.meta.env.VITE_MARKIVIO_GRAPHQL_API,
        Exchanges: [authExchange(async utils => {
            const auth = useAuthStore();
            return {
                addAuthToOperation(operation) {
                const token = auth.token;
                return utils.appendHeaders(operation, {
                    Authorization: `Bearer ${token}`
                });
                }
            }
            }), cacheExchange, fetchExchange]
}

export function GetConfig(): UrqlConfig {
    return config;
}
