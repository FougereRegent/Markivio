import * as z from 'zod';

export interface ArticleInformation {
    Id: string,
    Title: string,
    Source: string,
    Tags: Array<{
        Name: string,
        Color: string,
    }>,
}

const Tag = z.object({
  name: z.string(),
  id: z.guid(),
  color: z.string(),
});

export const ArticleSchema = z.object({
  id: z.guid()
    .nullable(),
  title: z.string()
    .regex(/^[a-zA-Z0-9]+$/),
  source: z.httpUrl(),
  description: z.string(),
  tags: z.array(Tag)
});


export type Article = z.infer<typeof ArticleSchema>;
export type Tag = z.infer<typeof Tag>;

export function validateArticle(article: Article): z.ZodSafeParseResult<Article> {
  return ArticleSchema.safeParse(article)
}
