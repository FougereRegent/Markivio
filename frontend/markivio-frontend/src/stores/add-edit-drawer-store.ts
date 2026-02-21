import { defineStore } from "pinia"
import { computed, ref } from "vue";

export enum ActionDrawer {
  Create,
  Edit,
}

export const useAddEditDrawer = defineStore('add-edit-drawer', () => {
  const drawer = ref(ActionDrawer.Create);
  const drawerState = ref(false);

  const drawerType = computed(() => drawer);

  function Open() {
    drawerState.value = true;
  }

  function Close() {
    drawerState.value = false;
  }

  return {
    Open,
    Close,
    drawerState,
    drawerType,
  }
})
