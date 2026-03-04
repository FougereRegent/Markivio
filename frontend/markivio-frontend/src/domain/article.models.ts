import * as z from 'zod';

export interface ArticleInformation {
  id: string;
  title: string;
  source: string;
  description?: string;
  tags: Array<{
    name: string;
    color: string;
  }>;
}

export const TagSchema = z.object({
  name: z.string(),
  id: z.guid(),
  color: z.string(),
});

export const ArticleSchema = z.object({
  id: z.guid().nullable(),
  title: z.string().min(1),
  source: z.httpUrl(),
  description: z.string(),
  tags: z.array(TagSchema),
});

export type Article = z.infer<typeof ArticleSchema>;
export type Tag = z.infer<typeof TagSchema>;

export function validateArticle(article: Article): z.ZodSafeParseResult<Article> {
  return ArticleSchema.safeParse(article);
}
