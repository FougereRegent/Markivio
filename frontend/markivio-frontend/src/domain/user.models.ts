import * as z from 'zod';

const nameRegex = /^[A-Za-zÀ-ÿà-ÿ\-'']+(?: [.'',A-Za-zÀ-ÿà-ÿ\-]+)*$/;

export const UserSchema = z.object({
  id: z.string().optional(),
  firstName: z.string().regex(nameRegex, "firstName doesn't respect format"),
  lastName: z.string().regex(nameRegex, "lastName doesn't respect format"),
  email: z.string().email(),
});

export type UserInformation = z.infer<typeof UserSchema>;

export function validateUser(user: UserInformation): z.ZodSafeParseResult<UserInformation> {
  return UserSchema.safeParse(user);
}
