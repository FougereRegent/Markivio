import { describe, expect, it } from 'vitest'
import { useZodValidation } from '@/features/auth/composables/zod.composable'
import * as z from 'zod'
import { ref } from 'vue'

const TestSchema = z.object({
  name: z.string().min(1),
  age: z.number().min(0),
})

describe('useZodValidation', () => {
  it('Should return valid when data matches schema', () => {
    const data = ref({ name: 'John', age: 25 })
    const { validate, isValid } = useZodValidation(TestSchema, data)

    const result = validate()

    expect(result).eq(true)
    expect(isValid.value).eq(true)
  })

  it('Should return invalid when data does not match schema', () => {
    const data = ref({ name: '', age: -1 })
    const { validate, isValid, errors } = useZodValidation(TestSchema, data)

    const result = validate()

    expect(result).eq(false)
    expect(isValid.value).eq(false)
    expect(errors.value).not.toBeNull()
  })

  it('Should have errors grouped by path', () => {
    const data = ref({ name: '', age: 25 })
    const { validate, errors } = useZodValidation(TestSchema, data)

    validate()

    expect(errors.value).not.toBeNull()
    expect(errors.value).toHaveProperty('name')
  })

  it('Should clear errors when validate is called with valid data', () => {
    const data = ref({ name: '', age: 25 })
    const { validate, errors } = useZodValidation(TestSchema, data)

    validate()
    expect(errors.value).not.toBeNull()

    data.value.name = 'John'
    validate()
    expect(errors.value).toBeNull()
  })

  it('Should expose validateWatch and stopWatch', () => {
    const data = ref({ name: 'John', age: 25 })
    const { validateWatch, stopWatch } = useZodValidation(TestSchema, data)

    expect(validateWatch).toBeTypeOf('function')
    expect(stopWatch).toBeTypeOf('function')
  })

  it('Should not throw when stopWatch is called without starting watch', () => {
    const data = ref({ name: 'John', age: 25 })
    const { stopWatch } = useZodValidation(TestSchema, data)

    expect(() => stopWatch()).not.toThrow()
  })
})
