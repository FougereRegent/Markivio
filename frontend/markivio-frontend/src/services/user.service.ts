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
    const { data, error, fetching } = useQuery<MeQuery>({
      query: getMequery,
      variables: {},
    })
    return { data, error, fetching };
  };

export function validateUser(user: UserUpdate) {
  const regexFirstName = new RegExp("");
  const regexLastName = new RegExp("");

  if(regexFirstName.test(user.FirstName ?? "")) {
    return Result.error("");
  }

  if(regexLastName.test(user.LastName ?? "")) {
    return Result.error("");
  }

  return Result.ok();
};

export async function updateUser(user: UserUpdate) {

  const result = useMutation<UpdateUserMutation>(updateMyUserMutation);

  //Implements logic here
  const resultValidation = validateUser(user);
  if(!resultValidation.isResult)
    return resultValidation;

  // Execute request 
  const res = await result.executeMutation({
    firstName: user.FirstName,
    lastName: user.LastName,
  } as UpdateUserInformationInput);

  return Result.ok({
    Id: res.data?.updateMyUser?.id,
    Email: res.data?.updateMyUser?.email,
    FirstName: res.data?.updateMyUser?.firstName,
    LastName: res.data?.updateMyUser?.lastName,
  } as UserInformation);
};

