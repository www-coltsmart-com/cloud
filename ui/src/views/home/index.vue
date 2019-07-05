<template>
  <section>
    <el-row :gutter="20">
      <el-col :v-show="stats.total_device_display" :span="6" key="total_device">
        <el-card>
          <div>
            <el-row>
              <el-col :span="10">
                <div class="icon-big text-center icon-warning">
                  <i class="el-icon-coin"></i>
                </div>
              </el-col>
              <el-col :span="14">
                <div class="numbers">
                  <p>总设备数</p>
                  {{stats.total_device_count}}
                </div>
              </el-col>
            </el-row>
            <div>
              <hr />
              <div class="info">
                <i class="el-icon-info"></i> 总设备数量
              </div>
            </div>
          </div>
        </el-card>
      </el-col>
      <el-col :v-show="stats.online_device_display" :span="6" key="online_device">
        <el-card>
          <div>
            <el-row>
              <el-col :span="10">
                <div class="icon-big text-center icon-success">
                  <i class="el-icon-link"></i>
                </div>
              </el-col>
              <el-col :span="14">
                <div class="numbers">
                  <p>在线设备数</p>
                  {{stats.online_device_count}}
                </div>
              </el-col>
            </el-row>
            <div>
              <hr />
              <div class="info">
                <i class="el-icon-info"></i> 当前在线的设备数量
              </div>
            </div>
          </div>
        </el-card>
      </el-col>
      <el-col :v-show="stats.total_user_display" :span="6" key="total_user">
        <el-card>
          <div>
            <el-row>
              <el-col :span="10">
                <div class="icon-big text-center icon-info">
                  <i class="el-icon-user"></i>
                </div>
              </el-col>
              <el-col :span="14">
                <div class="numbers">
                  <p>总用户数</p>
                  {{stats.total_user_count}}
                </div>
              </el-col>
            </el-row>
            <div>
              <hr />
              <div class="info">
                <i class="el-icon-info"></i> 用户总数量
              </div>
            </div>
          </div>
        </el-card>
      </el-col>
    </el-row>
  </section>
</template>

<script>
export default {
  name: "HomeIndex",
  data: function() {
    return {
      stats: {
        total_device_count: 0,
        total_device_display: false,
        online_device_count: 0,
        online_device_display: false,
        total_user_count: 0,
        total_user_display: false
      }
    };
  },
  methods: {
    getStatsInfo: function() {
      var user_no = localStorage.UserName;
      this.$http
        .get("api/Login/getstatsinfo?userNo=" + user_no)
        .then(resp => {
          if (resp.data.Type == "Error") {
            throw new Error(resp.data.Result);
          }
          if (resp.data.Result == null) {
            throw new Error("统计无效");
          }
          this.stats = {
            total_device_count: resp.data.Result.TotalDeviceCount,
            total_device_display: resp.data.Result.TotalDeviceDisplay,
            online_device_count: resp.data.Result.OnlineDeviceCount,
            online_device_display: resp.data.Result.OnlineDeviceDisplay,
            total_user_count: resp.data.Result.TotalUserCount,
            total_user_display: resp.data.Result.TotalUserDisplay
          };
        })
        .catch(error => {
          this.$message({
            message: error.message,
            type: "error"
          });
          this.stats = {
            total_device_count: 0,
            total_device_display: false,
            online_device_count: 0,
            online_device_display: false,
            total_user_count: 0,
            total_user_display: false
          };
        });
    }
  },
  mounted: function() {
    //自动加载统计数据
    this.getStatsInfo();
  }
};
</script>
<style>
.el-card .icon-big {
  font-size: 3em;
  min-height: 64px;
}
.el-card .numbers {
  font-size: 2em;
  text-align: right;
}
.el-card .numbers p {
  margin: 0;
  font-size: 16px;
  line-height: 1.4em;
}
.el-card .info {
  color: #a9a9a9;
  font-weight: 300;
  font-size: 14px;
  text-align: left;
}
hr {
  box-sizing: content-box;
  height: 0;
  overflow: visible;
  margin-top: 1rem;
  margin-bottom: 1rem;
  border: 0;
  border-top: 1px solid rgba(0, 0, 0, 0.1);
  border-color: #f1eae0;
}
.icon-primary {
  color: #7a9e9f;
}
.icon-info {
  color: #68b3c8;
}
.icon-success {
  color: #41b883;
}
.icon-warning {
  color: #f3bb45;
}
.icon-danger {
  color: #eb5e28;
}
</style>