import { apolloClient } from "@/config/apollo.config";
import type { Article, ArticleInformation } from "@/domain/article.models";
import type { OffsetPagination } from "@/domain/pagination.models";
import { AddArticles, GetArticles } from "@/graphql/article.queries";
import { from, map, mergeMap, Subject } from "rxjs";

export function getMyArticles() {
  const sub = new Subject<{ skip: number, take: number }>();
  return {
    subject: sub, observable: sub.pipe(
      mergeMap(x => apolloClient.query({
        query: GetArticles,
        variables: { skip: x.skip, take: x.take }
      })),
      map(src => src.data),
      map(src => {
        const data = src?.articles.items.map(
          src => {
            return {
              Id: src.id,
              Source: src.source,
              Title: src.title,
              Tags: src.tags.map(tags => {
                return {
                  Color: tags.color,
                  Name: tags.name
                }
              })
            } as ArticleInformation
          }
        );
        const count = src?.articles.totalCount;
        const pageInfo = src?.articles.pageInfo;

        return {
          Data: data,
          Count: count,
          HasNextPage: pageInfo?.hasNextPage,
          HasPreviousPage: pageInfo?.hasPreviousPage
        } as OffsetPagination<ArticleInformation>
      })
    )
  };
}

export function createArticle(article: Article) {
  const tags = article.tags.length > 0 ? article.tags.map(pre => ({ id: pre.id })) : []
  debugger;
  return from(apolloClient.mutate({
    mutation: AddArticles,
    variables: {
      input: {
        source: article.source,
        title: article.title,
        tags: tags,
      }
    }
  }))
}
