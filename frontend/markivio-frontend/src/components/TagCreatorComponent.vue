<script setup lang="ts">
import { InputText } from 'primevue';
import { ref, useTemplateRef } from 'vue';
const popoverRef = useTemplateRef("popover");
const colorPicker = ref("");

</script>
<template>
    <Button icon="ri-add-large-line" variant="text" class="text-neutral-700" @click="(event) => popoverRef?.toggle(event)"/> 
    <Popover ref="popover" class="flex flex-row">
        <div class="border-b border-b-neutral-300">
            <h3 class="text">Create Tag</h3>
        </div>
        <form class="m-1 flex flex-col gap-1">
            <div class="mt-3 flex flex-row">
                <div class="flex flex-col gap-1">
                    <label class="mb-1" for="tag-name">Tag Name</label>
                    <InputText id="tag-name" size="small"/>
                </div>
                <ColorPicker v-model="colorPicker" class="self-end ml-2 mb-1.5"/>
            </div>
            <Button label="Submit" />
        </form>
    </Popover>
</template>