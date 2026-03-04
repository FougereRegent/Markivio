import { validateUser, type UserInformation } from '@/domain/user.models';
import { Result } from 'typescript-result';
import { apolloClient } from '@/config/apollo.config';
import { GetMe, UpdateUser } from '@/graphql/user.queries';
import { catchError, from, map, of, switchMap } from 'rxjs';

export function getMe() {
  const observable = from(
    apolloClient.query({
      query: GetMe,
      fetchPolicy: 'cache-first',
    }),
  );

  return observable.pipe(
    map((response) => response.data),
    map(
      (data) =>
        ({
          email: data?.me.email,
          firstName: data?.me.firstName,
          lastName: data?.me.lastName,
          id: data?.me.id,
        }) as UserInformation,
    ),
  );
}

export function updateUser(user: UserInformation) {
  return of(user).pipe(
    switchMap((u) => {
      const resultValidation = validateUser(u);
      if (!resultValidation.success)
        return of(Result.error(resultValidation.error));

      return from(
        apolloClient.mutate({
          mutation: UpdateUser,
          variables: { firstName: u.firstName, lastName: u.lastName },
        }),
      ).pipe(
        map((response) => {
          if (response.errors)
            return Result.error(response.errors);
          return Result.ok({
            id: response.data?.me.id,
            firstName: response.data?.me.firstName,
            lastName: response.data?.me.lastName,
            email: response.data?.me.email,
          } as UserInformation);
        }),
        catchError((err) => of(Result.error(err))),
      );
    }),
  );
}
