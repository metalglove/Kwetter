<template>
    <el-menu :default-active="activeIndex"
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
        <el-menu-item v-for="route, index in activeRoutes" :index="route.to" :key="route.name">
            <i :class="route.icon"></i>{{ route.name }}
        </el-menu-item>
        <div class="dock-right">
            <profile />
        </div>
    </el-menu>
</template>

<script lang="ts">
    import { defineComponent } from 'vue';
    import Profile from '@/components/Profile.vue';
    import { mapActions, mapGetters } from 'vuex';
    import { UserGetterTypes } from '@/modules/User/User.getters';

    interface KwetterRoute {
        to: string;
        icon: string;
        name: string;
        authRequired: boolean;
    }

    export default defineComponent({
        name: 'TopBar',
        components: {
            Profile
        },
        data() {
            return {
                activeIndex: this.$route.path,
                routes: [
                    {
                        to: '/Home',
                        icon: 'el-icon-s-home',
                        name: 'Home',
                        authRequired: false
                    },
                    {
                        to: '/Timeline',
                        icon: 'el-icon-tickets',
                        name: 'Timeline',
                        authRequired: true
                    }
                ] as KwetterRoute[]
            };
        },
        computed: {
            ...mapGetters('user', [UserGetterTypes.GET_IS_LOGGED_IN]),
            activeRoutes(): KwetterRoute[] {
                const isLoggedIn: boolean = this.GET_IS_LOGGED_IN as boolean;
                if (!isLoggedIn)
                    return this.$data.routes.filter((route) => !route.authRequired);
                return this.$data.routes;
            }
        },
        methods: {
            handleSelect(key: string): void {
                if (key !== this.$route.path) {
                    this.$data.activeIndex = this.$route.path;
                }
            }
        }
    });
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
.dock-right {
  min-height: 50px;
  min-width: 100px;
  float: right;
  margin: 5px!important;
}
</style>
