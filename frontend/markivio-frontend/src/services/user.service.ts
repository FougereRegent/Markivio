import { type UserInformation, type UserUpdate } from "@/domain/user.models";
import { useMutation, useQuery } from "@urql/vue";
import { graphql } from "@/gql";
import { type UpdateUserInformationInput, type UpdateUserMutation, type MeQuery } from "@/gql/graphql";
import { Result } from "typescript-result";


const getMequery = graphql(`
query Me {
  me {
    id
    firstName
    lastName
    email
  }
}`);

const updateMyUserMutation = graphql(`
  mutation UpdateUser($firstName: String!, $lastName: String!) {
    updateMyUser(updateUserInformation: {
      firstName: $firstName,
      lastName: $lastName
    }){
      id
      firstName
      lastName
      email
    }
  }
`);


export function getMe() {
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

