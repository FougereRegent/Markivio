<template>
  <div class="flex flex-col justify-between h-11/24 px-3">
    <div class="flex flex-col py-2 mt-4">
      <NavElementComponent name="All Articles" icon-class="ri-list-view" />
      <NavElementComponent name="Favorites" icon-class="ri-star-line" />
      <NavElementComponent name="Tags" icon-class="ri-bookmark-line" />
    </div>
    <div>
      <h2 class="text-neutral-900 font-semibold text-2xl">Tags</h2>
      <NavElementComponent name="All Articles" tag-color="red" />
      <NavElementComponent name="Favorites" tag-color="red" />
      <NavElementComponent name="Tags" tag-color="red" />
    </div>
    <div>
      <h2 class="text-neutral-900 font-semibold text-2xl">Folder</h2>
      <NavElementComponent name="All Articles" icon-class="ri-folder-line" />
      <NavElementComponent name="Favorites" icon-class="ri-folder-line" />
      <NavElementComponent name="Tags" icon-class="ri-folder-line" />
    </div>
  </div>
</template>

<script setup lang="ts">
</script>
