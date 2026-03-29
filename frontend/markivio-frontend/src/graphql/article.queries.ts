import { gql, type TypedDocumentNode } from "@urql/vue";

export type GetArticlesInformationQuery = {
    articles: {
        __typename:"Articles"
        items: Array<{
            id: string
            source: string
            title: string
            description?: string,
            tags: Array<{
                name: string,
                color: string
            }>
        }>,
        totalCount: number,
        pageInfo: {
            hasNextPage: boolean
            hasPreviousPage: boolean
        }
    }
};

export type GetSourceUrlQuery = {
  articles: {
    items: Array<{
      id: string,
      source: string,
    }>
  }
};

export type AddArticleReturn = {
  article: {
    id: string,
  },
};


export const GetArticles : TypedDocumentNode<GetArticlesInformationQuery> = gql`
query Articles($offset: Int!, $limit: Int!) {
    articles(skip: $offset, take: $limit) {
        items{
            id
            source
            title
            description
            tags {
                name
                color
            }
        }
        totalCount
        pageInfo {
            hasNextPage
            hasPreviousPage
        }
    }
}
`;

export const AddArticles : TypedDocumentNode<AddArticleReturn> = gql`
mutation AddArticles($input: CreateArticleInput!) {
  createArticle(createArticle: $input) {
    id
  }
}
`;

export const GetUrlByArticleId: TypedDocumentNode<GetSourceUrlQuery> = gql`
query Articles($id: UUID!) {
    articles(where:  {
       id:  {
          eq: $id
       }
    }, skip: 0, take: 1) {
        items {
            id
            source
        }
    }
}
`;
