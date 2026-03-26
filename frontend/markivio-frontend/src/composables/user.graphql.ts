import type { UserInformation } from "@/domain/user.models";
import { GetMe, UpdateUser } from "@/graphql/user.queries";
import { useMutation, useQuery } from "@urql/vue";
import { computed, toValue, type Ref } from "vue";

export function useGetMyUser() {
  const { data, error, fetching } = useQuery({
    query: GetMe
  });

  const userInfo = computed(() => ({
    id: data.value?.me.id,
    email: data.value?.me.email ?? "",
    firstName: data.value?.me.firstName,
    lastName: data.value?.me.lastName
  } as UserInformation));

  return { userInfo, error, fetching };
}

export function useUpdateUserInformation(user: Ref<UserInformation>) {
  const { executeMutation, fetching, error } = useMutation(UpdateUser);

  const updateUser = () => {
    const userToUpdate: UserInformation = toValue(user);
    return executeMutation({
      firstName: userToUpdate.firstName,
      lastName: userToUpdate.lastName
    });
  };

  return { updateUser, fetching, error }
}
