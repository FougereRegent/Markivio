import { type UserInformation, type UserUpdate } from "@/domain/user.models";
import { Result } from "typescript-result";
import { apolloClient } from "@/config/apollo.config";
import { GetMe } from "@/graphql/user.queries";
import { from, map } from "rxjs";


export function getMe() {
  const observable = from(apolloClient.query({
    query: GetMe,
    fetchPolicy: "cache-first"
  }));

  return observable.pipe(
    map(src => src.data),
    map((src) => {
      return {
        Email: src?.me.email,
        FirstName: src?.me.firstName,
        LastName: src?.me.lastName,
        Id: src?.me.id
      } as UserInformation
    }),
  )
};

export function validateUser(user: UserUpdate) {
  const regexFirstName = new RegExp("");
  const regexLastName = new RegExp("");

  debugger;
  if (regexFirstName.test(user.FirstName ?? "")) {
    return Result.error("");
  }

  if (regexLastName.test(user.LastName ?? "")) {
    return Result.error("");
  }

  return Result.ok();
};

export async function updateUser(user: UserUpdate) {

  //Implements logic here
  const resultValidation = validateUser(user);
  if (!resultValidation.isResult)
    return resultValidation;

};

