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
            <el-form :model="form" ref="form" :rules="rules" status-icon label-width="0px"> 
            <h1 class="title">重置密码</h1>
            <p class="login-box-msg"></p>
            <el-form-item label="电子邮箱" prop="email">
              <el-input type="text" v-model="form.email" auto-complete="off" placeholder="需要通过邮箱接收验证码" >
                <el-button v-if="enble_verify_code" slot="suffix" type="text" @click="sendVerifyCode">发送验证码</el-button>
                <el-button v-if="!enble_verify_code" slot="suffix" type="text" disabled>{{verify_time}}秒后重新发送</el-button>
              </el-input>              
            </el-form-item>
            <el-form-item label="验证码" prop="verify_code">
              <el-input type="text" v-model="form.verify_code" auto-complete="off" placeholder="验证码" ></el-input>
            </el-form-item>
            <el-form-item>
              <el-button type="primary" @click="submitForm" :loading="loading" style="width:100%">重置密码</el-button>
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
import 'element-ui/lib/theme-chalk/display.css';
import { setInterval, clearInterval, setTimeout } from 'timers';

export default {
  name: 'ResetPassword',
  data: function () {
    return {
      loading:false,
      enble_verify_code:true,
      verify_time:60,
      form:{
        email:'',
        verify_code:''
      },
      rules: {   
        email:[{required:true,type: 'email', message: '请输入正确的邮箱地址', trigger: ['blur', 'change'] }],
        verify_code:[{required:true,message: '请输入验证码', trigger: 'blur'}],
      }
    }
  },
  methods: {
    submitForm () {
      this.$refs['form'].validate((valid) => {
        if (valid) {
          this.resetPassword()
        } else {
          return false;
        }
      })
    },
    sendVerifyCode(){
      var email = this.form.email;
      if(!email){
        this.$message({
          message:'邮箱不能为空',
          type:'error'
        })
        return;
      }
      this.enble_verify_code = false;
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
          this.enble_verify_code = true;
          clearInterval(handle);
        }
      },1000);
    },
    resetPassword(){
        this.loading = true;
        this.$http.post("api/Login/resetpassword", {
            RegEmall:this.form.email,
            NewPassword:this.form.verify_code
        }).then((res)=>{
            this.loading = false
            if (res.data.Type == "Error"){
                this.$message({
                    message: res.data.Result,
                    type: 'error'
                })
            } else {
                this.$message({
                    message: '密码重置成功，即将跳转到登录页!',
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
    }
  },
  mounted: function () {
  },
}
</script>

<style>