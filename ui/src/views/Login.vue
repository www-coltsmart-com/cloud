<template>
 <div class="login">
    <el-row >
      <el-col class="login-logo" :offset="1" :span="2">
        <img src="../assets/logo.png" width="200px"/>
      </el-col>
    </el-row>
    <el-row class="login-center" type="flex" :style="'background: url(' + require('../assets/bg.png') + ') no-repeat scroll center center / cover;' " justify="center">
      <el-col :xs="12" :sm="10" :md="8" :lg="8" :xl="6">
        <div style="height:80px;" class="hidden-lg-and-down"></div>
        <div style="height:80px;" ></div>
        <el-card class="login-box" >
          <el-form :model="LoginUser" ref="LoginUser" :rules="LoginRules" status-icon> 
            <h1 class="title">欢迎登录</h1>
            <p class="login-box-msg"></p>
            <el-form-item>
              <el-input type="text" v-model="LoginUser.UserName" auto-complete="off" placeholder="请输入用户名" ref="uname"  @keyup.enter.native="jumponenter($event)"></el-input>
            </el-form-item>
            <el-form-item>
              <el-input type="password" v-model="LoginUser.Password" auto-complete="off" placeholder="请输入密码" ref="pwd" @keyup.enter.native="jumponenter($event)"></el-input>
            </el-form-item>
            <el-form-item>
              <el-button type="primary" @click="submitForm" :loading="loading" class="pull-right" style="width:100%" ref="login">登录</el-button>
            </el-form-item>
          </el-form>
        </el-card>
        <div style="height:50px;" ></div>
        <div style="height:80px;" class="hidden-lg-and-down"></div>
      </el-col>
    </el-row>
    <el-row type="flex" justify="center">
      <el-col :span="12">
      <div class="login-bottom" >
        <span> ©&nbsp;2018 &nbsp;&nbsp; 上海市鸣驹智能科技有限公司 &nbsp;&nbsp; 技术支持 </span>
      </div>
      </el-col>
    </el-row>
  </div>
</template>

<script>
import JsEncrypt from 'jsencrypt'
import 'element-ui/lib/theme-chalk/display.css';

export default {
  name: 'login',
  data: function () {
    const validateUsername = (rule, value, callback) => {
      if (value.length <= 0) {
        callback(new Error('请输入正确的用户名'))
      } else {
        callback()
      }
    }
    const validatePass = (rule, value, callback) => {
      if (value.length < 5) {
        callback(new Error('密码不能小于5位'))
      } else {
        callback()
      }
    }
    return {
      LoginUser: {
        UserName: '',
        Password: ''
      },
      LoginRules: {
        UserName: [{required: true, trigger: 'blur', validator: validateUsername}],
        Password: [{required: true, trigger: 'blur', validator: validatePass}]
      },
      loading: false,
      checked: true
    }
  },
  methods: {
    submitForm () {
      var that = this
      this.$refs['LoginUser'].validate((valid) => {
        if (valid) {
          this.loading = true
          this.getPublicKey((publickey) => {
            var encrypt = new JSEncrypt();
              encrypt.setPublicKey(publickey);

              var user = {
                UserName: this.LoginUser.UserName,
                Password: encrypt.encrypt(this.LoginUser.Password)
              }
              this.$http.post('/api/login', user, { rsa: true})
                .then(res => {
                  
                  // 登录成功
                  this.loading = false
                  if (res.data) {
                    // 储存 token
                    //token.set(res.data)
                   that.$router.push({ path: '/' })
                  }
                  localStorage.UserName = this.LoginUser.UserName
                })
                .catch(error => {
                  this.loading = false
                  if (error.response.status == 401) {
                    this.$message({
                      message: '用户名或密码错误，登录失败1',
                      type: 'error'
                    })
                  }
                  else if (error.response.status == 405) {
                    this.$message({
                      message: '客户编码为空或不存在，登录失败',
                      type: 'error'
                    })
                  }
                  else {
                  this.loading = false
                    this.$message({
                      message: error.message,
                      type: 'error'
                    })
                  }
                })
         })    
        } else {
          // console.log('error submit!!')
          return false
        }
      })
    },
    getPublicKey: function (callback) {
      this.$http.get('/api/login/getpublickey').then(res => {
              //localStorage.setItem("publickey", res.data)
              if(callback)
                callback(res.data)
          }).catch(error => {  
             this.$message({
                  message: error.message,
                  type: 'error'
                })       
          })
    },
    jumponenter:function(e){
      const phtext=e.target.getAttribute('placeholder');
      if(phtext==="请输入用户名"){
        this.$refs.pwd.focus()
      }
      else if(phtext==="请输入密码"){
        this.submitForm()
      }
    }
  },
  mounted: function () {
    //this.getPublicKey()
    this.$refs.uname.focus()
  },

}
</script>
<style>
.login {
  /* background: url('../assets/bg.png') no-repeat scroll center center / cover; */
  background-size: 100% 100%;
  width: 100%;
  height: 100%;
  position: fixed;
}

.login-box {
  background: #ffffff;
  min-width: 400px;
  border-radius: 5px;
  border: 1px solid #eaeaea;
  box-shadow: 0 0 25px #cac6c6;
}

.login-box-msg {
  color: #000000;
  text-align: center;
}

.login-box .title {
  color: #000000;
  text-align: center;
}
.login-center {
  margin-top: 40px;
  z-index: 1;
}
.login-bottom {
  margin-top: 20px;
  min-width: 300px;
  font-size:12px;
}
.login-logo {
  margin-top: 40px;
  z-index: 1;
}
</style>