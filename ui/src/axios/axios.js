require('es6-promise').polyfill()
import axios from 'axios'
import token from '../utils/token'
import JsEncrypt from 'jsencrypt'

export const Axios = axios.create({
    baseURL: process.env.BASE_URL,
    timeout: 600000
})

export default {
    install(Vue) {
        Object.defineProperty(Vue.prototype, '$http', { value: Axios })
    }
}

const refreshToken = function () {
  return Axios.get('/api/login/refreshtoken').then(res => {
            if(res.data) {
                // 储存 token
                token.set(res.data)
            }
            return Promise.resolve(res.data)
        }).catch(error => {            
            return Promise.reject(error)
        })
}

// http request 拦截器
Axios.interceptors.request.use(
config => {

    if (token.hasValue()) {  // 判断是否存在token，如果存在的话，则每个http header都加上token
        config.headers.Authorization = `Bearer ${token.obtain()}`;
    }

    return config;
},
err => {
    return Promise.reject(err);
})
  
// http response 拦截器
Axios.interceptors.response.use(
response => {
    var isLogin = response.config.url.endsWith('/api/login')
    if (isLogin) {
        token.set(response.data)
    }
    return response;
},
error => {
    if (error.response) {       
        const originalRequest = error.config
        var isLogin = error.config.url.endsWith('/api/login')
        if (!isLogin && error.response.status === 401 && !originalRequest._retry) {
            originalRequest._retry = true

            return refreshToken().then((token) => {
                //重新设置auth
                originalRequest.headers.Authorization = `Bearer ${token}`;
                return Axios(originalRequest)
            })
        }
    }

    return Promise.reject(error);
})