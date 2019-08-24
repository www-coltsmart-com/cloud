<template>
  <section>
    <div class="login">
      <el-row>
        <el-col class="login-logo" :offset="1" :span="2">
          <img src="../assets/logo.png" width="200px" />
        </el-col>
      </el-row>
      <el-row
        class="login-center"
        type="flex"
        :style="'background: url(' + require('../assets/bg.png') + ') no-repeat scroll center center / cover;' "
        justify="center"
      >
        <el-col :xs="12" :sm="10" :md="8" :lg="8" :xl="6">
          <div style="height:80px;" class="hidden-lg-and-down"></div>
          <div style="height:80px;"></div>
          <el-card class="login-box">
            <el-form :model="LoginUser" ref="LoginUser" :rules="LoginRules" status-icon>
              <h1 class="title">欢迎登录</h1>
              <p class="login-box-msg"></p>
              <el-form-item>
                <el-input
                  type="text"
                  v-model="LoginUser.UserName"
                  auto-complete="off"
                  placeholder="请输入用户名"
                  ref="uname"
                  @keyup.enter.native="jumponenter($event)"
                  prefix-icon="el-icon-user-solid"
                ></el-input>
              </el-form-item>
              <el-form-item>
                <el-input
                  :type="PassEye"
                  v-model="LoginUser.Password"
                  auto-complete="off"
                  placeholder="请输入密码"
                  ref="pwd"
                  @keyup.enter.native="jumponenter($event)"
                  prefix-icon="el-icon-edit"
                >
                  <i
                    slot="suffix"
                    class="el-icon-view"
                    @mousedown="PassEye= ''"
                    @mouseup="PassEye='password'"
                  ></i>
                </el-input>
              </el-form-item>
              <el-form-item>
                <el-button
                  type="primary"
                  @click="submitForm"
                  :loading="loading"
                  class="pull-right"
                  style="width:100%"
                  ref="login"
                >登录</el-button>
                <hr />
                <p>
                  还没有账号，现在就去
                  <el-link type="primary" @click="redirect">注册</el-link>吧
                  <br />忘记密码了，申请
                  <el-link type="danger" @click="resetPassword">重置</el-link>密码
                </p>
              </el-form-item>
            </el-form>
          </el-card>
          <div style="height:50px;"></div>
          <div style="height:80px;" class="hidden-lg-and-down"></div>
        </el-col>
      </el-row>
      <el-row type="flex" justify="center">
        <el-col :span="12">
          <div class="login-bottom">
            <span>©&nbsp;2019 &nbsp;&nbsp; 上海市鸣驹智能科技有限公司 &nbsp;&nbsp; 技术支持</span>
          </div>
        </el-col>
      </el-row>
    </div>
    <!--密码重置弹出窗-->
    <el-dialog title="密码重置" :visible.sync="ResetPassword.visible" :close-on-click-modal="false">
      <el-form
        :model="ResetPassword.data"
        label-width="100px"
        :rules="ResetPassword.rules"
        ref="ResetPasswordForm"
      >
        <el-form-item label="用户名" prop="uname">
          <el-input
            type="text"
            v-model="ResetPassword.data.uname"
            auto-complete="off"
            placeholder="您的用户名"
          ></el-input>
        </el-form-item>
        <el-form-item label="电子邮箱" prop="email">
          <el-input
            type="text"
            v-model="ResetPassword.data.email"
            auto-complete="off"
            placeholder="您注册时所使用的邮箱"
          >
            <el-button
              v-if="ResetPassword.enble_verify_code==0"
              slot="suffix"
              type="text"
              disabled
            >发送验证码</el-button>
            <el-button
              v-if="ResetPassword.enble_verify_code==1"
              slot="suffix"
              type="text"
              @click="sendVerifyCode"
            >发送验证码</el-button>
            <el-button
              v-if="ResetPassword.enble_verify_code==2"
              slot="suffix"
              type="text"
              disabled
            >{{ResetPassword.verify_time}}秒后重新发送</el-button>
          </el-input>
        </el-form-item>
        <el-form-item label="验证码" prop="verify_code">
          <el-input
            type="text"
            v-model="ResetPassword.data.verify_code"
            auto-complete="off"
            placeholder="验证码"
          ></el-input>
        </el-form-item>
      </el-form>
      <div slot="footer" class="dialog-footer">
        <el-button @click.native="ResetPassword.visible = false">取消</el-button>
        <el-button type="primary" @click.native="ResetSubmit" :loading="ResetPassword.loading">重置</el-button>
      </div>
    </el-dialog>
  </section>
</template>

<script>
import JsEncrypt from "jsencrypt";
import "element-ui/lib/theme-chalk/display.css";

