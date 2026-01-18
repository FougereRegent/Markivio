<script lang="ts" setup>
import { Tag } from 'primevue';
import { ConstrasteColor as ContrastColor } from '@/helpers/ui.helpers';

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
    <div class="flex flex-row am rounded-md p-2 bg-white">
        <div class="flex flex-col flex-4">
            <p class="flex-1 text-2xl mb-1">{{ props.Title }}</p>
            <p class="flex-1 text-xs">{{ props.Description }}</p>
            <div class="flex flex-row flex-3 gap-2" >
                <template v-for="item of props.Tags" :key="item.Label">
                    <Tag :value="item.Label" rounded :style="{backgroundColor: item.Color, color: ContrastColor(item.Color)}"/>
                </template>
            </div>
        </div>
        <div class="flex flex-row-reverse flex-1 justify-start pt-3 gap-0.5">
            <p class="text-xs inline-block">17/11/2001</p>
        </div>
    </div>
</template>