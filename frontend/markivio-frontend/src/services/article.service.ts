import { apolloClient } from '@/config/apollo.config';
import type { Article, ArticleInformation } from '@/domain/article.models';
import type { OffsetPagination } from '@/domain/pagination.models';
import { AddArticles, GetArticles } from '@/graphql/article.queries';
import { catchError, EMPTY, from, map, mergeMap, Subject } from 'rxjs';

export function getMyArticles() {
  const subject = new Subject<{ skip: number; take: number }>();
  return {
    subject,
    observable: subject.pipe(
      mergeMap((params) =>
        apolloClient.query({
          query: GetArticles,
          variables: { skip: params.skip, take: params.take },
          fetchPolicy: 'network-only',
        }),
      ),
      map((response) => response.data),
      map((response) => {
        const items = response?.articles.items ?? [];
        const data = items.map(
          (article) =>
            ({
              id: article.id,
              source: article.source,
              title: article.title,
              description: article.description,
              tags: article.tags.map((tag) => ({
                color: tag.color,
                name: tag.name,
              })),
            }) as ArticleInformation,
        );
        const count = response?.articles.totalCount;
        const pageInfo = response?.articles.pageInfo;

        return {
          data,
          count,
          hasNextPage: pageInfo?.hasNextPage,
          hasPreviousPage: pageInfo?.hasPreviousPage,
        } as OffsetPagination<ArticleInformation>;
      }),
      catchError((err) => {
        console.error('Failed to fetch articles', err);
        return EMPTY;
      }),
    ),
  };
}

export function createArticle(article: Article) {
  const tags = article.tags.length > 0 ? article.tags.map((tag) => ({ id: tag.id })) : [];
  return from(
    apolloClient.mutate({
      mutation: AddArticles,
      variables: {
        input: {
          source: article.source,
          title: article.title,
          tags: tags,
          description: article.description,
        },
      },
    }),
  );
}