export default {
  name: "login",
  data: function() {
    const validateUsername = (rule, value, callback) => {
      if (value.length <= 0) {
        callback(new Error("请输入正确的用户名"));
      } else {
        callback();
      }
    };
    const validatePass = (rule, value, callback) => {
      if (value.length < 5) {
        callback(new Error("密码不能小于5位"));
      } else {
        callback();
      }
    };
    const validateEmail = (rule, value, callback) => {
      if (value.length <= 0) {
        this.ResetPassword.enble_verify_code = 0;
        callback(new Error("请输入邮箱地址"));
      } else {
        this.ResetPassword.enble_verify_code = 1;
        callback();
      }
    };
    return {
      LoginUser: {
        UserName: "",
        Password: ""
      },
      LoginRules: {
        UserName: [
          { required: true, trigger: "blur", validator: validateUsername }
        ],
        Password: [{ required: true, trigger: "blur", validator: validatePass }]
      },
      loading: false,
      checked: true,
      PassEye: "password",
      ResetPassword: {
        visible: false,
        loading: false,
        enble_verify_code: 0,
        verify_time: 60,
        rules: {
          uname: [{ required: true, message: "请输入用户名", trigger: "blur" }],
          email: [
            {
              required: true,
              trigger: ["blur", "change"],
              validator: validateEmail
            },
            { type: "email", message: "请输入正确的邮箱地址", trigger: "blur" }
          ],
          verify_code: [
            { required: true, message: "请输入验证码", trigger: "blur" }
          ]
        },
        data: {
          uname: "",
          email: "",
          verify_code: ""
        }
      }
    };
  },
  methods: {
    submitForm() {
      var that = this;
      this.$refs["LoginUser"].validate(valid => {
        if (valid) {
          this.loading = true;
          this.getPublicKey(publickey => {
            var encrypt = new JSEncrypt();
            encrypt.setPublicKey(publickey);

            var user = {
              UserName: this.LoginUser.UserName,
              Password: encrypt.encrypt(this.LoginUser.Password)
            };
            this.$http
              .post("/api/login", user, { rsa: true })
              .then(res => {
                // 登录成功
                this.loading = false;
                if (res.data) {
                  // 储存 token
                  //token.set(res.data)
                  that.$router.push({ path: "/home" });
                }
                localStorage.UserName = this.LoginUser.UserName;
              })
              .catch(error => {
                this.loading = false;
                if (error.response.status == 401) {
                  this.$message({
                    message: "用户名或密码错误，登录失败1",
                    type: "error"
                  });
                } else if (error.response.status == 405) {
                  this.$message({
                    message: "客户编码为空或不存在，登录失败",
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
        } else {
          return false;
        }
      });
    },
    getPublicKey: function(callback) {
      this.$http
        .get("/api/login/getpublickey")
        .then(res => {
          //localStorage.setItem("publickey", res.data)
          if (callback) callback(res.data);
        })
        .catch(error => {
          this.$message({
            message: error.message,
            type: "error"
          });
        });
    },
    jumponenter: function(e) {
      const phtext = e.target.getAttribute("placeholder");
      if (phtext === "请输入用户名") {
        this.$refs.pwd.focus();
      } else if (phtext === "请输入密码") {
        this.submitForm();
      }
    },
    redirect: function() {
      this.$router.push({ path: "/register" });
    },
    resetPassword: function() {
      this.ResetPassword.visible = true;
      this.ResetPassword.data = {
        uname: "",
        email: "",
        verify_code: ""
      };
    },
    sendVerifyCode: function() {
      var email = this.ResetPassword.data.email;
      if (!email) {
        this.$message({
          message: "邮箱不能为空",
          type: "error"
        });
        return;
      }
      this.ResetPassword.enble_verify_code = 2;
      this.$http
        .get("/api/login/sendverifycode?email=" + email)
        .then(res => {
          this.$message({
            message: "验证码发送成功,请及时查收您的邮箱！",
            type: "success"
          });
          this.ResetPassword.verify_time = 60;
          var handle = setInterval(() => {
            this.ResetPassword.verify_time--;
            if (this.ResetPassword.verify_time <= 0) {
              this.ResetPassword.verify_time = 60;
              this.ResetPassword.enble_verify_code = 1;
              clearInterval(handle);
            }
          }, 1000);
        })
        .catch(error => {
          this.ResetPassword.enble_verify_code = 1;
          if (error.response.status == 400) {
            this.$message({
              message: "邮箱不能为空",
              type: "error"
            });
          } else if (error.response.status == 500) {
            this.$message({
              message: "邮件发送失败，请尝试重新一次",
              type: "error"
            });
          } else {
            this.$message({
              message: error.message,
              type: "error"
            });
          }
        });
    },
    ResetSubmit: function() {
      this.$refs["ResetPasswordForm"].validate(valid => {
        if (valid) {
          this.ResetPassword.loading = true;
          this.$http
            .post("api/Login/resetpassword", {
              UserName: this.ResetPassword.data.uname,
              RegEmall: this.ResetPassword.data.email,
              NewPassword: this.ResetPassword.data.verify_code
            })
            .then(res => {
              this.ResetPassword.loading = false;
              this.$message({
                message: "密码重置成功，请使用系统初始密码重新登录!",
                type: "success"
              });
              this.ResetPassword.visible = false;
            })
            .catch(error => {
              this.ResetPassword.loading = false;
              if (error.response.status == 400) {
                this.$message({
                  message: "用户信息无效，请检查用户名、邮箱等信息是否正确",
                  type: "error"
                });
              } else if (error.response.status == 401) {
                this.$message({
                  message: "验证码不正确，你重新输入",
                  type: "error"
                });
              } else {
                this.$message({
                  message: error.message,
                  type: "error"
                });
              }
            });
        } else {
          return false;
        }
      });
    }
  },
  mounted: function() {
    this.$refs.uname.focus();
  }
};
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
  font-size: 12px;
}
.login-logo {
  margin-top: 40px;
  z-index: 1;
}
</style>