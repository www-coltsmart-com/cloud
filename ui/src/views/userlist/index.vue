<template>
  <section>
    <el-col :span="24" class="toolbar">
      <el-form :inline="true" :model="filter">
        <el-form-item>
          <el-input v-model="filter.UserName" placeholder="用户名称"></el-input>
        </el-form-item>
        <el-form-item>
          <el-button
            type="primary"
            icon="el-icon-search"
            v-on:click="queryUsers"
            :loading="table.loading"
          >查询</el-button>
        </el-form-item>

        <el-form-item>
          <el-button type="primary" icon="el-icon-edit" @click="addItem">新增</el-button>
        </el-form-item>
      </el-form>
    </el-col>

    <el-table
      :data="table.users"
      highlight-current-row
      :loading="table.loading"
      style="width: 100%"
      align="left"
    >
      <el-table-column prop="UserNo" label="用户编码" width="120" sortable></el-table-column>
      <el-table-column prop="UserName" label="用户名称" width="120" sortable></el-table-column>
      <el-table-column prop="RegEmall" label="注册邮箱" width="150" sortable></el-table-column>
      <el-table-column prop="RegDate" label="注册日期" width="120" :formatter="dateFormatter" sortable></el-table-column>
      <el-table-column prop="MobilePhone" label="手机号码" width="100" sortable></el-table-column>
      <el-table-column prop="DevCount" label="设备数目" width="100" sortable></el-table-column>
      <el-table-column label="操作" width="150">
        <template slot-scope="scope">
          <el-button
            size="mini"
            type="primary"
            icon="el-icon-edit-outline"
            @click="editItem(scope.row)"
          ></el-button>
          <el-button size="mini" type="danger" icon="el-icon-delete" @click="deleteItem(scope.row)"></el-button>
        </template>
      </el-table-column>
    </el-table>

    <el-col :span="24" class="pagination">
      <el-pagination
        background
        layout="total, prev, pager, next, jumper, slot"
        @current-change="handleCurrentChange"
        :current-page.sync="page.index"
        :page-size="page.size"
        :total="page.total"
      ></el-pagination>
    </el-col>

    <!--新增界面-->
    <el-dialog :title="add.title" :visible.sync="add.visible" :close-on-click-modal="false">
      <el-form :model="add.data" label-width="100px" :rules="add.rules" ref="addForm">
        <el-form-item label="用户编码：" prop="UserNo">
          <el-input v-model="add.data.UserNo" auto-complete="off"></el-input>
        </el-form-item>
        <el-form-item label="用户名称：" prop="UserName">
          <el-input v-model="add.data.UserName" auto-complete="off"></el-input>
        </el-form-item>
        <el-form-item label="邮箱：" prop="RegEmall">
          <el-input v-model="add.data.RegEmall" auto-complete="off"></el-input>
        </el-form-item>
        <el-form-item label="手机号码：" prop="MobilePhone">
          <el-input v-model="add.data.MobilePhone" auto-complete="off"></el-input>
        </el-form-item>
      </el-form>
      <div slot="footer" class="dialog-footer">
        <el-button @click.native="add.visible = false">取消</el-button>
        <el-button type="primary" @click.native="addSubmit" :loading="add.loading">提交</el-button>
      </div>
    </el-dialog>
  </section>
