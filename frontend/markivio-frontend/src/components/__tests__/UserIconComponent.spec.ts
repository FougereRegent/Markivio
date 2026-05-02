import { describe, it, expect, vi, beforeEach } from 'vitest'
import { mount } from '@vue/test-utils'
import UserIconComponent from '@/components/UserIconComponent.vue'

// Mock auth store
const mockGetUser = vi.fn()

vi.mock('@/features/auth/stores/auth-store', () => ({
  useAuthStore: () => ({
    getUser: mockGetUser(),
  }),
}))

describe('UserIconComponent', () => {
  beforeEach(() => {
    vi.clearAllMocks()
  })

  it('should render user image when accountPicture is provided', () => {
    mockGetUser.mockReturnValue({
      authId: '123',
      firstName: 'John',
      lastName: 'Doe',
      accountPicture: 'https://example.com/pic.jpg',
    })

    const wrapper = mount(UserIconComponent)

    const img = wrapper.find('img')
    expect(img.exists()).toBe(true)
    expect(img.attributes('src')).toBe('https://example.com/pic.jpg')
  })

  it('should render default user image when accountPicture is empty', () => {
    mockGetUser.mockReturnValue({
      authId: '123',
      firstName: 'John',
      lastName: 'Doe',
      accountPicture: '',
    })

    const wrapper = mount(UserIconComponent)

    const img = wrapper.find('img')
    expect(img.attributes('src')).toBe('/src/assets/default-user.svg')
  })

  it('should apply blue background when using default user', () => {
    mockGetUser.mockReturnValue({
      authId: '123',
      firstName: 'John',
      lastName: 'Doe',
      accountPicture: '',
    })

    const wrapper = mount(UserIconComponent)

    const div = wrapper.find('div')
    expect(div.classes()).toContain('bg-blue-500')
  })

  it('should not apply blue background when using custom picture', () => {
    mockGetUser.mockReturnValue({
      authId: '123',
      firstName: 'John',
      lastName: 'Doe',
      accountPicture: 'https://example.com/pic.jpg',
    })

    const wrapper = mount(UserIconComponent)

    const div = wrapper.find('div')
    expect(div.classes()).not.toContain('bg-blue-500')
  })

  it('should emit clickIcon when clicked', async () => {
    mockGetUser.mockReturnValue({
      authId: '123',
      firstName: 'John',
      lastName: 'Doe',
      accountPicture: 'https://example.com/pic.jpg',
    })

    const wrapper = mount(UserIconComponent)

    await wrapper.find('div').trigger('click')

    expect(wrapper.emitted('clickIcon')).toBeTruthy()
  })

  it('should have cursor pointer class', () => {
    mockGetUser.mockReturnValue({
      authId: '123',
      firstName: 'John',
      lastName: 'Doe',
      accountPicture: 'https://example.com/pic.jpg',
    })

    const wrapper = mount(UserIconComponent)

    const div = wrapper.find('div')
    expect(div.classes()).toContain('cursor-pointer')
  })

  it('should render rounded-full image', () => {
    mockGetUser.mockReturnValue({
      authId: '123',
      firstName: 'John',
      lastName: 'Doe',
      accountPicture: 'https://example.com/pic.jpg',
    })

    const wrapper = mount(UserIconComponent)

    const img = wrapper.find('img')
    expect(img.classes()).toContain('rounded-full')
  })
})
