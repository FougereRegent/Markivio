<script setup lang="ts">
import { useI18n } from 'vue-i18n'
import { computed, ref } from 'vue';
import { useVersionStore } from '@/stores/version-store';
import type { MenuItemCommandEvent } from 'primevue/menuitem';
import { useGetTenMostUsedTags } from '@/features/tag/composables/tag.graphql';
import { useArticleStore } from '@/stores/article-store';

const { t } = useI18n();
const { version } = useVersionStore();

const { tags } = useGetTenMostUsedTags();
const selected = ref("Tous les articles");
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
        label: "Tous les articles",
        icon: "ri-article-line",
        badge: 10,
        command: () => changeTypeFilter()
      },
      {
        label: "Favoris",
        icon: "ri-star-line",
        badge: 10,
        command: () => changeTypeFilter()
      },
      {
        label: "A lire",
        icon: "ri-book-line",
        badge: 10,
        command: () => changeTypeFilter()
      },
      {
        label: "Archives",
        icon: "ri-archive-line",
        badge: 10,
        command: () => changeTypeFilter()
      },
    ]
  },
  {
    label: "Tags",
    icon: "ri-add-line",
    command: (evt: MenuItemCommandEvent) => { console.log("hello") },
    items: tags.value?.map(pre => ({
      label: pre.name,
      color: pre.color,
      badge: pre.articleNumber,
      command: () => changeTagNameFilter(pre.name)
    } as tagsElementBarMenu)).concat([{
      label: "Tous les tags",
      icon: "ri-bookmark-line",
    }, {
      label: "Gérer les tags",
      icon: "ri-settings-5-line"
    }] as Array<tagsElementBarMenu>)
  },
  {
    separator: true,
  },
  {
    items: [{
      label: "Corbeille",
      icon: "ri-delete-bin-line"
    }]
  }
]);

</script>

<template>
  <Menu :model="items" class="md:w-60 h-full flex flex-col justify-between" :pt="{
    root: {
      style: {
        border: 'none',
        boxShadow: 'none'
      }
    },
  }">
    <template #submenulabel="{ item }">
      <div class="flex flex-row justify-between w-full h-full">
        <span class="text-gray-400 font-semibold text-xl">{{ item.label }}</span>
        <i class="text-gray-400 text-xl ml-auto hover:opacity-70" :class="item.icon" @click="item.command" />
      </div>
    </template>
    <template #item="{ item, props }">
      <a v-ripple class="flex items-center" v-bind="props.action">
        <div class="flex flex-row gap-4 justify-start items-center w-full">
          <span class="rounded-full w-4 h-4" v-if="item.color != undefined"
            :style="{ backgroundColor: item.color }"></span>
          <i class="text-gray-400 text-xl" :class="item.icon" v-if="item.icon" />
          <span v-tooltip.right="{ value: item.label, showDelay: 650 }" class="text-l truncate w-35
              font-semibold">{{ item.label }}</span>
          <span class="ml-auto">{{ item.badge }}</span>
        </div>
      </a>
    </template>
    <template #end>
      <p class="text-gray-400 font-light italic px-2 text-xs">Version : {{ version }}</p>
    </template>
  </Menu>
</template>
