import Vue from 'vue'
import App from './App.vue'
import router from './router'

import config from './components/DialogFlow/config/config'

Vue.prototype.config = config
Vue.config.productionTip = false

/* (global) This code is going to tell us, if history mode can be activated on the client, so the application can be consumed without localstorage */
Vue.prototype.history = () => {
  try {
      localStorage.getItem('check')
      return true
  }

  catch {
      return false
  }
}

/* (global) Currently selected language or fallback language (en). Needs to be a function, since it's reactive. No need for vuex there */
Vue.prototype.lang = () => {
  if(Vue.prototype.history()) return localStorage.getItem('lang') || config.app.fallback_lang

  else {
      return config.app.fallback_lang
  }
}

new Vue({
  router,
  render: h => h(App)
}).$mount('#app')
