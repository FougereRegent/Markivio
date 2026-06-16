<script setup lang="ts">
import LogoComponent from '@/components/LogoComponent.vue'
import 'remixicon/fonts/remixicon.css'
import UserIconComponent from '@/components/UserIconComponent.vue'
import UserMenuComponent from '@/components/UserMenuComponent.vue'
import LanguageSelector from '@/components/LanguageSelector.vue'
import { useAddEditDrawer } from '@/stores/add-edit-drawer-store'
import { ref } from 'vue'
import { useI18n } from 'vue-i18n'

const popoverRef = ref()
const drawer = useAddEditDrawer()
const { t } = useI18n()

const clickIcon = () => {
  popoverRef.value.toggle(event)
}
</script>

<template>
  <div class="flex flex-row w-full items-center justify-between">
    <LogoComponent class="pb-2" />
    <IconField class="2xl:w-9/12">
      <InputIcon class="w-full">
        <i class="ri-search-line text-neutral-700"></i>
      </InputIcon>
      <AutoComplete size="small" :placeholder="t('header.search')" class="w-full" input-class="w-full" />
    </IconField>
    <div class="flex items-center gap-3">
      <LanguageSelector />
      <Button :label="t('header.addLink')" @click="drawer.open(false)" />
      <div class="h-7 border border-neutral-300"></div>
      <UserIconComponent @click-icon="clickIcon" class="mr-4" />
    </div>
    <Popover ref="popoverRef">
      <UserMenuComponent />
    </Popover>
  </div>
</template>
