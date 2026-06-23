<script setup lang="ts">
import { useI18n } from 'vue-i18n'
import { ref } from 'vue';
import { useVersionStore } from '@/stores/version-store';

type itemStr = {
  icon: string
  label: string
}

type tagStr = {
  label: string
  color: string
};

const { t } = useI18n();
const {version} = useVersionStore();


const items = ref([
  {
    separator: false
  },
  {
    items: [
      {
        label: "Tous les documents",
        icon: "pi pi-align-justify",
        badge: 10,
      },
      {
        label: "Favoris",
        icon: "pi pi-start",
        badge: 10,
      },
      {
        label: "A lire",
        icon: "pi pi-book",
        badge: 10,
      },
    ]
  },
  {
    label: "Tags",
    icon: "pi pi-book",
    items: [
      {
        label: "React",
        color: "red",
        bagde: 10
      },
      {
        label: "Vue JS",
        color: "blue",
        bagde: 10
      },
      {
        label: "Design",
        color: "green",
        bagde: 10
      },
      {
        label: "Javascript",
        color: "yellow",
        bagde: 10
      },
      {
        label: "IA",
        color: "pink",
        bagde: 10
      },
      {
        label: "Backend",
        color: "orange",
        bagde: 10
      },
    ]
  }
])
</script>

<template>
  <Menu :model="items" class="w-full md:w-60">
    <template #submenulabel="{ item }">
      <span class="text-primary font-bold" :class="item.icon"></span>
      <span class="text-primary font-bold">{{ item.label }}</span>
    </template>
    <template #item="{ item, props }">
      <a v-ripple class="flex items-center" v-bind="props.action">
        <div class="grid grid-flow-col grid-rows-1 gap-4 justify-center">
          <span class="col-span-1 rounded-full w-4 h-4" v-if="item.color != undefined"
          :style="{backgroundColor: item.color}"></span>
          <span :class="item.icon" />
          <span class="col-span-3">{{ item.label }}</span>
          <span class="col-span-1">{{ item.badge }}</span>
        </div>
      </a>
    </template>
    <template #end>
      <span class="inline-flex flex-col items-start">{{version}}</span>
    </template>
  </Menu>
</template>
