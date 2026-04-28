import { ArticleSchema, type Article } from "@/domain/article.models";
import type { Tag } from "@/domain/tag.models";
import { computed, ref, watch, type Ref } from "vue";
import { useZodValidation } from "../zod.composable";
import { useCreateArticle, useGetArticleById, useUpdateArticle } from "../article.graphql";
import { ActionDrawer, useAddEditDrawer } from "@/stores/add-edit-drawer-store";

export function useArticleForm() {
  const drawer = useAddEditDrawer();
  const article = ref<Article>({
    id: null,
    title: '',
    source: '',
    description: '',
    tags: [],
  });

  const tagName = ref<string | null>('');

  function reset() {
    article.value = {
      id: null,
      title: '',
      source: '',
      description: '',
      tags: [],
    }
    tagName.value = ''
  }

  function addTag(tag: Tag) {
    if (!article.value.tags.find(t => t.id === tag.id)) {
      article.value.tags.push(tag);
    }
  }

  function removeTag(tag: Tag) {
    article.value.tags = article.value.tags.filter(t => t.id !== tag.id)
  }

  watch(() => drawer.drawerState, (open) => {
    if(!open) {
      reset();
    }
  });

  return {
    article,
    tagName,
    addTag,
    removeTag,
  }
}

export function useArticleSubmit(article: Ref<Article>) {
  const { validate, errors } = useZodValidation(ArticleSchema, article);
  const { createArticle, fetching } = useCreateArticle(article);
  const { updateArticle } = useUpdateArticle();
  const drawer = useAddEditDrawer()
  const hasError = computed(() => ({
    title: {
      hasError: errors.value?.title != undefined,
      message: errors.value?.title?.[0]?.message
    },
    source: {
      hasError: errors.value?.source != undefined,
      message: errors.value?.source?.[0]?.message
    }
  }))

  async function submit() {
    if (!validate()) return

    if (drawer.drawerType === ActionDrawer.Create) {
      await createArticle()
    } else {
      await updateArticle(article)
    }

    drawer.close()
  }

  return {
    submit,
    fetching,
    hasError
  }
}

export function useArticleDrawer(article: Ref<Article>) {
  const drawer = useAddEditDrawer()

  const { article: art, executeQuery: fetchArticle } =
    useGetArticleById(
      computed(() => drawer.drawerArticleId),
      {
        pause: computed(() =>
          drawer.drawerArticleId == null ||
          drawer.drawerType === ActionDrawer.Edit
        )
      }
    )

  watch(() => drawer.drawerState, (open) => {
    if (!open) return

    // reset géré ailleurs idéalement
    if (drawer.drawerType === ActionDrawer.Edit) {
      fetchArticle({ requestPolicy: 'network-only' })
    }
  })

  watch(art, (newArticle) => {
    if (newArticle) {
      article.value = newArticle
    }
  })

  return {
    drawer,
  }
}
