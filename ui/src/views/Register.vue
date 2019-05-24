<template>
<div class="login">
  <el-row >
      <el-col class="login-logo" :offset="1" :span="2">
        <img src="../assets/logo.png" width="200"/>
      </el-col>
  </el-row>

<el-row class="login-center" type="flex" :style="'background: url(' + require('../assets/bg.png') + ') no-repeat scroll center center / cover;' " justify="center">
      <el-col :xs="12" :sm="10" :md="8" :lg="8" :xl="6">
        <div style="height:80px;" class="hidden-lg-and-down"></div>
        <div style="height:80px;" ></div>
        <el-card class="login-box" >
            <el-form :model="RegisterInfo" ref="RegisterInfo" :rules="RegisterRules" status-icon label-width="0px"> 
            <h1 class="title">昂捷部署平台注册</h1>
              <p class="login-box-msg"></p>
              
            <el-form-item label="" prop="Use">
              <el-select ref="selectInput" v-model="RegisterInfo.Use" style="width:100%" auto-complete="off" placeholder="请选择平台用途">
                <el-option v-for="item in Useage.Options" :key="item.Code" :label="item.Name" :value="item.Code"></el-option>
              </el-select>
            </el-form-item>
            <el-form-item label="" prop="No">
              <el-input type="text" v-model="RegisterInfo.No" auto-complete="off" placeholder="请输入客户代码" ></el-input>
            </el-form-item>
            <el-form-item prop="RegPassword">
              <el-input :type="PassEye" v-model="RegisterInfo.RegPassword" auto-complete="off" placeholder="请输入注册密码">
               <i slot="suffix" class="el-icon-view" @mousedown="PassEye= ''" @mouseup="PassEye='password'"></i>
               </el-input>
            </el-form-item>
            <el-form-item>
              <el-button type="primary" @click="submitForm" :loading="loading" style="width:100%">立即注册</el-button>
              <!-- <el-button type="primary" @click="redirect()" >登录</el-button> -->
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
          <span> ©&nbsp;2018 &nbsp;&nbsp; 深圳市昂捷信息技术股份有限公司 &nbsp;&nbsp; 技术支持 </span>
        </div>
      </el-col>
  </el-row>
  </div>
</template>

<script>
import JsEncrypt from 'jsencrypt'
import 'element-ui/lib/theme-chalk/display.css';

export default {
  name: 'register',
  data: function () {
    const validateClientCode = (rule, value, callback) => {
      if (value.length <= 0) {
        callback(new Error('请输入正确的客户编码'))
      } else {
        callback()
      }
    }
    const validateRegCode = (rule, value, callback) => {
      if (value.length !=36) {
        callback(new Error('注册码必须为36位'))
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
      PassEye: 'password',
      loading:false,
      RegisterInfo:{
        No:'',
        Use:''
      },
      RegisterRules: {
        No: [{required: true, trigger: 'blur', validator: validateClientCode}],
        Use: [{required: true, trigger: 'blur', message: "请选择平台用途" }],
        RegPassword: [{required: true, trigger: 'blur', validator: validatePass}]
      },
      Useage:{
        Options:[{Code:'测试',Name:'测试'},{Code:'正式',Name:'正式'}]
      }
    }
  },
  methods: {
    submitForm () {
      this.$refs['RegisterInfo'].validate((valid) => {
        if (valid) {
          this.saveRegistration()
        } else {
          // console.log('error submit!!')
          return false
        }
      })
    },
    redirect:function(){
      this.$router.push('/login')
    },


    saveRegistration(){
      this.loading = true
      this.$http.post("api/Login/saveri", this.RegisterInfo).then(
        (res)=>{
          this.loading = false
          if (res.data.Type == "Error")
          {
            this.$message({
              message: res.data.Result,
              type: 'error'
            })
          }
          else {
            this.$router.push('/login')
          }
        }
      ).catch(error=> {
        this.loading = false
        this.$message({
          message: error.message,
          type: 'error'
        })
      })
    }
  },
  mounted: function () {
    //this.getPublicKey()
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