import { nameof, Validation, ValidationError } from "@/helpers/validation.helpers"
import { Result } from "typescript-result"

export interface UserInformation {
  Id?: string | undefined
  FirstName: string | undefined
  LastName: string | undefined
  Email: string
}


export function validateUser(user: UserInformation) {
  const regexFirstName = new RegExp("");
  const regexLastName = new RegExp("");
  const validation = new Validation();

  validation.IsValid(() => regexFirstName.test(user.FirstName ?? ""), {
    properyName: nameof<UserInformation>("FirstName"),
    errorMessage: "",
  }).IsValid(() => regexLastName.test(user.LastName ?? ""), {
    properyName: nameof<UserInformation>("LastName"),
    errorMessage: ""
  })

  return Result.try(() => validation.Run())
    .mapError(err => err as ValidationError);
}
