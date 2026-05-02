<script setup lang="ts">
import { useCreateTags } from '@/features/tag/composables/tag.graphql'
import { useZodValidation } from '@/features/auth/composables/zod.composable'
import { type Tag, TagSchema } from '@/features/tag/models/tag.models'
import { InputText } from 'primevue'
import { computed, ref, useTemplateRef } from 'vue'

const popoverRef = useTemplateRef('popover')
const emptyTag = () => ({
  id: null,
  name: '',
  color: '#ff00f0',
})
const tag = ref<Tag>(emptyTag())

const tagColor = computed({
  get: () => tag.value.color.replace('#', ''),
  set: (value: string) => {
    tag.value.color = `#${value}`
  },
})

const { createTags } = useCreateTags(tag)
const { validate } = useZodValidation(TagSchema, tag)

async function submit() {
  if (validate()) {
    await createTags()
    tag.value = emptyTag()
  }
}

const onClick = (event: PointerEvent) => {
  popoverRef.value?.toggle(event)
  tag.value = emptyTag()
}
</script>
<template>
  <Button icon="ri-add-large-line" variant="text" class="text-neutral-700" @click="onClick" />
  <Popover ref="popover" class="flex flex-row">
    <div class="border-b border-b-neutral-300">
      <h3 class="text">Create Tag</h3>
    </div>
    <form class="m-1 flex flex-col gap-1">
      <div class="mt-3 flex flex-row">
        <div class="flex flex-col gap-1">
          <label class="mb-1" for="tag-name">Tag Name</label>
          <InputText id="tag-name" size="small" v-model="tag.name" />
        </div>
        <ColorPicker
          v-model="tagColor"
          inputId="cp-hex"
          format="hex"
          class="self-end ml-2 mb-1.5"
        />
      </div>
      <Button label="Create" class="mt-2" @click="submit" />
    </form>
  </Popover>
</template>
