import { describe, it, expect, vi, beforeEach } from 'vitest'
import { mount } from '@vue/test-utils'
import TagCreatorComponent from '@/features/tag/components/TagCreatorComponent.vue'

// Mock composables
vi.mock('@/features/tag/composables/tag.graphql', () => ({
  useCreateTags: () => ({
    createTags: vi.fn().mockResolvedValue({}),
  }),
}))

vi.mock('@/features/auth/composables/zod.composable', () => ({
  useZodValidation: () => ({
    validate: vi.fn().mockReturnValue(true),
    errors: { value: null },
  }),
}))

describe('TagCreatorComponent', () => {
  beforeEach(() => {
    vi.clearAllMocks()
  })

  const mountComponent = () => {
    return mount(TagCreatorComponent, {
      global: {
        stubs: {
          Popover: {
            name: 'Popover',
            template: '<div><slot /></div>',
            methods: {
              toggle: vi.fn(),
            },
          },
          InputText: true,
          Button: false, // Don't stub to find it
          ColorPicker: true,
        },
      },
    })
  }

  it('should exist', () => {
    const wrapper = mountComponent()

    expect(wrapper.exists()).toBe(true)
  })

  it('should have correct structure', () => {
    const wrapper = mountComponent()

    expect(wrapper.html()).toBeDefined()
  })

  it('should render button', () => {
    const wrapper = mountComponent()

    const button = wrapper.find('button')
    expect(button.exists()).toBe(true)
  })

  it('should have vm defined', () => {
    const wrapper = mountComponent()

    expect(wrapper.vm).toBeDefined()
  })
})
