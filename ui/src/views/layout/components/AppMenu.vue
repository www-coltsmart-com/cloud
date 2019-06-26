<template>
    <section>
    <div style="display: flex; height: 60px;">
      <transition name="el-zoom-in-center">
        <div v-show="!collapse" class="transition-box">
              <img src="../../../assets/coltsmart.png" class="top-logo"
                  style="height:24px;width:120px"/>
        </div>
      </transition>
    </div>
    <el-menu
    :collapse="collapse"
    :default-active="activeMenu"
    class="el-menu-vertical-demo"
    background-color="#545c64"
    text-color="#fff"
    active-text-color="#ffd04b"
    :router="isrouter" >
    <template v-for="route in routes">
        <template v-if="route.children && route.children.filter(f=>!f.hidden).length > 0 && route.name">
            <el-submenu :index="route.path" :key="route.name">
                <template slot="title"><i :class="route.iconClass"></i>{{route.name}}</template>
                <template v-for="croute in route.children.filter(f=>!f.hidden)"> 
                    <el-menu-item :index="croute.path" :key="croute.name" :route="croute">{{croute.name}}</el-menu-item>
                </template>
            </el-submenu>
        </template>
        <template v-if="route.children && route.children.filter(f=>!f.hidden).length == 0 ">
            <el-menu-item :index="route.path" :key="route.name" :route="route">
                <i :class="route.icon"></i>
                <span slot="title">{{route.name}}</span>
            </el-menu-item>
        </template>
        <template v-if="!route.children && route.name && !route.hidden">
            <el-menu-item :index="route.path" :key="route.name" :route="route">               
                <i :class="route.icon"></i>
                <span slot="title">{{route.name}}</span>
            </el-menu-item>
        </template>
    </template>
    </el-menu>
    </section>
</template>
<script>
export default {
  name: 'AppMenu',
  props: ['collapse'],
  data() {
    return {
        isrouter: true,
        activeMenu:""
    }
  },
  computed: {
    routes() {
      return this.$router.options.routes[this.$router.options.routes.length-1].children
    },
  },
  mounted: function () {
      this.activeMenu = '/home'
      this.$router.afterEach((to, from) => {
         this.activeMenu = to.path
        })
  }

}
</script>
<style>
.el-menu{
    border: 0px;
}
.el-menu-vertical-demo {
    text-align: left;
}
.el-menu-vertical-demo:not(.el-menu--collapse) {
    width: 200px;
    min-height: 400px;
}
.el-menu-item{
    background-color: #545c64;
}
.transition-box {
    margin-bottom: 0px;
    width: 200px;
    height: 60px;
    border-radius: 4px;
    /* background-color: #409EFF; */
    text-align: center;
    color: #fff;
    padding: 0px 0px;
    box-sizing: border-box;
    margin-right: 0px;
  }
.top-logo{
    margin-top: 20px;
}
</style>

