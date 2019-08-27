<template>
  <el-tabs value="first" type="border-card">
    <el-tab-pane label="修改密码" name="first">
      <el-form :model="user" label-width="80px" style="width:400px;" :rules="rules" ref="editForm">
        <el-form-item label="用户名" prop="UserNo">
          <el-input v-model="user.UserNo" auto-complete="off" disabled></el-input>
        </el-form-item>
        <el-form-item label="原密码" prop="Password">
          <el-input :type="PassEye" v-model="user.Password" auto-complete="off">
            <i
              slot="suffix"
              class="el-icon-view"
              @mousedown="PassEye= ''"
              @mouseup="PassEye='password'"
            ></i>
          </el-input>
        </el-form-item>
        <el-form-item label="新密码" prop="NewPassword">
          <el-input :type="NewPassEye" v-model="user.NewPassword" auto-complete="off">
            <i
              slot="suffix"
              class="el-icon-view"
              @mousedown="NewPassEye= ''"
              @mouseup="NewPassEye='password'"
            ></i>
          </el-input>
        </el-form-item>
        <el-form-item>
          <el-button type="primary" @click="submitForm()" :loading="loading">修改密码</el-button>
        </el-form-item>
      </el-form>
    </el-tab-pane>
  </el-tabs>
</template>
<script>
export default {
  name: "ModifyPassword",
  data: function() {
    return {
      loading: false,
      user: {
        UserNo: "",
        Password: "",
        NewPassword: ""
      },
      rules: {
        Password: [
          { required: true, message: "请输入原密码", trigger: "blur" }
        ],
        NewPassword: [
          { required: true, message: "请输入新密码", trigger: "blur" }
        ]
      },
      PassEye: "password",
      NewPassEye: "password"
    };
  },
  methods: {
    submitForm: function() {
      this.$refs["editForm"].validate(valid => {
        if (valid) {
          this.$confirm("请确认是否修改原密码吗？", "提示", {}).then(() => {
            this.loading = true;
            this.getPublicKey(publickey => {
              var encrypt = new JSEncrypt();
              encrypt.setPublicKey(publickey);

              var data = {
                UserNo: this.user.UserNo,
                Password: encrypt.encrypt(this.user.Password),
                NewPassword: encrypt.encrypt(this.user.NewPassword)
              };
              this.$http
                .post("/api/login/modifypassword", data)
                .then(res => {
                  this.loading = false;
                  this.$message({
                    message: "密码已被修改，请牢记新密码！",
                    type: "success"
                  });
                  setTimeout(() => {
                    this.$router.push("/home");
                  }, 2000);
                })
                .catch(error => {
                  this.loading = false;
                  if (error.response.status == 400) {
                    this.$message({
                      message: "您输入的密码无效，请检查原密码和新密码是否正确",
                      type: "error"
                    });
                  } else if (error.response.status == 500) {
                    this.$message({
                      message: "密码修改失败，请重新尝试",
                      type: "error"
                    });
                  } else {
                    this.$message({
                      message: error.message,
                      type: "error"
                    });
                  }
                });
            });
          });
        }
      });
    },
    getPublicKey: function(callback) {
      this.$http
        .get("/api/login/getpublickey")
        .then(res => {
          if (callback) callback(res.data);
        })
        .catch(error => {
          this.$message({
            message: error.message,
            type: "error"
          });
        });
    }
  },
  mounted: function() {
    this.user.UserNo = localStorage.UserName;
  }
};
</script>
