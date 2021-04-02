<template>
    <el-menu :default-active="activeIndex"
             class="el-menu-demo"
             mode="horizontal"
             @select="handleSelect"
             background-color="#545c64"
             text-color="#fff"
             :router="true"
             active-text-color="#ffd04b">
        <el-menu-item :route="{ name: '/' }">
          <div class="cropped">
            <img src="/assets/annoyed-bird.png" />
          </div>
        </el-menu-item>
        <el-menu-item v-for="route in routes" :index="route.to" :route="{ name: route.name }" :key="route.name">
            <i :class="route.icon"></i>{{ route.name }}
        </el-menu-item>
    </el-menu>
</template>

<script>
  export default {
    data() {
      return {
          activeIndex: this.$route.path,
          routes: [
              {
                  to: '/Home',
                  icon: 'el-icon-s-home',
                  name: 'Home'
              },
              {
                  to: '/Alias',
                  icon: 'el-icon-circle-plus-outline',
                  name: 'Alias'
              }
          ]
      };
    },
    methods: {
      handleSelect(key) {
        if (key !== this.$route.path) {
          this.$data.activeIndex = this.$route.path;
        }
      }
    }
  }
</script>

<style>
.cropped {
  max-height: 50px;
  max-width: 50px;
  overflow: hidden;
  object-fit: cover;
}
.cropped img {
  width: 50px;
  margin: -15px 0px 0px 0px;
}
</style>
