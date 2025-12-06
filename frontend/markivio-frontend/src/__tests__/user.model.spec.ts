import { describe, expect, test } from "vitest";
import { faker } from '@faker-js/faker'
import { validateUser, type UserInformation } from '@/domain/user.models';
import { randomUUID } from "crypto";


describe.concurrent("Validate User", () => {
  describe("First Name and Last Name match regex", () => {
    const singleNameValues = new Array<UserInformation>();
    for (let i = 0; i < 20; ++i) {
      singleNameValues.push({
        Id: randomUUID(),
        Email: faker.internet.email(),
        FirstName: faker.person.firstName(),
        LastName: faker.person.lastName(),
      });
    }
    const doubleNameValues = new Array<UserInformation>();
    for (let i = 0; i < 20; ++i) {
      doubleNameValues.push({
        Id: randomUUID(),
        Email: faker.internet.email(),
        FirstName: `${faker.person.firstName()} ${faker.person.firstName()}`,
        LastName: `${faker.person.lastName()} ${faker.person.lastName()}`,
      });
    }
    test.each(singleNameValues)("Validate $FirstName $LastName", (user) => {
      const result = validateUser(user);
      expect(result.ok).eq(true);
    });
    test.each(doubleNameValues)("Validate with double name $FirstName $LastName", (user) => {
      const result = validateUser(user);
      expect(result.ok).eq(true);
    });
  });

  describe("First Name and Last Name doesn't match regex", () => {
    test("First Name doesn't match regex should return bad result", () => {
      const user: UserInformation = {
        Id: randomUUID(),
        Email: faker.internet.email(),
        FirstName: faker.string.hexadecimal(),
        LastName: faker.person.lastName(),
      };
      const result = validateUser(user);
      expect(result.ok).eq(false);
      expect(result.error?.type).eq("validation-errors")
      expect(result.error?.validationErrors).toMatchSnapshot()
    });

    test("Last Name doesn't match regex should return bad result", () => {
      const user: UserInformation = {
        Id: randomUUID(),
        Email: faker.internet.email(),
        FirstName: faker.person.firstName(),
        LastName: faker.string.hexadecimal(),
      };
      const result = validateUser(user);
      expect(result.ok).eq(false);
      expect(result.error?.type).eq("validation-errors")
      expect(result.error?.validationErrors).toMatchSnapshot()
    });

    test("First Name and Last Name doesn't match with regex should return bad result", () => {
      const user: UserInformation = {
        Id: randomUUID(),
        Email: faker.internet.email(),
        FirstName: faker.string.hexadecimal(),
        LastName: faker.string.hexadecimal(),
      };
      const result = validateUser(user);
      expect(result.ok).eq(false);
      expect(result.ok).eq(false);
      expect(result.error?.type).eq("validation-errors")
      expect(result.error?.validationErrors).toMatchSnapshot()
    });
  })
});
