<template>
    <div class="size-12 rounded-full bg-blue-500 p-1 cursor-pointer" @click="$emit('clickIcon')">
        <img :src="defaultUser" alt="User icon"/>
    </div>
</template>

<script setup lang="ts">
    import { ref } from 'vue';
    const defaultUser = ref("/src/assets/default-user.svg");
</script>