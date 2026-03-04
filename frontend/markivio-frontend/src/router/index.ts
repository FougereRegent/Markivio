import { authGuard } from '@auth0/auth0-vue';
import { createRouter, createWebHistory, type RouteRecordRaw } from 'vue-router';

const routes: RouteRecordRaw[] = [
  {
    path: '/',
    redirect: '/login',
  },
  {
    path: '/callback',
    component: () => import('@/pages/CallBack.vue'),
  },
  {
    path: '/login',
    component: () => import('@/pages/SignIn.vue'),
    name: 'login',
  },
  {
    path: '/app',
    component: () => import('@/DefaultLayout.vue'),
    beforeEnter: authGuard,
    children: [
      { path: 'home', component: () => import('@/pages/DashBoard.vue'), name: 'home' },
      { path: 'user', component: () => import('@/pages/UpdateProfil.vue'), name: 'updateUser' },
    ],
  },
];

const router = createRouter({
  history: createWebHistory(import.meta.env.BASE_URL),
  routes,
});

export default router;
