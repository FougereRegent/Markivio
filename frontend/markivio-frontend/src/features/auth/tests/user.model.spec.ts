import { describe, expect, test } from 'vitest'
import { faker } from '@faker-js/faker'
import { validateUser, type UserInformation } from '@/features/auth/models/user.models'
import { randomUUID } from 'crypto'

describe.concurrent('Validate User', () => {
  describe('firstName and lastName match regex', () => {
    const singleNameValues = new Array<UserInformation>()
    for (let i = 0; i < 20; ++i) {
      singleNameValues.push({
        id: randomUUID(),
        email: faker.internet.email(),
        firstName: faker.person.firstName(),
        lastName: faker.person.lastName(),
      })
    }
    const doubleNameValues = new Array<UserInformation>()
    for (let i = 0; i < 20; ++i) {
      doubleNameValues.push({
        id: randomUUID(),
        email: faker.internet.email(),
        firstName: `${faker.person.firstName()} ${faker.person.firstName()}`,
        lastName: `${faker.person.lastName()} ${faker.person.lastName()}`,
      })
    }

    test.each(singleNameValues)('Validate $firstName $lastName', (user) => {
      const result = validateUser(user)
      expect(result.success).eq(true)
    })

    test.each(doubleNameValues)('Validate with double name $firstName $lastName', (user) => {
      const result = validateUser(user)
      expect(result.success).eq(true)
    })
  })

  describe("firstName and lastName doesn't match regex", () => {
    test("firstName doesn't match regex should return bad result", () => {
      const user: UserInformation = {
        id: randomUUID(),
        email: faker.internet.email(),
        firstName: faker.string.hexadecimal(),
        lastName: faker.person.lastName(),
      }
      const result = validateUser(user)
      expect(result.success).eq(false)
    })

    test("lastName doesn't match regex should return bad result", () => {
      const user: UserInformation = {
        id: randomUUID(),
        email: faker.internet.email(),
        firstName: faker.person.firstName(),
        lastName: faker.string.hexadecimal(),
      }
      const result = validateUser(user)
      expect(result.success).eq(false)
    })

    test("firstName and lastName don't match regex should return bad result", () => {
      const user: UserInformation = {
        id: randomUUID(),
        email: faker.internet.email(),
        firstName: faker.string.hexadecimal(),
        lastName: faker.string.hexadecimal(),
      }
      const result = validateUser(user)
      expect(result.success).eq(false)
    })
  })
})
