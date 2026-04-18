import { describe, expect, it, beforeEach } from 'vitest'
import { useAddEditDrawer, ActionDrawer } from '@/stores/add-edit-drawer-store'
import { createPinia, setActivePinia } from 'pinia'

describe('add-edit-drawer-store', () => {
  beforeEach(() => {
    setActivePinia(createPinia())
  })

  it('Should have drawerState false by default', () => {
    const store = useAddEditDrawer()
    expect(store.drawerState).eq(false)
  })

  it('Should have drawerType set to Create by default', () => {
    const store = useAddEditDrawer()
    expect(store.drawerType).eq(ActionDrawer.Create)
  })

  it('Should set drawerState to true when open is called', () => {
    const store = useAddEditDrawer()
    store.open()
    expect(store.drawerState).eq(true)
  })

  it('Should set drawerState to false when close is called', () => {
    const store = useAddEditDrawer()
    store.open()
    expect(store.drawerState).eq(true)

    store.close()
    expect(store.drawerState).eq(false)
  })

  it('Should toggle drawerState with open and close', () => {
    const store = useAddEditDrawer()

    store.open()
    expect(store.drawerState).eq(true)

    store.close()
    expect(store.drawerState).eq(false)

    store.open()
    expect(store.drawerState).eq(true)
  })
})
