<script lang="ts" setup>
import { reactive, ref } from 'vue';
import { Tag } from 'primevue';
import { ConstrasteColor as ContrastColor } from '@/helpers/ui.helpers';
const title = ref("Test");
const description = ref("Description");

const color: string = "#03FF3F";

const chipStyle = reactive([{
    backgroundColor: color,
    color: ContrastColor(color)
}])

</script>

<template>
    <div class="flex flex-row am rounded-md p-2 bg-white">
        <div class="flex flex-col flex-4">
            <p class="flex-1 text-2xl mb-1">{{ title }}</p>
            <p class="flex-1 text-xs">{{ description }}</p>
            <div class="flex flex-row flex-3 gap-2">
                <Tag value="Info" rounded :style="chipStyle"/>
                <Tag value="Info" rounded :style="chipStyle"/>
                <Tag value="Info" rounded :style="chipStyle"/>
                <Tag value="Info" rounded :style="chipStyle"/>
                <Tag value="Info" rounded :style="chipStyle"/>
            </div>
        </div>
        <div class="flex flex-row-reverse flex-1 justify-start pt-3 gap-0.5">
            <p class="text-xs inline-block">17/11/2001</p>
        </div>
    </div>
</template>