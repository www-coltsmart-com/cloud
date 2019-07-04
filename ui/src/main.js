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

import quillEditor from 'vue-quill-editor'
import 'quill/dist/quill.core.css'
import 'quill/dist/quill.snow.css'
import 'quill/dist/quill.bubble.css'

import 'vue-element-extends/lib/index.css'
import { Editable, EditableColumn } from 'vue-element-extends'

Vue.config.productionTip = false

Vue.use(ElementUI, { size: 'small' })
Vue.use(token)
Vue.use(Axios)
Vue.use(quillEditor)
Vue.use(Editable)
Vue.use(EditableColumn)

window.onbeforeunload = function () {
  token.clear()
}

/* eslint-disable no-new */
new Vue({
  el: '#app',
  router,
  components: { App },
  template: '<App/>'
})
