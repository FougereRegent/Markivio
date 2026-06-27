import { gql, type TypedDocumentNode } from '@urql/vue'

export type GetTagsInformationQuery = {
  tags: {
    __typename: 'tags'
    items: Array<{
      id: string
      name: string
      color: string
    }>
    totalCount: number
    pageInfo: {
      hasNextPage: boolean
      hasPreviousPage: boolean
    }
  }
}

export type TagsInformationQuery = {
  nodes: Array<{
    id: string
    name: string
    color: string
  }>
}

export type TagsStatsQuery = {
  tagsStats: {
    items: Array<{
      name: string,
      color: string,
      articleNumber: number
    }>
    totalCount: number
  }
}

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
`

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

export const GetTagsAndAssociatedArticleNumber: TypedDocumentNode<TagsStatsQuery> = gql`
  query TagsStats($skip: Int!, $take: Int!) {
    tagsStats(order: [ {
       articleNumber: DESC
    }], skip: $skip, take: $take) {
        items {
            name
            color
            articleNumber
        }
        totalCount
    }
  }
`;
