import { describe, expect, it } from 'vitest'
import { TagSchema } from '@/features/tag/models/tag.models'
import { randomUUID } from 'crypto'

describe('TagSchema', () => {
  const validTag = {
    name: 'Vue',
    id: randomUUID(),
    color: '#ff0000',
  }

  it('Should validate a valid tag', () => {
    const result = TagSchema.safeParse(validTag)
    expect(result.success).toBe(true)
  })

  it('Should accept null id', () => {
    const tag = { ...validTag, id: null }
    const result = TagSchema.safeParse(tag)
    expect(result.success).toBe(true)
  })

  it('Should reject invalid GUID id', () => {
    const tag = { ...validTag, id: 'invalid-guid' }
    const result = TagSchema.safeParse(tag)
    expect(result.success).toBe(false)
  })

  it('Should accept any string name', () => {
    const tag = { ...validTag, name: 'React' }
    const result = TagSchema.safeParse(tag)
    expect(result.success).toBe(true)
  })

  it('Should accept empty string name', () => {
    const tag = { ...validTag, name: '' }
    const result = TagSchema.safeParse(tag)
    expect(result.success).toBe(true)
  })

  it('Should accept any string color', () => {
    const tag = { ...validTag, color: 'red' }
    const result = TagSchema.safeParse(tag)
    expect(result.success).toBe(true)
  })

  it('Should reject missing required fields', () => {
    const result = TagSchema.safeParse({})
    expect(result.success).toBe(false)
  })

  it('Should validate multiple valid tags', () => {
    const tags = [
      { name: 'Vue', id: randomUUID(), color: '#ff0000' },
      { name: 'React', id: randomUUID(), color: '#61dafb' },
    ]
    tags.forEach((tag) => {
      const result = TagSchema.safeParse(tag)
      expect(result.success).toBe(true)
    })
  })
})
