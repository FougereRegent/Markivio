import { gql, type TypedDocumentNode } from "@apollo/client";

export type GetUserInformationQuery = {
  me: {
    __typename: "Me",
    id: string,
    firstName: string,
    lastName: string,
    email: string,
  }
};

export const GetMe: TypedDocumentNode<GetUserInformationQuery> = gql`
query Me {
  me {
    id
    firstName
    lastName
    email
  }
}`;


export const UpdateUser: TypedDocumentNode<GetUserInformationQuery> = gql`
mutation UpdateMyUser($firstName: !String, $lastName: !String){
    updateMyUser(updateUserInformation:  {
       firstName: $fistName,
       lastName: $lastName
    }){
        id
        firstName
        lastName
        email
    }
}`;
