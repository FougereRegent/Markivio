import { type UserInformation, type UserUpdate } from "@/domain/user.models";
import { Result } from "typescript-result";
import { apolloClient } from "@/config/apollo.config";
import { GetMe, UpdateUser } from "@/graphql/user.queries";
import { catchError, from, map, of, switchMap } from "rxjs";


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

export function validateUser(user: UserUpdate | UserInformation) {
  const regexFirstName = new RegExp("");
  const regexLastName = new RegExp("");

  //if (regexFirstName.test(user.FirstName ?? "")) {
  //  return Result.error("");
  //}

  //if (regexLastName.test(user.LastName ?? "")) {
  //  return Result.error("");
  //}

  return Result.ok();
};

export function updateUser(user: UserUpdate | UserInformation) {
  return of(user).pipe(
    switchMap(u => {
      const resultValidation = validateUser(u);
      if (!resultValidation.isResult)
        return of(Result.error(resultValidation.error));

      return from(apolloClient.mutate({
        mutation: UpdateUser,
        variables: { firstName: u.FirstName, lastName: u.LastName }
      })).pipe(
        map(data => {
          if (data.error)
            return Result.error("idk")
          return Result.ok({
            Id: data.data?.me.id,
            FirstName: data.data?.me.firstName,
            LastName: data.data?.me.lastName,
            Email: data.data?.me.email
          } as UserInformation)
        }),
        catchError(err => of(Result.error(err)))
      )
    }),
  )
};

