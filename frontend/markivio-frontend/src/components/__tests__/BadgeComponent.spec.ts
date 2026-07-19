import { describe, it, expect } from 'vitest'
import { mount } from '@vue/test-utils'
import BadgeComponent from '@/components/BadgeComponent.vue'

function mountBadge(counter = 0) {
  return mount(BadgeComponent, { props: { counter } })
}

describe('BadgeComponent', () => {
  it('should render', () => {
    const wrapper = mountBadge()
    expect(wrapper.exists()).toBe(true)
  })

  it('should display 0 when counter is 0', () => {
    const wrapper = mountBadge(0)
    expect(wrapper.text()).toBe('0')
  })

  it('should display the counter value', () => {
    const wrapper = mountBadge(42)
    expect(wrapper.text()).toBe('42')
  })

  it('should display 99+ when counter exceeds 99', () => {
    const wrapper = mountBadge(100)
    expect(wrapper.text()).toBe('99+')
  })

  it('should display 99+ for very large values', () => {
    const wrapper = mountBadge(9999)
    expect(wrapper.text()).toBe('99+')
  })

  it('should display 99 when counter is exactly 99', () => {
    const wrapper = mountBadge(99)
    expect(wrapper.text()).toBe('99')
  })

  it('should have a rounded-full span', () => {
    const wrapper = mountBadge(5)
    expect(wrapper.find('span').classes()).toContain('rounded-full')
  })
})
