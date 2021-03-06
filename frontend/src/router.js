import Vue from 'vue'
import Router from 'vue-router'
import Home from './views/Home.vue'
import Attendants from './views/Attendants.vue'
import DialogFlow from './components/DialogFlow/DialogFlow.vue'

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
      path: '/attendants',
      name: 'attendants',
      component: Attendants
    },
    {
      path: '/dialog',
      name: 'dialogflow',
      component: DialogFlow
    }
  ]
})
