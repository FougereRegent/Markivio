<script setup lang="ts">
import LogoComponent from './LogoComponent.vue';
import { ref } from 'vue';
import 'remixicon/fonts/remixicon.css';

const data = ref(["Test1", "Test2", "Test3"]);
const search = () => {
    return [
        "Test1",
        "Test2",
        "Test3",
        "Test4",
    ]
};

</script>

<template>
    <div class="flex flex-row items-center w-screen">
        <LogoComponent />
        <div class="items-center">
            <IconField>
                <InputIcon>
                    <i class="ri-search-line text-neutral-700"></i>
                </InputIcon>
                <AutoComplete size="small" placeholder="Search ..." @complete="search" :suggestions="data"/>
            </IconField>
        </div>
        <div>
        </div>
    </div>
</template>