<template>
  <section>
    <el-col :span="24" class="toolbar">
      <el-form :inline="true" :model="filter">
        <el-form-item>
          <el-input v-model="filter.DeviceId" placeholder="序列号"></el-input>
        </el-form-item>
        <el-form-item>
          <el-input v-model="filter.DeviceName" placeholder="设备名称"></el-input>
        </el-form-item>
        <el-form-item>
          <el-button
            type="primary"
            icon="el-icon-search"
            v-on:click="queryDevices"
            :loading="table.loading"
          >查询</el-button>
        </el-form-item>
      </el-form>
    </el-col>

    <el-table
      :data="table.devices"
      highlight-current-row
      :loading="table.loading"
      style="width: 100%"
      align="left"
    >
      <el-table-column prop="DeviceId" label="序列号" width="120" sortable></el-table-column>
      <el-table-column prop="DeviceType" label="型号" width="90" sortable></el-table-column>
      <el-table-column prop="IsGetway" label="网关" width="80" sortable></el-table-column>
      <el-table-column prop="DeviceName" label="名称" width="100" sortable></el-table-column>
      <el-table-column label="状态" width="60" sortable>
        <template slot-scope="scope">
          <span v-if="scope.row.IsOnline" style="color:green">在线</span>
          <span v-if="!scope.row.IsOnline" style="color:red">离线</span>
        </template>
      </el-table-column>
      <el-table-column prop="InDate" label="接入日期" width="100" :formatter="dateFormatter" sortable></el-table-column>
      <el-table-column prop="UserOwn" label="使用人" width="90" sortable></el-table-column>
      <el-table-column label="删除" width="80">
        <template slot-scope="scope">
          <el-button size="mini" type="danger" icon="el-icon-delete" @click="handleDel(scope.row)"></el-button>
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
  </section>
</template>


<script>
export default {
  name: "DeviceList",
  data: function() {
    return {
      filter: {
        DeviceId: "",
        DeviceName: ""
      },
      page: {
        index: 1,
        size: 10,
        total: 0
      },
      table: {
        devices: [],
        multipleSelection: [],
        loading: false
      }
    };
  },
  methods: {
    dateFormatter: function(row, column, cellValue) {
      var dt = new Date(cellValue);
      return dt.getFullYear() + "-" + (dt.getMonth() + 1) + "-" + dt.getDate();
    },
    handleDel: function(row) {
      this.$confirm("确认删除该设备吗？", "提示", { type: "warning" })
        .then(() => {
          this.table.loading = true;
          this.$http
            .delete("/api/devices/" + row.id)
            .then(res => {
              this.table.loading = false;
              this.$message({
                message: "删除成功!",
                type: "success"
              });
              this.getDevices();
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
        })
        .catch(() => {});
    },
    handleCurrentChange(val) {
      this.getDevices();
    },
    handleSelectionChange(val) {
      this.table.multipleSelection = val;
    },
    queryDevices() {
      this.page.index = 1;
      this.getDevices();
    },
    getDevices() {
      this.table.loading = true;
      this.$http
        .get("/api/devices/", {
          params: {
            page: this.page.index,
            size: this.page.size,
            userNo: localStorage.UserName,
            deviceId: this.filter.DeviceId,
            deviceName: this.filter.DeviceName
          }
        })
        .then(res => {
          this.table.loading = false;
          if (res.data) {
            this.table.devices = res.data.Result;
            this.page.total = res.data.TotalCount;
          }
        })
        .catch(error => {
          this.table.loading = false;
          this.error(error.message);
        });
    }
  },
  mounted: function() {
    this.getDevices();
  }
};
</script>
