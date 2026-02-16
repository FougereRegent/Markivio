import { apolloClient } from "@/config/apollo.config";
import type { ArticleInformation } from "@/domain/article.models";
import type { OffsetPagination } from "@/domain/pagination.models";
import { GetArticles } from "@/graphql/article.queries";
import { map, mergeMap, Subject } from "rxjs";

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
