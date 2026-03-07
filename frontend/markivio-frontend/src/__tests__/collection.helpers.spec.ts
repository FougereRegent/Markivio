import { describe, expect, it } from 'vitest';
import { groupBy } from '@/helpers/collection.helpers';

interface Item {
  category: string;
  name: string;
}

describe('groupBy', () => {
  it('Should group items by key', () => {
    const items: Item[] = [
      { category: 'fruit', name: 'apple' },
      { category: 'fruit', name: 'banana' },
      { category: 'vegetable', name: 'carrot' },
    ];

    const result = groupBy(items, 'category');

    expect(Object.keys(result)).toEqual(['fruit', 'vegetable']);
    expect(result['fruit']).toHaveLength(2);
    expect(result['vegetable']).toHaveLength(1);
  });

  it('Should return empty object for empty list', () => {
    const result = groupBy([] as Item[], 'category');
    expect(result).toEqual({});
  });

  it('Should create single-element groups when all keys are unique', () => {
    const items: Item[] = [
      { category: 'a', name: 'x' },
      { category: 'b', name: 'y' },
      { category: 'c', name: 'z' },
    ];

    const result = groupBy(items, 'category');

    expect(Object.keys(result)).toHaveLength(3);
    expect(result['a']).toHaveLength(1);
    expect(result['b']).toHaveLength(1);
    expect(result['c']).toHaveLength(1);
  });

  it('Should group all items under one key when they share the same value', () => {
    const items: Item[] = [
      { category: 'same', name: 'a' },
      { category: 'same', name: 'b' },
      { category: 'same', name: 'c' },
    ];

    const result = groupBy(items, 'category');

    expect(Object.keys(result)).toHaveLength(1);
    expect(result['same']).toHaveLength(3);
  });

  it('Should group by a different key', () => {
    const items: Item[] = [
      { category: 'fruit', name: 'apple' },
      { category: 'vegetable', name: 'apple' },
    ];

    const result = groupBy(items, 'name');

    expect(Object.keys(result)).toEqual(['apple']);
    expect(result['apple']).toHaveLength(2);
  });
});
