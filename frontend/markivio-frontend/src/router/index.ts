import App from '@/App.vue'
import DefaultLayout from '@/DefaultLayout.vue'
import DashBoard from '@/pages/DashBoard.vue'
import SignIn from '@/pages/SignIn.vue'
import { createRouter, createWebHistory, type RouteRecordRaw} from 'vue-router'

const routes: RouteRecordRaw[] = [
  {path: '/', component: App},
  {path: '/login', component: SignIn},
  {path: '/app', component: DefaultLayout, children: [
    {path: '', component: DashBoard}
  ]},
]

const router = createRouter({
  history: createWebHistory(import.meta.env.BASE_URL),
  routes
})

export default router
