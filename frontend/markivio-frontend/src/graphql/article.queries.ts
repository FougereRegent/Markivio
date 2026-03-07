import { gql, type TypedDocumentNode } from "@apollo/client";

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

export type AddArticleReturn = {
  article: {
    id: string,
  },
};


export const GetArticles : TypedDocumentNode<GetArticlesInformationQuery> = gql`
query Articles($skip: Int!, $take: Int!) {
    articles(skip: $skip, take: $take) {
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
