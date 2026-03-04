import { describe, expect, it } from 'vitest';
import { faker } from '@faker-js/faker';
import { validateArticle, type Article } from '@/domain/article.models';
import { randomUUID } from 'crypto';

describe('validateArticle', () => {
  it('Should validate a correct article', () => {
    const article: Article = {
      id: null,
      title: faker.lorem.word(),
      source: faker.internet.url(),
      description: faker.lorem.sentence(),
      tags: [],
    };

    const result = validateArticle(article);
    expect(result.success).eq(true);
  });

  it('Should validate article with tags', () => {
    const article: Article = {
      id: null,
      title: faker.lorem.word(),
      source: faker.internet.url(),
      description: faker.lorem.sentence(),
      tags: [
        { id: randomUUID(), name: faker.lorem.word(), color: '#FF0000' },
        { id: randomUUID(), name: faker.lorem.word(), color: '#00FF00' },
      ],
    };

    const result = validateArticle(article);
    expect(result.success).eq(true);
  });

  it('Should fail when title is empty', () => {
    const article: Article = {
      id: null,
      title: '',
      source: faker.internet.url(),
      description: faker.lorem.sentence(),
      tags: [],
    };

    const result = validateArticle(article);
    expect(result.success).eq(false);
  });

  it('Should fail when source is not a valid URL', () => {
    const article: Article = {
      id: null,
      title: faker.lorem.word(),
      source: 'not-a-url',
      description: faker.lorem.sentence(),
      tags: [],
    };

    const result = validateArticle(article);
    expect(result.success).eq(false);
  });

  it('Should fail when tag has invalid structure', () => {
    const article = {
      id: null,
      title: faker.lorem.word(),
      source: faker.internet.url(),
      description: faker.lorem.sentence(),
      tags: [{ name: faker.lorem.word() }],
    };

    const result = validateArticle(article as unknown as Article);
    expect(result.success).eq(false);
  });
});
