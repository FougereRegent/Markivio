import * as z from 'zod'
import { TagSchema } from './tag.models'

export interface ArticleInformation {
  id: string
  title: string
  source: string
  description?: string
  tags: Array<{
    name: string
    color: string
  }>
}

export const ArticleSchema = z.object({
  id: z.guid().nullable(),
  title: z.string().min(1),
  source: z.httpUrl(),
  description: z.string().nullable(),
  tags: z.array(TagSchema),
})

export type Article = z.infer<typeof ArticleSchema>
