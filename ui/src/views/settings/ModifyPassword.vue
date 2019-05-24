<template>
 <el-tabs value="first" type="border-card">
    <el-tab-pane label="修改密码" name="first">
      
    <el-form :model="user" label-width="80px" style="width:400px;" :rules="rules" ref="editForm">
        <el-form-item label="用户民" prop="UserName">
            <el-input v-model="user.UserName" auto-complete="off" disabled></el-input>
        </el-form-item>
        <el-form-item label="密码" prop="Password">
            <el-input :type="PassEye" v-model="user.Password" auto-complete="off">
                <i slot="suffix" class="el-icon-view" @mousedown="PassEye= ''" @mouseup="PassEye='password'"></i>
            </el-input>
        </el-form-item>	
        <el-form-item label="新密码" prop="NewPassword">
            <el-input :type="NewPassEye" v-model="user.NewPassword" auto-complete="off">          
                <i slot="suffix" class="el-icon-view" @mousedown="NewPassEye= ''" @mouseup="NewPassEye='password'"></i>
            </el-input>
        </el-form-item>	
        <el-form-item>
            <el-button type="primary" @click="submitForm()" :loading="loading">立即保存</el-button>
        </el-form-item>
    </el-form>
    </el-tab-pane>
  </el-tabs>
</template>
<script>
export default {
    name: 'ModifyPassword',
    data: function () {
        return {
            loading: false,
            user: {
                UserName: '',
                Password: '',
                NewPassword: ''
            },
            rules: {  		
                Password: [
                    { required: true, message: '请输入密码', trigger: 'blur' }
                ],		
                NewPassword: [
                    { required: true, message: '请输入新密码', trigger: 'blur' }
                ]
            },
            PassEye: 'password',
            NewPassEye: 'password'
        }
    },
    methods: {
        submitForm: function () {
            this.$refs['editForm'].validate((valid) => {
				if (valid) {
					this.$confirm('确认提交吗？', '提示', {}).then(() => {
						this.loading = true
						this.$http.post('/api/login/modifypassword', this.user)
							.then((res) => {
                                this.loading = false
                                if (res.data.Type == 'Error') {
                                    this.$message({
                                        message: res.data.Result,
                                        type: 'error',
                                        duration: 0,
                                        showClose: true
                                    })
                                }
                                else {
                                    this.$message({
                                        message: '保存成功',
                                        type: 'success'
                                    })
                                }

							}).catch((error) => {
								this.loading = false
								this.$message({
                                    message: error.message,
                                    type: 'error',
                                    duration: 0,
                                    showClose: true
                                })
							})
					})
				}
			})
        }
    },
    mounted: function () {
        this.user.UserName = localStorage.UserName
    }
}
</script>
