import { CombinedGraphQLErrors, CombinedProtocolErrors, LocalStateError, ServerError, UnconventionalError } from "@apollo/client";

export enum ErrType {
  auth,
  business,
  forbidden,
  network,
  server,
  graphql,
  unknown,
}

export interface Err {
  type: ErrType;
  message: string;
}

export function mapGraphqlError(error: unknown): Err {
  if(CombinedGraphQLErrors.is(error)) {
    return {
      message: "",
      type: ErrType.graphql
    };
  }
  if(CombinedProtocolErrors.is(error)) {
    return {
      message: "",
      type: ErrType.network
    }
  }
  if(LocalStateError.is(error)) {
    return {
      message: "",
      type: ErrType.unknown
    }
  }
  if(ServerError.is(error)) {
    return {
      message: "",
      type: ErrType.server
    }
  }
  if(UnconventionalError.is(error)) {
    return {
      message: "",
      type: ErrType.unknown
    }
  }
  else {
    return {
      message: "",
      type: ErrType.unknown
    }
  }

}

interface ApolloLikeError {
  networkError?: unknown;
  graphQLErrors?: Array<{ extensions?: Record<string, unknown> }>;
}

function isApolloLikeError(err: unknown): err is ApolloLikeError {
  return (
    typeof err === 'object' &&
    err !== null &&
    ('networkError' in err || 'graphQLErrors' in err)
  );
}

export function mapApolloError(err: unknown): Err[] {
  if (isApolloLikeError(err)) {
    if (err.networkError) {
      return [{ type: ErrType.network, message: 'Network error, please check your connection' }];
    }
    if (err.graphQLErrors && err.graphQLErrors.length > 0) {
      return err.graphQLErrors.map((e) => mapGraphqlError(e.extensions?.code as string));
    }
  }
  return [{ type: ErrType.unknown, message: 'An unexpected error occurred' }];
}
