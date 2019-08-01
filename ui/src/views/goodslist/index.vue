<template>
  <section>
    <el-col :span="24" class="toolbar">
      <el-form :inline="true" :model="filter">
        <el-form-item>
          <el-input v-model="filter.name" placeholder="按产品名称查询"></el-input>
        </el-form-item>
        <el-form-item>
          <el-button
            type="primary"
            icon="el-icon-search"
            @click="search"
            :loading="table.loading"
          >查询</el-button>
        </el-form-item>
        <el-form-item>
          <el-button type="primary" icon="el-icon-edit" @click="addItem" :loading="item.loading">新增</el-button>
        </el-form-item>
      </el-form>
    </el-col>

    <el-table
      :data="table.list"
      highlight-current-row
      :loading="table.loading"
      style="width:100%"
      align="left"
    >
      <el-table-column prop="DisplayOrder" label="序号" width="60" sortable></el-table-column>
      <el-table-column prop="Name" label="名称" width="250" sortable></el-table-column>
      <el-table-column prop="Info" label="简介" width="400"></el-table-column>
      <el-table-column label="状态" width="100">
        <template slot-scope="scope">
          <span v-if="scope.row.Status==0" style="color:blue">未发布</span>
          <span v-if="scope.row.Status==1" style="color:green">已发布</span>
          <span v-if="scope.row.Status<0" style="color:red">其他</span>
          <span v-if="scope.row.Status>1" style="color:red">其他</span>
        </template>
      </el-table-column>
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
        @current-change="page_change"
        :current-page.sync="page.index"
        :page-size="page.size"
        :total="page.total"
      ></el-pagination>
    </el-col>

    <el-dialog
      :title="item.title"
      :visible.sync="item.visible"
      :close-on-click-modal="false"
      width="60%"
    >
      <el-form :model="item.data" :rules="item.rules" ref="form">
        <el-form-item label="产品名称" prop="name" label-width="100px">
          <el-input v-model="item.data.name" auto-complete="off"></el-input>
        </el-form-item>
        <el-form-item label="图片" prop="picture" label-width="100px">
          <el-upload
            class="avatar-uploader"
            :action="uploadURL"
            :show-file-list="false"
            :on-success="onAvatarSuccess"
            :before-upload="beforeAvatarUpload"
          >
            <img v-if="item.data.picture" :src="item.data.picture" class="avatar" />
            <i v-else class="el-icon-plus avatar-uploader-icon"></i>
          </el-upload>
        </el-form-item>
        <el-form-item label="简介" prop="info" label-width="100px">
          <el-input
            type="textarea"
            v-model="item.data.info"
            :autosize="{ minRows: 2, maxRows: 4}"
            placeholder="请简单描述一下产品的基本信息"
            auto-complete="off"
          ></el-input>
        </el-form-item>
        <el-form-item label="显示顺序" prop="displayOrder" label-width="100px">
          <el-input-number v-model="item.data.displayOrder" :min="0" :max="999999" label="显示顺序"></el-input-number>
        </el-form-item>
        <el-tabs v-model="item.activeTabName">
          <el-tab-pane label="详细信息" name="detail">
            <quill-editor
              v-model="item.data.description"
              ref="myQuillEditor"
              class="editer"
              :options="item.editorOption"
            ></quill-editor>
          </el-tab-pane>
          <el-tab-pane label="规格参数" name="paramter">
            <el-button type="primary" size="mini" @click="$refs.editable.insert()">新增</el-button>
            <el-button type="danger" size="mini" @click="$refs.editable.removeSelecteds()">删除选中</el-button>
            <elx-editable ref="editable" :data.sync="item.data.attrs">
              <elx-editable-column type="selection" width="55"></elx-editable-column>
              <elx-editable-column type="index" width="55"></elx-editable-column>
              <elx-editable-column
                prop="name"
                label="规格"
                :edit-render="{name: 'ElInput'}"
                width="150"
              ></elx-editable-column>
              <elx-editable-column
                prop="value"
                label="参数"
                :edit-render="{name: 'ElInput'}"
                width="360"
              ></elx-editable-column>
              <elx-editable-column
                prop="displayOrder"
                label="排序"
                :edit-render="{name: 'ElInputNumber'}"
              ></elx-editable-column>
            </elx-editable>
          </el-tab-pane>
          <el-tab-pane label="附件清单" name="dowload">
            <el-upload
              class="upload-demo"
              :action="uploadURL"
              multiple
              :on-success="onFileSuccess"
              :before-upload="beforeFileUpload"
              :on-change="onFileChange"
              :file-list="item.data.downloads"
            >
              <el-button size="mini" type="primary">上传</el-button>
              <div slot="tip" class="el-upload__tip">上传文件格式不限，文件大小不得超过10M</div>
            </el-upload>
          </el-tab-pane>
        </el-tabs>
      </el-form>
      <div slot="footer" class="dialog-footer">
        <el-button @click="item.visible = false">取消</el-button>
        <el-button type="primary" @click="saveItem" :loading="item.loading">保存</el-button>
      </div>
    </el-dialog>
  </section>
</template>

