import { defineStore } from 'pinia'
import { ref } from 'vue'

export enum ActionDrawer {
  Create,
  Edit,
}

export const useAddEditDrawer = defineStore('add-edit-drawer', () => {
  const drawerType = ref(ActionDrawer.Create)
  const drawerState = ref(false)
  const drawerArticleId = ref<string | null>(null);

  function open(isEdit: boolean = false, articleId: string | null = null) {
    drawerType.value = isEdit ? ActionDrawer.Edit : ActionDrawer.Create;
    drawerState.value = true
    drawerArticleId.value = articleId;
  }

  function close() {
    drawerState.value = false
    drawerArticleId.value = null;
    drawerType.value = ActionDrawer.Create;
  }

  return {
    open,
    close,
    drawerState,
    drawerType,
    drawerArticleId,
  }
})
