import Vue from 'vue'
import axios from 'axios'
import Router from 'vue-router'
import Home from '@/views/layout/Home'
import Login from '@/views/Login'
import Register from '@/views/Register'
import ModifyPassword from '@/views/settings/ModifyPassword'
import AccountInfo from '@/views/settings/AccountInfo'
import ResetPassword from '@/views/ResetPassword'

Vue.use(Router)

const router = new Router({
  routes: [
    { 
      path: '/login', 
      name: 'login',
      component: Login, 
      hidden: true 
    },
    { 
      path: '/register', 
      name: 'register',
      component: Register, 
      hidden: true 
    },
    { 
      path: '/resetpassword', 
      name: 'resetpassword',
      component: ResetPassword, 
      hidden: true 
    },
    {
      path: '/',
      name: '首页',
      component: Home, 
      hidden: false,
      children:[        
        {
          path: '/account',
          name: '账户信息',
          hidden: true,
          component: AccountInfo
        },
        {
          path: '/password',
          name: '修改密码',
          hidden: true,
          component: ModifyPassword
        },
        {
          path: '/userlist',
          name: '用户列表',
          hidden: false,
          component: () => import('@/views/userlist/index')
        },
        {
          path: '/devicelist',
          name: '设备列表',
          hidden: false,
          component: () => import('@/views/devicelist/index')
        }
      ]
    }
  ]
})

const tokenIsExpired = function () {
  if(localStorage.expiration)
  {
      var expirationDt = new Date(localStorage.expiration)
      if (expirationDt.getTime() < (new Date()).getTime()) {
          return true
      }
  }
  return false
}

const checkRegistion = function () {
  return axios.get('api/Login/checkri',{
    baseURL: process.env.BASE_URL,
    timeout: 600000
}).then((response) => {
    return Promise.resolve(response.data)
  }).catch(error=> Promise.reject(error))
}

router.beforeEach((to, from, next) => {
  // 获取 JWT Token
  if (localStorage.token) {
    next()
  } else {
    if (to.name === 'login') 
    {
      next()
      /*
      checkRegistion().then(result => {        
        if (result == true) {
          next()
        }
        else {
          next({name: 'register'})
        }
      }).catch(error => {
        this.$message({
          message: error.message,
          type: 'error'
        })
      })
      */
    } 
    else if (to.name === 'register') {
      next()
    }
    else {
      next({name: 'login'});
    }
  }
})

export default router
