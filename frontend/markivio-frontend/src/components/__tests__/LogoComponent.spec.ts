import { describe, it, expect, vi } from 'vitest'
import { mount } from '@vue/test-utils'
import LogoComponent from '@/components/LogoComponent.vue'
import router from '@/router'

// Mock router
vi.mock('@/router', () => ({
  default: {
    push: vi.fn(),
  },
}))

describe('LogoComponent', () => {
  it('should render logo image', () => {
    const wrapper = mount(LogoComponent)

    const img = wrapper.find('img')
    expect(img.exists()).toBe(true)
    expect(img.attributes('alt')).toBe('markivio logo')
  })

  it('should render Markivio text', () => {
    const wrapper = mount(LogoComponent)

    expect(wrapper.text()).toContain('Markivio')
  })

  it('should navigate to home when clicked', async () => {
    const wrapper = mount(LogoComponent)

    await wrapper.find('div').trigger('click')

    expect(router.push).toHaveBeenCalledWith({ name: 'home' })
  })

  it('should have correct CSS classes', () => {
    const wrapper = mount(LogoComponent)

    const div = wrapper.find('div.flex')
    expect(div.classes()).toContain('flex')
    expect(div.classes()).toContain('items-center')
  })

  it('should have correct image size class', () => {
    const wrapper = mount(LogoComponent)

    const img = wrapper.find('img')
    expect(img.classes()).toContain('size-10')
  })

  it('should have correct text styling', () => {
    const wrapper = mount(LogoComponent)

    const h2 = wrapper.find('h2')
    expect(h2.classes()).toContain('text-4xl')
    expect(h2.classes()).toContain('font-semibold')
  })
})
