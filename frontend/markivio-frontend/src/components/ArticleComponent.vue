<script lang="ts" setup>
import { Tag } from 'primevue';
import { ContrasteColor } from '@/helpers/ui.helpers';

export type ArticleProps = {
    Id: string,
    Title: string,
    Description?: string,
    Tags?: Array<{
        Label: string,
        Color: string,
    }>,
};

const props = defineProps<ArticleProps>();
</script>

<template>
  <div class="flex flex-row rounded-md px-4 py-5 border-gray-200 border bg-white hover:bg-gray-50 hover:border-gray-300">
        <div class="flex flex-col flex-4 w-10 h-full">
            <p class="flex-1 text-3xl mb-1 text-gray-900 font-semibold">{{ props.Title }}</p>
            <div class="h-8/12 w-11/12 text-s my-2 text-gray-600">
              <p class="line-clamp-3 text-justify">{{ props.Description }}</p>
            </div>
            <div class="flex flex-row flex-2 gap-3" >
                <template v-for="item of props.Tags" :key="item.Label">
                    <Tag :value="item.Label" :style="{backgroundColor: item.Color, color:
                    ContrasteColor(item.Color), opacity: 0.55}"/>
                </template>
            </div>
        </div>
        <div class="flex flex-row-reverse flex-1 justify-start pt-3 gap-0.5">
            <p class="text-s inline-block">17/11/2001</p>
        </div>
    </div>
</template>
