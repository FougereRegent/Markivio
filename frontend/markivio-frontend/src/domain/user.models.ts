import { nameof, Validation, ValidationError } from "@/helpers/validation.helpers"
import { Result } from "typescript-result"

export interface UserInformation {
  Id?: string | undefined
  FirstName: string | undefined
  LastName: string | undefined
  Email: string
}


export function validateUser(user: UserInformation) {
  const regexFirstName = new RegExp("^[A-Za-zÀ-ÿà-ÿ\-\'’]+(?: [\.\'’\,A-Za-zÀ-ÿà-ÿ\-]+)*$");
  const regexLastName = new RegExp("^[A-Za-zÀ-ÿà-ÿ\-\'’]+(?: [\.\'’\,A-Za-zÀ-ÿà-ÿ\-]+)*$");
  const validation = new Validation();

  debugger;
  validation.IsValid(() => regexFirstName.test(user.FirstName ?? ""), {
    properyName: nameof<UserInformation>("FirstName"),
    errorMessage: "FirstName doesn't respect format",
  }).IsValid(() => regexLastName.test(user.LastName ?? ""), {
    properyName: nameof<UserInformation>("LastName"),
    errorMessage: "LastName doesn't respect format"
  })

  return Result.try(() => validation.Throw())
    .mapError(err => err as ValidationError);
}
