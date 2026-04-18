import { defineStore } from 'pinia'
import { ref } from 'vue'

export enum ActionDrawer {
  Create,
  Edit,
}

export const useAddEditDrawer = defineStore('add-edit-drawer', () => {
  const drawerType = ref(ActionDrawer.Create)
  const drawerState = ref(false)
  const drawerTitle = ref('')

  function open(isEdit: boolean = false) {
    drawerTitle.value = isEdit ? 'Edit' : 'Create'
    drawerState.value = true
  }

  function close() {
    drawerState.value = false
  }

  return {
    open,
    close,
    drawerState,
    drawerType,
    drawerTitle,
  }
})
