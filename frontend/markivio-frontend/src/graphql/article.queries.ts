import { gql, type TypedDocumentNode } from '@urql/vue'

export type ArticleInformationQuery = {

};

export type GetArticlesInformationQuery = {
  articles: {
    __typename: 'Articles'
    items: Array<{
      id: string
      source: string
      title: string
      description?: string
      tags: Array<{
        id: string
        name: string
        color: string
      }>
    }>
    totalCount: number
    pageInfo: {
      hasNextPage: boolean
      hasPreviousPage: boolean
    }
  }
}

export type GetSourceUrlQuery = {
  articles: {
    items: Array<{
      id: string
      source: string
      isFramable: boolean
    }>
  }
}

export type AddArticleReturn = {
  article: {
    id: string
  }
}

const ArticleInformationFragment = `
fragment Article on ArticleInformation {
    id
    source
    title
    description
    tags {
        id
        name
        color
    }
}`;

const ArticlePaginationFragment = `
fragment Pagination on ArticlesCollectionSegment {
    totalCount
    pageInfo {
         hasNextPage
         hasPreviousPage
    }
}
`;

export const GetArticles: TypedDocumentNode<GetArticlesInformationQuery> = gql`
  ${ArticleInformationFragment}
  ${ArticlePaginationFragment}
  query Articles($offset: Int!, $limit: Int!) {
    articles(skip: $offset, take: $limit) {
      items {
        ...Article
      }
      ...Pagination
    }
  }
`

export const AddArticles: TypedDocumentNode<AddArticleReturn> = gql`
  mutation AddArticles($input: CreateArticleInput!) {
    createArticle(createArticle: $input) {
      id
    }
  }
`

export const UpdateArticle: TypedDocumentNode<AddArticleReturn> = gql`
  ${ArticleInformationFragment}
  mutation UpdateArticle($input: UpdateArticleInput!) {
    updateArticle(updateArticle: $input) {
      ...Article
    }
  }
`

export const GetUrlByArticleId: TypedDocumentNode<GetSourceUrlQuery> = gql`
  query Articles($id: UUID!) {
    articles(where: { id: { eq: $id } }, skip: 0, take: 1) {
      items {
        id
        source
        isFramable
      }
    }
  }
`

export const GetArticleById: TypedDocumentNode<GetArticlesInformationQuery> = gql`
  ${ArticleInformationFragment}
  ${ArticlePaginationFragment}
  query Articles($id: UUID!) {
    articles(where: {
      id: {
        eq: $id
      }
    }, skip: 0, take: 1) {
      items {
        ...Article
      }
      ...Pagination
    }
  }
`
