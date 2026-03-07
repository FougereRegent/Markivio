import { apolloClient } from '@/config/apollo.config';
import type { OffsetPagination } from '@/domain/pagination.models';
import type { Tag } from '@/domain/tag.models';
import { ErrType, mapApolloError, type Err } from '@/errors/errors';
import { AddTags, GetAllTags } from '@/graphql/tags.queries';
import { catchError, EMPTY, from, map, mergeMap, of, Subject } from 'rxjs';
import { Result } from 'typescript-result';

export function getTags() {
  const subject = new Subject<{ skip: number; take: number; tagName?: string }>();
  return {
    subject,
    observable: subject.pipe(
      mergeMap((params) =>
        apolloClient.query({
          query: GetAllTags,
          variables: { skip: params.skip, take: params.take, tagName: params.tagName },
        }),
      ),
      map((response) => response.data),
      map((response) => {
        const items = response?.tags.items ?? [];
        const data = items.map(
          (tag) =>
            ({
              id: tag.id,
              color: tag.color,
              name: tag.name,
            }) as Tag,
        );

        const count = response?.tags.totalCount;
        const pageInfo = response?.tags.pageInfo;

        return {
          data,
          count,
          hasNextPage: pageInfo?.hasNextPage,
          hasPreviousPage: pageInfo?.hasPreviousPage,
        } as OffsetPagination<Tag>;
      }),
      catchError((err) => {
        console.error('Failed to fetch tags', err);
        return EMPTY;
      }),
    ),
  };
}

export function CreateTag(tag: Tag) {
  return from(apolloClient.mutate({
    mutation: AddTags,
    variables: {
      input: [{
        name: tag.name,
        color: tag.color
      }]
    },
  })).pipe(
    map(response => {
      if(response.error)
        return Result.error<Err[]>([{message: "", type: ErrType.unknown}]);
      return Result.ok(response.data);
    }),
    catchError((err) => of(Result.error<Err[]>(mapApolloError(err))))
  )
}
