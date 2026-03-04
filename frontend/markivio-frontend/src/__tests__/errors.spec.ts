import { describe, expect, it } from 'vitest';
import { mapGraphqlError, ErrType } from '@/errors/errors';

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
