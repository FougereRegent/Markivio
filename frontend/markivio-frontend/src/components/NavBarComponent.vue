<script setup lang="ts">
import { useI18n } from 'vue-i18n'
import { ref } from 'vue';
import { useVersionStore } from '@/stores/version-store';
import type { MenuPassThroughOptionType } from 'primevue';
import { pt } from 'zod/v4/locales';
import type { MenuItemCommandEvent } from 'primevue/menuitem';

const { t } = useI18n();
const { version } = useVersionStore();


const items = ref([
  {
    items: [
      {
        label: "Tous les documents",
        icon: "ri-article-line",
        badge: 10,
      },
      {
        label: "Favoris",
        icon: "ri-star-line",
        badge: 10,
      },
      {
        label: "Récent",
        icon: "ri-article-line",
      },
      {
        label: "A lire",
        icon: "ri-book-line",
        badge: 10,
      },
      {
        label: "Archives",
        icon: "ri-archive-line",
        badge: 10,
      },
    ]
  },
  {
    label: "Tags",
    icon: "ri-add-line",
    command: (evt: MenuItemCommandEvent) => { console.log("hello") },
    items: [
      {
        label: "React",
        color: "red",
        badge: 10
      },
      {
        label: "Vue JS",
        color: "blue",
        badge: 10
      },
      {
        label: "Design",
        color: "green",
        badge: 10
      },
      {
        label: "Javascript",
        color: "yellow",
        badge: 10
      },
      {
        label: "IA",
        color: "pink",
        badge: 10
      },
      {
        label: "Backend",
        color: "orange",
        badge: 10
      },
      {
        label: "Tous les tags",
        icon: "ri-bookmark-line",
      },
      {
        label: "Gérer les tags",
        icon: "ri-settings-5-line",
      }
    ]
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
])
</script>

<template>
  <Menu :model="items" class="md:w-60 h-full flex flex-col justify-between" :pt="{root: {style: {border: 'none',
                boxShadow:'none'}}}">
    <template #submenulabel="{ item }">
      <div class="flex flex-row justify-between w-full h-full">
        <span class="text-gray-400 font-semibold text-2xl">{{ item.label }}</span>
        <i class="text-gray-400 text-2xl ml-auto"
           :class="item.icon"
          @click="item.command"/>
      </div>
    </template>
    <template #item="{ item, props }">
      <a v-ripple class="flex items-center" v-bind="props.action">
        <div class="flex flex-row gap-4 justify-start items-center w-full">
          <span class="rounded-full w-4 h-4" v-if="item.color != undefined"
            :style="{ backgroundColor: item.color }"></span>
          <i class="text-gray-400 text-2xl" :class="item.icon" v-if="item.icon" />
          <span class="text-xl">{{ item.label }}</span>
          <span class="ml-auto">{{ item.badge }}</span>
        </div>
      </a>
    </template>
    <template #end>
      <p class="text-gray-300 font-light italic px-2">Version : {{version}}</p>
    </template>
  </Menu>
</template>
