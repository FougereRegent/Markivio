<template>
    <div>
        <i :class="iconName" class="size-10" />
        <span>{{ name }}</span>
    </div>
</template>

<script setup lang="ts">
    const {name, iconName} = defineProps<{
        name?: string,
        iconName?: string
    }>();
</script>