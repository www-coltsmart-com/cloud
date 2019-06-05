<template>
  <el-container style="height: 100%;">
    <div class="aside">   
      <app-menu :collapse="!menuopened"></app-menu>  
    </div>
    <el-container style="height: 100%;">
        <el-header height="50px" class="header">
          <el-container class="header">
            <el-aside width="40px">
              <hamburger class="hamburger-container" :toggleClick="toggleSideBar" 
              :isActive="menuopened"></hamburger>
              </el-aside>
            <el-main class="header-main">
            <el-col :span="6" align="middle">
              <el-breadcrumb separator="/" class="breadcrumb-inner">
                <el-breadcrumb-item v-for="item in this.$route.matched" :key="item.path">
                  {{ item.name }}
                </el-breadcrumb-item>
              </el-breadcrumb>
            </el-col>
            
            <el-col :span="5" align="middle" >
              <el-breadcrumb class="el-col-header" :class="el-col-header">{{info.totaldevice}}&nbsp;</el-breadcrumb>
            </el-col>
            <el-col :span="5" align="middle" >
              <el-breadcrumb class="el-col-header" :class="el-col-header">{{info.onlinedevice}}&nbsp;</el-breadcrumb>
            </el-col>
            <el-col :span="5" align="middle" >
              <el-breadcrumb class="el-col-header" >{{info.totaluser}}&nbsp;</el-breadcrumb>
            </el-col>
            <el-col :span="2" >
              <el-dropdown trigger="hover" style="float:right;" >
                <div class="el-dropdown-link" style="vertical-align:middle;text-align:center;margin:0 auto">
                  <img src="../../assets/touxiang.png" class="use-image" :alt="user.username"/>
                </div>
                <el-dropdown-menu slot="dropdown">
                  <el-dropdown-item @click.native="accountInfo">账户信息</el-dropdown-item>
                  <el-dropdown-item @click.native="modifyPassword">修改密码</el-dropdown-item>
                  <el-dropdown-item divided @click.native="logout">退出登录</el-dropdown-item>
                </el-dropdown-menu>
              </el-dropdown>
            </el-col>
           <el-col :span="1" >
            <!-- {{user.username}}-->
           </el-col>
            </el-main>
          </el-container>
        </el-header>
        <el-main class="main">
          <div class="container" >
            <router-view></router-view>
          </div>
        </el-main>
    </el-container>
  </el-container>
</template>

<script>
import AppMenu from '@/views/layout/components/AppMenu'
import Hamburger from '@/components/Hamburger'

export default {
  name: 'layout',
  components: {
    AppMenu,
    Hamburger
  },
  data() {
    return {
      user: {
        id: 100,
        username: "admin",
        avatar: '../../assets/touxiang.png'
      },
      info:{
        totaldevice:"",
        onlinedevice:"",
        totaluser:"",
      },
      isAdmin:false,
      asideWidth: '200px',
      menuopened: true
    }
  },
  methods: {
    logout: function () {
      this.$confirm('确认退出吗?', '提示', {
        type: 'warning'
      }).then(() => {
        this.$router.push('/login');
      }).catch(() => {
      })
    },
    modifyPassword: function () {
      this.$router.push('/password');
    },
    accountInfo: function () {
      this.$router.push('/account');
    },
   
    toggleSideBar() {
      this.menuopened = !this.menuopened
    },
    getUserInfo:function(){
      this.$http.get('api/Login/userinfo?userName='+this.user.username,).then((res)=>{
        if(res.data.Id ==0){
          this.$router.push('/Login')
        }
        else{
          this.user.id=res.data.Id;
          this.user.isAdmin=res.data.IsAdmin;

          //如果是默认密码，则强制修改
          if(res.data.IsDefaultPassword){
            this.$confirm('您的密码不安全，请修改密码后重新登录！', '提示', {
              confirmButtonText: '现在去修改',
              cancelButtonText: '退出登录',
              type: 'warning'
            }).then(() => {
              this.$router.push('/password')
            }).catch(() => {
              this.$router.push('/Login')         
            });
          }
          if(this.user.isAdmin){
            this.info.totaldevice="总设备数："+res.data.TotalDevice;
            this.info.totaluser="总用户数："+res.data.TotalUser;
            this.info.onlinedevice="在线设备："+res.data.OnlineDevice;
          }
        }
      });
    }
  },
  mounted:function(){
    this.user.username=localStorage.UserName;
    this.getUserInfo()
  }
}
</script>
<style>
.header {
  padding: 0px;
  line-height: 50px;
  vertical-align: middle;
  border-bottom: 1px solid #72bff3;
}
.breadcrumb-inner {
  margin-left: 0px;
  line-height: 42px;
  
}

.main {
  background-color: #f0f0f0;
  padding-left: 0px;
  padding-top: 10px;
  padding-right: 10px;
  padding-bottom: 5px;
  margin: 0px;
  height: 100%
}
.use-image {
  height: 30px; 
  width: 30px;
  margin-top: 10px;
  margin-right: 0px;
}
.container{
  height: 100%; 
  width: 100%; 
  margin: 0px; 
  padding: 0px 0px 0px 10px; 
  background-color: #f0f0f0
}

.aside {
  background-color: #545c64;
}
.header-main {
  vertical-align: middle;
  padding: 0px;
}

.el-col-header{
  margin-left: 0px;
  line-height: 42px;
  font-weight:bold;
  color: red
}
</style>