<script>
export default {
  name: "GoodsList",
  data: function() {
    return {
      filter: {
        name: ""
      },
      page: {
        index: 1,
        size: 10,
        total: 0
      },
      table: {
        list: [],
        loading: false
      },
      item: {
        title: "新增产品信息",
        visible: false,
        loading: false,
        rules: {
          name: [{ required: true, message: "请输入名称", trigger: "blur" }]
        },
        data: {
          id: 0,
          name: "",
          picture: "",
          info: "",
          description: "",
          displayOrder: 0,
          attrs: [],
          downloads: []
        },
        activeTabName: "detail",
        editorOption: {
          placeholder: "请在这里描述一下产品的具体细节",
          modules: {}
        }
      }
    };
  },
  methods: {
    search: function() {
      this.page.index = 1;
      this.query();
    },
    query: function() {
      this.table.loading = true;
      this.$http
        .get("/api/goods", {
          params: {
            page: this.page.index,
            size: this.page.size,
            name: this.filter.name
          }
        })
        .then(rep => {
          this.table.loading = false;
          if (rep.data) {
            this.table.list = rep.data.Result;
            this.page.total = rep.data.TotalCount;
          } else {
            this.table.list = [];
            this.page.total = 0;
          }
        })
        .catch(err => {
          this.table.loading = false;
          this.error(err.message);
        });
    },
    page_change: function(val) {
      this.query();
    },
    deleteItem: function(row) {
      this.$confirm("确认是否要删除该记录吗？", "提示", {
        type: "warning"
      }).then(() => {
        this.table.loading = true;
        this.$http
          .delete("/api/goods/" + row.id)
          .then(() => {
            this.table.loading = false;
            this.$message("删除成功");
            this.search();
          })
          .catch(error => {
            this.table.loading = false;
            this.$message({
              message: error.message,
              type: "error",
              duration: 0,
              showClose: true
            });
          });
      });
    },
    editItem: function(row) {
      this.table.loading = true;
      this.$http
        .get("api/goods/" + row.id)
        .then(res => {
          if (res.data.Type == "Success") {
            this.table.loading = false;
            this.item.title = "修改产品信息";
            this.item.visible = true;
            this.item.data = res.data.Result;
          } else {
            this.table.loading = false;
            this.$message({
              message: res.data.Result,
              type: "error",
              duration: 0,
              showClose: true
            });
          }
        })
        .catch(error => {
          this.table.loading = false;
          this.$message({
            message: error.message,
            type: "error",
            duration: 0,
            showClose: true
          });
        });
    },
    addItem: function() {
      this.item.loading = true;
      this.item.visible = true;
      this.item.title = "新增产品信息";
      this.item.data = {
        id: 0,
        name: "",
        picture: "",
        info: "",
        description: "",
        displayOrder: 0,
        attrs: [],
        downloads: []
      };
      this.item.loading = false;
    },
    saveItem: function() {
      console.log(this.item.data);
      this.item.loading = true;
      this.$http
        .post("api/goods", this.item.data)
        .then(res => {
          this.$message({ message: "保存成功", type: "success" });
          this.$refs["form"].resetFields();
          this.item.visible = false;
          this.item.loading = false;
          this.query();
        })
        .catch(error => {
          this.item.loading = false;
          this.$message({
            message: error.message,
            type: "error",
            duration: 0,
            showClose: true
          });
        });
    },
    onAvatarSuccess(res, file) {
      this.item.data.picture = URL.createObjectURL(file.raw);
    },
    beforeAvatarUpload(file) {
      const isLt2M = file.size / 1024 / 1024 < 10;

      if (!isLt2M) {
        this.$message.error("上传图片大小不能超过 10MB!");
      }
      return isLt2M;
    },
    onFileSuccess(response, file, fileList) {
      file.url = URL.createObjectURL(file.raw);
    },
    beforeFileUpload(file) {
      const isLt2M = file.size / 1024 / 1024 < 25;

      if (!isLt2M) {
        this.$message.error("上传大小不能超过 25MB!");
      }
      return isLt2M;
    },
    onFileChange(file, fileList) {
      this.item.data.downloads = fileList;
    }
  },
  computed: {
    editor() {
      return this.$refs.myQuillEditor.quillEditor;
    },
    serverURL() {
      return process.env.BASE_URL + "/";
    },
    uploadURL() {
      return process.env.BASE_URL + "/api/uploadfile";
    }
  },
  mounted: function() {
    this.query();
  }
};
</script>
<style>
.avatar-uploader .el-upload {
  border: 1px dashed #d9d9d9;
  border-radius: 6px;
  cursor: pointer;
  position: relative;
  overflow: hidden;
}
.avatar-uploader .el-upload:hover {
  border-color: #409eff;
}
.avatar-uploader-icon {
  font-size: 28px;
  color: #8c939d;
  width: 80px;
  height: 80px;
  line-height: 80px;
  text-align: center;
}
.avatar {
  width: 80px;
  height: 80px;
  display: block;
}
.el-tab-pane {
  height: 300px;
}
.ql-editor.ql-blank,
.ql-editor {
  height: 250px;
}
</style>