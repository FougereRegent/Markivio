import { describe, expect, it } from 'vitest';
import { mapGraphqlError, mapApolloError, ErrType } from '@/errors/errors';

describe('mapGraphqlError', () => {
  it('Should return unknown type for unrecognized error input', () => {
    const result = mapGraphqlError('SomeRandomError');
    expect(result.type).eq(ErrType.unknown);
    expect(result.message).eq('');
  });

  it('Should return unknown type for empty string', () => {
    const result = mapGraphqlError('');
    expect(result.type).eq(ErrType.unknown);
  });

  it('Should return unknown type for null', () => {
    const result = mapGraphqlError(null);
    expect(result.type).eq(ErrType.unknown);
  });
});

describe('mapApolloError', () => {
  it('Should return network error when error has networkError', () => {
    const error = {
      networkError: new Error('Failed to fetch'),
      graphQLErrors: [],
    };

    const result = mapApolloError(error);

    expect(result).toHaveLength(1);
    expect(result[0]!.type).eq(ErrType.network);
    expect(result[0]!.message).eq('Network error, please check your connection');
  });

  it('Should map graphQLErrors to unknown when extensions code is a string', () => {
    const error = {
      graphQLErrors: [
        { message: 'Already exists', extensions: { code: 'AlreadyExistError' } },
      ],
    };

    const result = mapApolloError(error);

    expect(result).toHaveLength(1);
    expect(result[0]!.type).eq(ErrType.unknown);
  });

  it('Should map multiple graphQLErrors', () => {
    const error = {
      graphQLErrors: [
        { message: 'Error 1', extensions: { code: 'Err1' } },
        { message: 'Error 2', extensions: { code: 'Err2' } },
      ],
    };

    const result = mapApolloError(error);

    expect(result).toHaveLength(2);
    expect(result[0]!.type).eq(ErrType.unknown);
    expect(result[1]!.type).eq(ErrType.unknown);
  });

  it('Should prioritize networkError over graphQLErrors', () => {
    const error = {
      networkError: new Error('Timeout'),
      graphQLErrors: [
        { message: 'Some error', extensions: { code: 'AlreadyExistError' } },
      ],
    };

    const result = mapApolloError(error);

    expect(result).toHaveLength(1);
    expect(result[0]!.type).eq(ErrType.network);
  });

  it('Should return unknown error for standard Error', () => {
    const result = mapApolloError(new Error('Something broke'));

    expect(result).toHaveLength(1);
    expect(result[0]!.type).eq(ErrType.unknown);
    expect(result[0]!.message).eq('An unexpected error occurred');
  });

  it('Should return unknown error for non-Error values', () => {
    const result = mapApolloError('string error');

    expect(result).toHaveLength(1);
    expect(result[0]!.type).eq(ErrType.unknown);
  });

  it('Should return unknown error for null', () => {
    const result = mapApolloError(null);

    expect(result).toHaveLength(1);
    expect(result[0]!.type).eq(ErrType.unknown);
  });
});
