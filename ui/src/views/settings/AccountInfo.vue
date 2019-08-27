<template>
  <el-tabs value="first" type="border-card">
    <el-tab-pane label="账户信息" name="first">
      <el-form :model="user" label-width="120px" style="width:400px;">
        <el-form-item label="用户名：" prop="UserName">{{user.UserName}}</el-form-item>
        <el-form-item label="邮箱：" prop="Email">{{user.EMail}}</el-form-item>
        <el-form-item label="手机：" prop="MobilePhone">{{user.MobilePhone}}</el-form-item>
        <el-form-item label="公司名称：" prop="Company">{{user.Company}}</el-form-item>
        <el-form-item label="注册时间：" prop="RegDate">{{formatDate(user.RegDate)}}</el-form-item>
      </el-form>
    </el-tab-pane>
  </el-tabs>
</template>
<script>
import { throws } from "assert";
export default {
  name: "AccountInfo",
  data: function() {
    return {
      loading: false,
      user: {
        UserName: "",
        EMail: "",
        MobilePhone: "",
        Company: "",
        RegDate: ""
      }
    };
  },
  methods: {
    formatDate: function(dt) {
      var date = new Date(dt);
      return (
        date.getFullYear() +
        "年" +
        (date.getMonth() + 1) +
        "月" +
        date.getDate() +
        "日"
      );
    },
    getUserInfo: function() {
      this.$http
        .get("api/Login/userinfo?userName=" + this.user.UserName)
        .then(res => {
          if (res.data.Id == 0) {
            this.$router.push("/Login");
          } else {
            this.user.EMail = res.data.EMail;
            this.user.MobilePhone = res.data.MobilePhone;
            this.user.Company = res.data.Company;
            this.user.RegDate = res.data.RegDate;
          }
        });
    }
  },
  mounted: function() {
    this.user.UserName = localStorage.UserName;
    this.getUserInfo();
  }
};
</script>
