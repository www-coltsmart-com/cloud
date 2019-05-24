<template>
  <div id="app">
    <!-- <img src="./assets/logo.png"> -->
    <router-view/>
  </div>
</template>

<script>
export default {
  name: 'App'
}
</script>

<style >
#app {
  font-family: 'Avenir', Helvetica, Arial, sans-serif;
  -webkit-font-smoothing: antialiased;
  -moz-osx-font-smoothing: grayscale;
  text-align: center;
  color: #2c3e50;
  margin-top: 0px;
}

.el-form {
  text-align: left;
}

.toolbar {
	background: #f2f2f2;
	padding: 10px;
	border:1px solid #dfe6ec;
	margin: 10px 0px;
  padding-bottom: 0px;
}

.toolbar > .el-form {
  text-align: left;
}

.toolbar > .el-form > .el-form-item {
  margin-bottom: 10px;
}

.pagination {
	background: #f2f2f2;
	padding: 5px;
	border:1px solid #dfe6ec;
	margin: 10px 0px;
  padding-bottom: 0px;
}

.pagination > .el-pagination { 
  text-align: left;
  margin-bottom: 5px;
}

.fade-enter-active,
.fade-leave-active {
	transition: all .2s ease;
}

.fade-enter,
.fade-leave-active {
	opacity: 0;
}
</style>