</template>
<script>
export default {
  name: "UserList",
  data: function() {
    let checkEMail = (rule, value, callback) => {
      if (!value) {
        return callback();
      }
      if (value) {
        setTimeout(() => {
          var reg = /^([a-zA-Z0-9]+[_|\_|\.]?)*[a-zA-Z0-9]+@([a-zA-Z0-9]+[_|\_|\.]?)*[a-zA-Z0-9]+\.[a-zA-Z]{2,3}$/;
          if (!reg.test(value)) {
            callback(new Error("请输入有效的电子邮箱！"));
          } else {
            callback();
          }
        }, 500);
      }
    };
    return {
      filter: {
        UserName: ""
      },
      page: {
        index: 1,
        size: 10,
        total: 0
      },
      table: {
        users: [],
        multipleSelection: [],
        loading: false
      },
      add: {
        title: "新增用户",
        visible: false,
        loading: false,
        rules: {
          UserNo: [
            { required: true, message: "请输入用户编码！", trigger: "blur" }
          ],
          UserName: [
            { required: true, message: "请输入用户名称！", trigger: "blur" }
          ],
          RegEmall: [{ required: false, trigger: "blur" }],
          MobilePhone: [{ required: false, trigger: "blur" }]
        },
        data: {
          Id: 0,
          UserNo: "",
          UserName: "",
          RegEmall: "",
          MobilePhone: ""
        }
      }
    };
  },
  methods: {
    dateFormatter: function(row, column, cellValue) {
      var dt = new Date(cellValue);
      return dt.getFullYear() + "-" + (dt.getMonth() + 1) + "-" + dt.getDate();
    },
    addItem: function() {
      this.add.title = "新增用户";
      this.add.visible = true;
      this.add.data = {
        Id: 0,
        UserNo: "",
        UserName: "",
        RegEmall: "",
        MobilePhone: ""
      };
    },
    editItem: function(row) {
      this.table.loading = true;
      this.$http
        .get("api/users/" + row.Id)
        .then(res => {
          this.table.loading = false;
          this.add.title = "修改用户";
          this.add.visible = true;
          this.add.data = res.data;
        })
        .catch(error => {
          this.table.loading = false;
          if (error.response.status == 400) {
            this.$message({
              message: "请选择一个用户",
              type: "error"
            });
          } else if (error.response.status == 404) {
            this.$message({
              message: "用户不存在",
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
    deleteItem: function(row) {
      this.$confirm("确认删除该用户吗？", "提示", { type: "warning" }).then(
        () => {
          this.table.loading = true;
          this.$http
            .delete("/api/users/" + row.id)
            .then(res => {
              this.table.loading = false;
              this.$message("删除成功");
              this.getUsers();
            })
            .catch(error => {
              this.table.loading = false;
              if (error.response.status == 400) {
                this.$message({
                  message: "记录无效，删除失败",
                  type: "error"
                });
              } else if (error.response.status == 500) {
                this.$message({
                  message: "删除失败，请重新尝试",
                  type: "error"
                });
              } else {
                this.$message({
                  message: error.message,
                  type: "error"
                });
              }
            });
        }
      );
    },
    handleCurrentChange(val) {
      this.getUsers();
    },
    handleSelectionChange(val) {
      this.table.multipleSelection = val;
    },
    queryUsers() {
      this.page.index = 1;
      this.getUsers();
    },
    getUsers() {
      this.table.loading = true;
      this.$http
        .get("/api/users/", {
          params: {
            page: this.page.index,
            size: this.page.size,
            userName: this.filter.UserName
          }
        })
        .then(res => {
          this.table.loading = false;
          if (res.data) {
            this.table.users = res.data.Result;
            this.page.total = res.data.TotalCount;
          }
        })
        .catch(error => {
          this.table.loading = false;
          this.error(error.message);
        });
    },
    //新增
    addSubmit: function() {
      this.$refs["addForm"].validate(valid => {
        if (valid) {
          this.$confirm("请检查用户信息是否完整", "提示", {}).then(() => {
            this.add.loading = true;
            this.$http
              .post("/api/users/", this.add.data)
              .then(res => {
                this.add.loading = false;
                this.$message({ message: "保存成功", type: "success" });
                this.$refs["addForm"].resetFields();
                this.add.visible = false;
                this.getUsers();
              })
              .catch(error => {
                this.add.loading = false;
                if (error.response.status == 400) {
                  this.$message({
                    message: "用户信息不完整，请重新填写",
                    type: "error"
                  });
                } else if (error.response.status == 500) {
                  this.$message({
                    message: "保存失败，请重新尝试",
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
        }
      });
    }
  },
  mounted: function() {
    this.getUsers();
  }
};
</script>
