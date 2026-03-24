import type { Tag } from "@/domain/tag.models";
import { AddTags, GetAllTags } from "@/graphql/tags.queries";
import { useMutation, useQuery } from "@urql/vue";
import { computed, type Ref } from "vue";

export function useGetAllTags(limit: number, offset: Ref<number>, tagName: Ref<string>) {
  const { data, fetching, error, executeQuery} = useQuery({
    query: GetAllTags,
    variables: computed(() => ({
      skip: offset.value,
      take: limit,
      tagName: tagName.value
    }))
  });

  const tags = computed(() => data.value?.tags.items.map(pre => ({
    id: pre.id,
    color: pre.color,
    name: pre.name
  } as Tag)))

  return { tags, fetching, error, executeQuery}
}


export function useCreateTags(input: Ref<Tag> | Ref<Tag[]>) {
  const { executeMutation, error, fetching, data } = useMutation(AddTags);

  const tags = computed(() => data.value?.tags.items.map(pre => ({
    id: pre.id,
    name: pre.name,
    color: pre.color
  } as Tag)))

  const createTags = () => {
    const tags:Tag[] = [];

    if(Array.isArray(input.value)) {
      tags.push(...input.value);
    } else {
      tags.push(input.value);
    }

    return executeMutation({
      input: tags.map(pre => ({color: pre.color, name: pre.name}))
    })
  };

  return { tags, error, fetching, createTags }
}
