import { describe, expect, it } from 'vitest';
import { mapGraphqlError, mapApolloError, ErrType } from '@/errors/errors';

describe('mapGraphqlError', () => {
  it('Should map AlreadyExistError to business error', () => {
    const result = mapGraphqlError('AlreadyExistError');
    expect(result.type).eq(ErrType.business);
    expect(result.message).eq('This item already exist');
  });

  it('Should map DuplicatedItemsError to business error', () => {
    const result = mapGraphqlError('DuplicatedItemsError');
    expect(result.type).eq(ErrType.business);
    expect(result.message).eq('Duplicated items in your collections');
  });

  it('Should map NotFoundError to business error', () => {
    const result = mapGraphqlError('NotFoundError');
    expect(result.type).eq(ErrType.business);
    expect(result.message).eq('Item not found');
  });

  it('Should map UnauthorizedError to auth error', () => {
    const result = mapGraphqlError('UnauthorizedError');
    expect(result.type).eq(ErrType.auth);
    expect(result.message).eq('Not connected');
  });

  it('Should map unknown error code to unknown type with default message', () => {
    const result = mapGraphqlError('SomeRandomError');
    expect(result.type).eq(ErrType.unknown);
    expect(result.message).eq('An unexpected error occurred');
  });

  it('Should map empty string to unknown type', () => {
    const result = mapGraphqlError('');
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
    expect(result[0].type).eq(ErrType.network);
    expect(result[0].message).eq('Network error, please check your connection');
  });

  it('Should map graphQLErrors from Apollo-like error', () => {
    const error = {
      graphQLErrors: [
        { message: 'Already exists', extensions: { code: 'AlreadyExistError' } },
      ],
    };

    const result = mapApolloError(error);

    expect(result).toHaveLength(1);
    expect(result[0].type).eq(ErrType.business);
    expect(result[0].message).eq('This item already exist');
  });

  it('Should map multiple graphQLErrors', () => {
    const error = {
      graphQLErrors: [
        { message: 'Already exists', extensions: { code: 'AlreadyExistError' } },
        { message: 'Not found', extensions: { code: 'NotFoundError' } },
      ],
    };

    const result = mapApolloError(error);

    expect(result).toHaveLength(2);
    expect(result[0].type).eq(ErrType.business);
    expect(result[1].type).eq(ErrType.business);
    expect(result[1].message).eq('Item not found');
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
    expect(result[0].type).eq(ErrType.network);
  });

  it('Should return unknown error for standard Error', () => {
    const result = mapApolloError(new Error('Something broke'));

    expect(result).toHaveLength(1);
    expect(result[0].type).eq(ErrType.unknown);
    expect(result[0].message).eq('An unexpected error occurred');
  });

  it('Should return unknown error for non-Error values', () => {
    const result = mapApolloError('string error');

    expect(result).toHaveLength(1);
    expect(result[0].type).eq(ErrType.unknown);
  });

  it('Should return unknown error for null', () => {
    const result = mapApolloError(null);

    expect(result).toHaveLength(1);
    expect(result[0].type).eq(ErrType.unknown);
  });
});
