import { describe, test, expect, assert } from "vitest";
import { nameof, Validation, ValidationError } from '@/helpers/validation.helpers'

describe("Validations", () => {
  test("Should add error when callback return false", () => {
    const validation = new Validation();
    validation.IsValid(() => false, {
      errorMessage: "error message"
    });

    expect(validation._errors.length).greaterThan(0);
  });

  test("Should not add error when callback return true", () => {
    const validation = new Validation();
    validation.IsValid(() => true, {
      errorMessage: "error message"
    });

    expect(validation._errors.length).eq(0);
  });

  test("HasError should be true when callback return false", () => {
    const validation = new Validation();
    validation.IsValid(() => false, {
      errorMessage: "error message"
    });

    expect(validation.HasError).eq(true);
  });

  test("HasError should be false when callback return true", () => {
    const validation = new Validation();
    validation.IsValid(() => true, {
      errorMessage: "error message"
    });

    expect(validation.HasError).eq(false);
  });

  test("Errors occured should throw", () => {
    const validation = new Validation();
    validation.IsValid(() => false, {
      errorMessage: "error message"
    });

    expect(() => validation.Throw())
      .toThrowError(ValidationError);
  });

  test("None Errors occured should not throw", () => {
    const validation = new Validation();
    validation.IsValid(() => true, {
      errorMessage: "error message"
    });

    validation.Throw()
  });
});

describe("nameof", () => {
  type Color = "BLUE" | "BROWN" | "GREEN";

  interface ObjTest1 {
    FirstName: string,
    LastName: string,
    Age: number,
    EyeColor: Color
  };

  test.each([
    { propertyName: 'FirstName' },
    { propertyName: 'LastName' },
    { propertyName: 'Age' },
    { propertyName: 'EyeColor' }])
    ("Get Name", ({ propertyName }) => {
      const name = nameof<ObjTest1>(propertyName as keyof ObjTest1);
      expect(name).eq(propertyName);
    });
});
