// The Vue build version to load with the `import` command
// (runtime-only or standalone) has been set in webpack.base.conf with an alias.
import Vue from 'vue'
import ElementUI from 'element-ui'
import 'element-ui/lib/theme-chalk/index.css'
import App from './App'
import router from './router'
import Axios from './axios/axios'
import token from './utils/token'
import 'babel-polyfill'

Vue.config.productionTip = false

Vue.use(ElementUI,{ size: 'small' })
Vue.use(token)
Vue.use(Axios)

window.onbeforeunload = function(){
  token.clear()
}

/* eslint-disable no-new */
new Vue({
  el: '#app',
  router,
  components: { App },
  template: '<App/>'
})