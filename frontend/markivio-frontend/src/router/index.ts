import DefaultLayout from '@/DefaultLayout.vue'
import CallBack from '@/pages/CallBack.vue'
import DashBoard from '@/pages/DashBoard.vue'
import SignIn from '@/pages/SignIn.vue'
import { authGuard } from '@auth0/auth0-vue'
import { createRouter, createWebHistory, type RouteRecordRaw} from 'vue-router'

const routes: RouteRecordRaw[] = [
  {
    path: '/', 
    redirect: '/login'
  },
  {
    path: '/callback',
    component: CallBack
  },
  {
    path: '/login', 
    component: SignIn,
    name: "login",
  },
  {
    path: '/app', 
    component: DefaultLayout, 
    beforeEnter: authGuard,
    children: 
    [
      {path: 'home', component: DashBoard, name:"home"}
    ],
  },
]

const router = createRouter({
  history: createWebHistory(import.meta.env.BASE_URL),
  routes
})

export default router
