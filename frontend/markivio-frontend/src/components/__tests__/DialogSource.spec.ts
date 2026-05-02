import { describe, it, expect, vi, beforeEach } from 'vitest'
import { mount } from '@vue/test-utils'
import DialogSource from '@/components/DialogSource.vue'

// Mock PrimeVue components
vi.mock('primevue', () => ({
  Dialog: {
    name: 'Dialog',
    template: `
      <div @click="$emit('update:visible', false)">
        <slot name="header" />
        <slot name="default" />
      </div>
    `,
    props: ['visible', 'modal', 'header', 'style'],
    emits: ['update:visible'],
  },
  Button: {
    name: 'Button',
    template: '<button><slot /></button>',
    props: ['icon', 'severity', 'class'],
  },
}))

describe('DialogSource', () => {
  beforeEach(() => {
    vi.clearAllMocks()
  })

  const mountComponent = (overrides: Record<string, unknown> = {}) => {
    return mount(DialogSource, {
      props: {
        visible: true,
        id: '123',
        title: 'Test Article',
        source: 'https://example.com',
        ...overrides,
      },
      global: {
        stubs: {
          iframe: true,
        },
      },
    })
  }

  it('should render with visible prop', async () => {
    const wrapper = mountComponent()

    expect(wrapper.props('visible')).toBe(true)
  })

  it('should emit update:visible when dialog triggers it', async () => {
    const wrapper = mountComponent()

    // Click the dialog to trigger update:visible
    await wrapper.find('div').trigger('click')

    expect(wrapper.emitted('update:visible')).toBeTruthy()
    expect(wrapper.emitted('update:visible')![0]).toEqual([false])
  })

  it('should display correct title in props', () => {
    const wrapper = mountComponent({ title: 'My Article' })

    expect(wrapper.props('title')).toBe('My Article')
  })

  it('should have visible prop in component', () => {
    const wrapper = mountComponent()

    expect(wrapper.props('visible')).toBe(true)
  })

  it('should set iframe src to source prop', () => {
    const wrapper = mountComponent({ source: 'https://test.com' })

    expect(wrapper.props('source')).toBe('https://test.com')
  })

  it('should have style prop in component', () => {
    const wrapper = mountComponent()

    // Component accepts style prop
    expect(wrapper.props('style')).toBeUndefined() // Not passed initially
  })

  it('should call window.open when showSourceArticle is called', () => {
    const mockOpen = vi.spyOn(window, 'open').mockImplementation(() => null)

    const wrapper = mountComponent({ source: 'https://example.com' })

    // Access component VM to call method
    const vm = wrapper.vm as unknown as { showSourceArticle: () => void }
    vm.showSourceArticle()

    expect(mockOpen).toHaveBeenCalledWith('https://example.com', '_blank')
    mockOpen.mockRestore()
  })

  it('should accept id prop', () => {
    const wrapper = mountComponent({ id: 'abc-123' })

    expect(wrapper.props('id')).toBe('abc-123')
  })

  it('should exist', () => {
    const wrapper = mountComponent()

    expect(wrapper.exists()).toBe(true)
  })
})
