import { beforeAll, describe, expect, test } from "vitest";
import { fa, faker } from '@faker-js/faker'
import { validateUser, type UserInformation } from '@/domain/user.models';
import { randomUUID } from "crypto";
import { nameof } from "@/helpers/validation.helpers";


describe.concurrent("Validate User", () => {
  describe("First Name and Last Name match regex", () => {
    const values = new Array<UserInformation>();
    for (let i = 0; i < 20; ++i) {
      values.push({
        Id: randomUUID(),
        Email: faker.internet.email(),
        FirstName: faker.person.firstName(),
        LastName: faker.person.lastName(),
      });
    }
    test.each(values)("Validate $FirstName $LastName", (user) => {
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
