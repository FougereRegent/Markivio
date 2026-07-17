<script setup lang="ts">
import UserIconComponent from '@/components/UserIconComponent.vue'
import UserMenuComponent from '@/components/UserMenuComponent.vue'
import { useI18n } from 'vue-i18n'
import { computed, ref } from 'vue';
import { useVersionStore } from '@/stores/version-store';
import { useGetTenMostUsedTags } from '@/features/tag/composables/tag.graphql';
import { useArticleStore } from '@/stores/article-store';

const emit = defineEmits<{
  'close-sidebar': []
}>()

const popoverRef = ref()
const { t } = useI18n();
const { version } = useVersionStore();

const clickUserIcon = () => {
  popoverRef.value.toggle(event)
}

const { tags } = useGetTenMostUsedTags();
const { changeTagNameFilter, changeTypeFilter } = useArticleStore();

type tagsElementBarMenu = {
  label: string,
  color: string,
  icon?: string,
  badge?: number,
  command?: () => void
}

const items = computed(() => [
  {
    items: [
      {
        label: t("nav.allArticles"),
        icon: "ri-article-line",
        badge: 10,
        command: () => changeTypeFilter()
      },
      {
        label: t("nav.favorites"),
        icon: "ri-star-line",
        badge: 10,
        command: () => changeTypeFilter()
      },
      {
        label: t("nav.toRead"),
        icon: "ri-book-line",
        badge: 10,
        command: () => changeTypeFilter()
      },
      {
        label: t("nav.archived"),
        icon: "ri-archive-line",
        badge: 10,
        command: () => changeTypeFilter()
      },
    ]
  },
  {
    label: t("nav.tags"),
    icon: "ri-add-line",
    command: () => { console.log("hello") },
    items: tags.value?.map(pre => ({
      label: pre.name,
      color: pre.color,
      badge: pre.articleNumber,
      command: () => changeTagNameFilter(pre.name)
    } as tagsElementBarMenu)).concat([{
      label: t("nav.allTags"),
      icon: "ri-bookmark-line",
    }, {
      label: t("nav.handleTags"),
      icon: "ri-settings-5-line"
    }] as Array<tagsElementBarMenu>)
  },
  {
    separator: true,
  },
  {
    items: [{
      label: t("nav.bin"),
      icon: "ri-delete-bin-line"
    }]
  }
]);

</script>

<template>
  <Menu :model="items" class="w-full h-full flex flex-col justify-between" :pt="{
    root: {
      style: {
        border: 'none',
        boxShadow: 'none',
        padding: 0,
        minWidth: 0
      }
    },
    list: {
      style: {
        padding: '0.25rem 0',
        margin: 0,
        gap: '2px'
      }
    },
  }">
    <template #submenulabel="{ item }">
      <div class="flex flex-row justify-between w-full h-full px-4 py-2">
        <span class="text-gray-400 font-semibold text-xl">{{ item.label }}</span>
        <i class="text-gray-400 text-xl ml-auto hover:opacity-70" :class="item.icon" @click="(item.command as any)?.()" />
      </div>
    </template>
    <template #item="{ item, props }">
      <a v-ripple class="flex items-center px-4 py-2.5" v-bind="props.action">
        <div class="flex flex-row gap-3 justify-start items-center w-full">
          <span class="rounded-full w-4 h-4 shrink-0" v-if="item.color != undefined"
            :style="{ backgroundColor: item.color }"></span>
          <i class="text-neutral-500 text-xl" :class="item.icon" v-if="item.icon" />
          <span v-tooltip.right="{ value: item.label, showDelay: 650 }" class="text-sm sm:text-base truncate w-35
              font-semibold text-neutral-700">{{ item.label }}</span>
          <span class="ml-auto text-sm font-semibold text-neutral-600 bg-neutral-100 rounded-full px-2.5 py-0.5 min-w-7 text-center">{{ item.badge }}</span>
        </div>
      </a>
    </template>
    <template #end>
      <div class="border-t border-neutral-200">
        <div class="flex items-center gap-3 cursor-pointer hover:bg-neutral-50 transition-colors px-4 py-3" @click="clickUserIcon">
          <UserIconComponent />
          <div class="flex-1 min-w-0 hidden sm:block">
            <p class="text-sm font-medium text-neutral-800 truncate">{{ t('userMenu.editProfile') }}</p>
            <p class="text-xs text-neutral-400">{{ t('userMenu.logout') }}</p>
          </div>
          <i class="ri-more-2-line text-neutral-400 hidden sm:block"></i>
        </div>
        <p class="text-gray-400 font-light italic px-4 text-xs pb-3">Version : {{ version }}</p>
      </div>
    </template>
  </Menu>
  <Popover ref="popoverRef">
    <UserMenuComponent />
  </Popover>
</template>
