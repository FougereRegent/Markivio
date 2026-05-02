import { describe, expect, it } from 'vitest'
import { ArticleSchema } from '@/features/article/models/article.models'
import { randomUUID } from 'crypto'

describe('ArticleSchema', () => {
  const validArticle = {
    id: randomUUID(),
    title: 'Test Article',
    source: 'https://example.com',
    description: 'A test description',
    tags: [],
  }

  it('Should validate a valid article', () => {
    const result = ArticleSchema.safeParse(validArticle)
    expect(result.success).toBe(true)
  })

  it('Should accept null id', () => {
    const article = { ...validArticle, id: null }
    const result = ArticleSchema.safeParse(article)
    expect(result.success).toBe(true)
  })

  it('Should reject invalid GUID id', () => {
    const article = { ...validArticle, id: 'invalid-guid' }
    const result = ArticleSchema.safeParse(article)
    expect(result.success).toBe(false)
  })

  it('Should reject empty title', () => {
    const article = { ...validArticle, title: '' }
    const result = ArticleSchema.safeParse(article)
    expect(result.success).toBe(false)
  })

  it('Should reject invalid URL source', () => {
    const article = { ...validArticle, source: 'not-a-url' }
    const result = ArticleSchema.safeParse(article)
    expect(result.success).toBe(false)
  })

  it('Should accept valid HTTP URL', () => {
    const article = { ...validArticle, source: 'http://example.com' }
    const result = ArticleSchema.safeParse(article)
    expect(result.success).toBe(true)
  })

  it('Should accept null description', () => {
    const article = { ...validArticle, description: null }
    const result = ArticleSchema.safeParse(article)
    expect(result.success).toBe(true)
  })

  it('Should validate article with tags', () => {
    const article = {
      ...validArticle,
      tags: [
        { name: 'Vue', id: randomUUID(), color: '#ff0000' },
      ],
    }
    const result = ArticleSchema.safeParse(article)
    expect(result.success).toBe(true)
  })

  it('Should reject tag with missing required fields', () => {
    const article = {
      ...validArticle,
      tags: [{ }],
    }
    const result = ArticleSchema.safeParse(article)
    expect(result.success).toBe(false)
  })
})
