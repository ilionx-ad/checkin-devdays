import Vue from 'vue'
import Router from 'vue-router'
import Home from './views/Home.vue'
import Eindbazen from './views/Eindbazen.vue'
import Attendants from './views/Attendants.vue'

Vue.use(Router)

export default new Router({
  mode: 'history',
  base: process.env.BASE_URL,
  routes: [
    {
      path: '/',
      name: 'home',
      component: Home
    },
    {
      path: '/eindbazen',
      name: 'eindbazen',
      component: Eindbazen
    },
    {
      path: '/attendants',
      name: 'attendants',
      component: Attendants
    }
  ]
})
