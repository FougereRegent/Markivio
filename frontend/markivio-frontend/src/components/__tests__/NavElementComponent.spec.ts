import { describe, it, expect } from 'vitest'
import { mount } from '@vue/test-utils'
import NavElementComponent from '@/components/NavElementComponent.vue'

describe('NavElementComponent', () => {
  it('should render name correctly', () => {
    const wrapper = mount(NavElementComponent, {
      props: {
        name: 'All Articles',
        iconClass: 'ri-article-line',
      },
    })

    expect(wrapper.text()).toContain('All Articles')
  })

  it('should render icon when iconClass is provided', () => {
    const wrapper = mount(NavElementComponent, {
      props: {
        name: 'Test',
        iconClass: 'ri-home-line',
      },
    })

    const icon = wrapper.find('i')
    expect(icon.exists()).toBe(true)
    expect(icon.classes()).toContain('ri-home-line')
  })

  it('should not render icon when iconClass is empty', () => {
    const wrapper = mount(NavElementComponent, {
      props: {
        name: 'Test',
        iconClass: '',
      },
    })

    const icon = wrapper.find('i')
    expect(icon.attributes('hidden')).toBe('')
  })

  it('should render color tag when tagColor is provided', () => {
    const wrapper = mount(NavElementComponent, {
      props: {
        name: 'Favorites',
        tagColor: '#ff0000',
      },
    })

    const tag = wrapper.find('div.size-4')
    expect(tag.exists()).toBe(true)
    // Check that the style contains the background color (RGB format from hex)
    expect(tag.attributes('style')).toContain('background-color: rgb(255, 0, 0)')
  })

  it('should not render color tag when tagColor is not provided', () => {
    const wrapper = mount(NavElementComponent, {
      props: {
        name: 'Test',
      },
    })

    const tag = wrapper.find('div.size-4')
    expect(tag.attributes('hidden')).toBe('')
  })

  it('should emit click-element-nav when clicked', async () => {
    const wrapper = mount(NavElementComponent, {
      props: {
        name: 'Test',
      },
    })

    await wrapper.find('div').trigger('click')

    expect(wrapper.emitted('click-element-nav')).toBeTruthy()
    expect(wrapper.emitted('click-element-nav')).toHaveLength(1)
  })

  it('should apply hover classes', () => {
    const wrapper = mount(NavElementComponent, {
      props: {
        name: 'Test',
      },
    })

    const div = wrapper.find('div')
    expect(div.classes()).toContain('hover:bg-neutral-100')
    expect(div.classes()).toContain('hover:cursor-pointer')
  })
})
