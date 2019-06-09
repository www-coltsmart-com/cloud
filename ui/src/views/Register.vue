<template>
<div class="login">
  <el-row >
      <el-col class="login-logo" :offset="1" :span="2">
        <img src="../assets/logo.png" width="200"/>
      </el-col>
  </el-row>

<el-row class="login-center" type="flex" :style="'background: url(' + require('../assets/bg.png') + ') no-repeat scroll center center / cover;' " justify="center">
      <el-col :xs="12" :sm="10" :md="8" :lg="8" :xl="6">
        <div style="height:50px;" class="hidden-lg-and-down"></div>
        <div style="height:50px;" ></div>
        <el-card class="login-box" >
            <el-form :model="RegisterUser" ref="RegisterUser" :rules="RegisterRules" status-icon label-width="0px"> 
            <h1 class="title">立即注册</h1>
            <p class="login-box-msg"></p>              
            <el-form-item label="" prop="UserNo">
              <el-input type="text" v-model="RegisterUser.UserNo" auto-complete="off" placeholder="请输入用户名" prefix-icon="el-icon-user-solid"></el-input>
            </el-form-item>
            <el-form-item prop="Password">
              <el-input :type="PassEye" v-model="RegisterUser.Password" auto-complete="off" placeholder="请输入密码" prefix-icon="el-icon-edit"> 
               <i slot="suffix" class="el-icon-view" @mousedown="PassEye= ''" @mouseup="PassEye='password'"></i>
               </el-input>
            </el-form-item>
            <el-form-item prop="ConfirmPassword">
              <el-input :type="PassEye2" v-model="RegisterUser.ConfirmPassword" auto-complete="off" placeholder="请再输入一遍密码" prefix-icon="el-icon-edit">
               <i slot="suffix" class="el-icon-view" @mousedown="PassEye2= ''" @mouseup="PassEye2='password'"></i>
               </el-input>
            </el-form-item>
            <el-form-item label="" prop="RegEmall">
              <el-input type="text" v-model="RegisterUser.RegEmall" auto-complete="off" placeholder="需要通过邮箱接收验证码" >
                <el-button v-if="enbleVerifyCode" slot="suffix" type="text" @click="sendVerifyCode">发送验证码</el-button>
                <el-button v-if="!enbleVerifyCode" slot="suffix" type="text" disabled>{{verify_time}}秒后重新发送</el-button>
              </el-input>              
            </el-form-item>
            <el-form-item label="" prop="NewPassword">
              <el-input type="text" v-model="RegisterUser.NewPassword" auto-complete="off" placeholder="验证码" ></el-input>
            </el-form-item>
            <el-form-item>
              <el-button type="primary" @click="submitForm" :loading="loading" style="width:100%">立即注册</el-button>
              <hr />
              <p>已有账号，直接去<el-link type="primary" @click="redirect">登录</el-link>吧</p>
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
          <span> ©&nbsp;2018 &nbsp;&nbsp; 上海市鸣驹智能科技有限公司 &nbsp;&nbsp; 技术支持 </span>
        </div>
      </el-col>
  </el-row>
  </div>
</template>

<script>
import JsEncrypt from 'jsencrypt'
import 'element-ui/lib/theme-chalk/display.css';
import { setInterval, clearInterval, setTimeout } from 'timers';

export default {
  name: 'register',
  data: function () {
    const validateUserNo = (rule,value,callback)=>{
      if(value.length<=0){
        callback(new Error('用户名不能为空'))
      }else{
        callback()
      }
    }
    const validatePass = (rule, value, callback) => {
      if (value === '' || value.length < 5) {
        callback(new Error('密码不能小于5位'))
      } else {
        if(this.RegisterUser.ConfirmPassword !== ''){
          this.$refs.RegisterUser.validateField('ConfirmPassword');
        }
        callback()
      }
    }
    const validateConfirmPass = (rule,value,callback)=>{
      if(value === '' || value.length < 5 ){
        callback(new Error('确认密码无效，请重新输入'))
      }else if(value != this.RegisterUser.Password){
        callback(new Error('两次密码输入不一致'))
      }else{
        callback()
      }
    }
    return {
      PassEye: 'password',
      PassEye2:'password',
      loading:false,
      enbleVerifyCode:true,
      verify_time:60,
      RegisterUser:{
        UserNo:'',
        Password:'',
        ConfirmPassword:'',
        RegEmall:'',
        NewPassword:''
      },
      RegisterRules: {
        UserNo: [{required: true, trigger: 'blur', validator: validateUserNo}],
        Password: [{required: true, trigger: 'blur', validator: validatePass}],
        ConfirmPassword:[{required:true,trigger:'blur',validator:validateConfirmPass}],        
        RegEmall:[{required:true,type: 'email', message: '请输入正确的邮箱地址', trigger: ['blur', 'change'] }],
        NewPassword:[{required:true,message: '请输入验证码', trigger: 'blur'}],
      }
    }
  },
  methods: {
    submitForm () {
      this.$refs['RegisterUser'].validate((valid) => {
        if (valid) {
          this.saveRegistration()
        } else {
          return false
        }
      })
    },
    sendVerifyCode(){
      var email = this.RegisterUser.RegEmall;
      if(!email){
        this.$message({
          message:'邮箱不能为空',
          type:'error'
        })
        return;
      }
      this.enbleVerifyCode = false;
      this.$http.get('/api/login/sendverifycode?email='+email).then(res=>{
        this.$message({
          message: res.data.Result,
          type: (res.data.Type == "Error"?'error':'success')
        });     
      }).catch(error=>{
        this.$message({
          message:err.message,
          type:'error'
        })
      });
      this.verify_time = 60;
      var handle = setInterval(()=>{
        this.verify_time--;
        if(this.verify_time<=0){
          this.verify_time = 60;
          this.enbleVerifyCode = true;
          clearInterval(handle);
        }
      },1000);
    },
    redirect:function(){
      this.$router.push('/login')
    },
    saveRegistration(){
      this.loading = true
      this.getPublicKey((publicKey)=>{
        var encrypt = new JsEncrypt();
        encrypt.setPublicKey(publicKey);

        var user = {
          UserNo:this.RegisterUser.UserNo,
          Password:encrypt.encrypt(this.RegisterUser.ConfirmPassword),
          RegEmall:this.RegisterUser.RegEmall,
          NewPassword:this.RegisterUser.NewPassword
        };
        this.$http.post("api/Login/savereg", user,{rsa: true}).then((res)=>{
            this.loading = false
            if (res.data.Type == "Error"){
              this.$message({
                message: res.data.Result,
                type: 'error'
              })
            } else {
              this.$message({
                message: '恭喜您注册成功，即将跳转到登录页!',
                type: 'success'
              }); 
              setTimeout(()=>{
                this.$router.push('/login');
              },2000);              
            }
          }).catch(error=> {
            this.loading = false
            this.$message({
              message: error.message,
              type: 'error'
            })
          })
      });
    },
    getPublicKey:function(callback){
      this.$http.get('/api/login/getpublickey').then(res=>{
        if(callback){
          callback(res.data)
        }
      }).catch(error=>{
        this.$message({
          message:err.message,
          type:'error'
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