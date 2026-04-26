import { defineStore } from 'pinia'
import { computed, ref } from 'vue'

export enum ActionDrawer {
  Create,
  Edit,
}

export const useAddEditDrawer = defineStore('add-edit-drawer', () => {
  const drawerType = ref(ActionDrawer.Create)
  const drawerState = ref(false)
  const drawerTitle = ref('Create')
  const drawerArticleId = ref<string | null>(null);

  function open(isEdit: boolean = false, articleId: string | null = null) {
    if (isEdit) {
      drawerTitle.value = "Edit";
      drawerType.value = ActionDrawer.Edit;
    } else {
      drawerTitle.value = "Create";
      drawerType.value = ActionDrawer.Create;
    }
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
    drawerTitle,
    drawerArticleId,
  }
})
