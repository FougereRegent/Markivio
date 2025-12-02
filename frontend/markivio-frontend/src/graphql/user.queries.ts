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
}
`;
