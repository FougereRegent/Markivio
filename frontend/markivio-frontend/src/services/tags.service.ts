import { apolloClient } from "@/config/apollo.config";
import type { Tag } from "@/domain/article.models";
import type { OffsetPagination } from "@/domain/pagination.models";
import { GetAllTAgs } from "@/graphql/tags.queries";
import { map, mergeMap, Subject } from "rxjs";

export function getTags() {
  const sub = new Subject<{skip: number, take: number}>();
  return {
    subject: sub,
    observable: sub.pipe(
      mergeMap(x => apolloClient.query({
        query: GetAllTAgs,
        variables: { skip: x.skip, take: x.take }
      })),
      map(src => src.data),
      map(src => {
        const data = src?.tags.items.map(
          src => ({
            id: src.id,
            color: src.color,
            name: src.name
          } as Tag)
        );

        const count = src?.tags.totalCount;
        const pageInfo = src?.tags.pageInfo;

        return {
          Data: data,
          Count: count,
          HasNextPage: pageInfo?.hasNextPage,
          HasPreviousPage: pageInfo?.hasPreviousPage,
        } as OffsetPagination<Tag>;
      })
    ),
  };
};

