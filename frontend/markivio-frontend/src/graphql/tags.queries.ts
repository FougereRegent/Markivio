import { gql, type TypedDocumentNode } from '@apollo/client';

export type GetTagsInformationQuery = {
  tags: {
    __typename: 'tags';
    items: Array<{
      id: string;
      name: string;
      color: string;
    }>;
    totalCount: number;
    pageInfo: {
      hasNextPage: boolean;
      hasPreviousPage: boolean;
    };
  };
};

export type TagsInformationQuery = {
  nodes: Array<{
    id: string,
    name: string,
    color: string,
  }>
};

export const GetAllTags: TypedDocumentNode<GetTagsInformationQuery> = gql`
  query Tags($skip: Int!, $take: Int!, $tagName: String) {
    tags(skip: $skip, take: $take, tagName: $tagName) {
      __typename
      totalCount
      items {
        id
        name
        color
      }
      pageInfo {
        hasNextPage
        hasPreviousPage
      }
    }
  }
`;


export const AddTags: TypedDocumentNode<GetTagsInformationQuery> = gql`
mutation CreateTag($input: [CreateTagInput!]!) {
  createTags(createTags: $input) {
    nodes {
        id
        name
        color
      }
  }
}
`;
