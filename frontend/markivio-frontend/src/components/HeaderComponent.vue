<script setup lang="ts">
import LogoComponent from '@/components/LogoComponent.vue'
import 'remixicon/fonts/remixicon.css'
import UserIconComponent from '@/components/UserIconComponent.vue'
import UserMenuComponent from '@/components/UserMenuComponent.vue'
import LanguageSelector from '@/components/LanguageSelector.vue'
import { useAddEditDrawer } from '@/stores/add-edit-drawer-store'
import { ref } from 'vue'
import { useI18n } from 'vue-i18n'

const emit = defineEmits<{
  'toggle-sidebar': []
}>()

const popoverRef = ref()
const drawer = useAddEditDrawer()
const { t } = useI18n()

const clickIcon = () => {
  popoverRef.value.toggle(event)
}
</script>

<template>
  <div class="flex flex-row w-full items-center justify-between gap-1 md:gap-2">
    <div class="flex items-center gap-1">
      <div class="sm:hidden">
        <Button icon="ri-menu-line" severity="secondary" text @click="emit('toggle-sidebar')" />
      </div>
      <LogoComponent class="pb-2" />
    </div>
    <IconField class="flex-1 max-w-3xl 2xl:max-w-none 2xl:w-9/12 mx-1 md:mx-0">
      <InputIcon class="w-full">
        <i class="ri-search-line text-neutral-700"></i>
      </InputIcon>
      <AutoComplete size="small" :placeholder="t('header.search')" class="w-full" input-class="w-full" />
    </IconField>
    <div class="flex items-center gap-1 md:gap-2">
      <LanguageSelector />
      <Button icon="ri-add-line" size="small" class="shrink-0" @click="drawer.open(false)" />
      <div class="h-6 border-l border-neutral-300"></div>
      <UserIconComponent @click-icon="clickIcon" class="mr-1 md:mr-3" />
    </div>
    <Popover ref="popoverRef">
      <UserMenuComponent />
    </Popover>
  </div>
</template>
