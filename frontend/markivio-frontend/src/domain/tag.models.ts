import * as z from 'zod';


export const TagSchema = z.object({
  name: z.string(),
  id: z.guid().nullable(),
  color: z.string(),
});

export type Tag = z.infer<typeof TagSchema>;
