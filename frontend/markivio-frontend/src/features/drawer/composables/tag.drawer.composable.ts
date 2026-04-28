import type { Tag } from "@/features/tag/models/tag.models";
import { useDebounce } from "@vueuse/core";
import { ref, watch, type Ref } from "vue";
import { useGetAllTags } from "@/features/tag/composables/tag.graphql";
import { CONST } from "@/config/constante.config";

export function useTagAutocomplete(selectedTags: Ref<Tag[]>) {
  const tagName = ref<string|null>("");

  const offset = ref(0)
  const refSuggestion = ref<Tag[]>([])

  const debounceTagName = useDebounce(tagName, CONST.debounceTime.inputTime)

  const { tags, executeQuery } = useGetAllTags(debounceTagName)

  watch(tags, (newData) => {
    refSuggestion.value =
      newData?.filter(tag =>
        !selectedTags.value.find(t => t.id === tag.id)
      ) ?? []
  })

  function search(query?: string) {
    offset.value = 0
    tagName.value = query ?? ''

    if (!query) {
      executeQuery({ requestPolicy: 'network-only' })
    }
  }

  return {
    tagName,
    refSuggestion,
    search,
  }
}
