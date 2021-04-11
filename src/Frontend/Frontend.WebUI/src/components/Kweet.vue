<template>
    <el-card class="box-card">
        <el-row>
            <el-col :span="2">
                <div class="grid-content">
                    <div class="block">
                        <el-avatar :size="50" :src="kweet.avatar" />
                    </div>
                </div>
            </el-col>
            <el-col :span="20">
                <div class="grid-content">
                    {{ kweet.message }}
                </div>
            </el-col>
            <el-col :span="2">
                <img class="like" v-show="kweet.liked" src="/assets/heart icon fill.png" />
                <img class="like" v-show="!kweet.liked" src="/assets/heart icon.png"  />
            </el-col>
        </el-row>
    </el-card>
</template>

<script lang="ts">
    import { defineComponent, PropType } from 'vue';
    import { Kweet } from '@/modules/Kweet/Kweet';

    export default defineComponent({
        name: 'Kweet',
        props: {
            kweet: Object as PropType<Kweet>
        },
        // TODO: Actions for liking and unliking
        // TODO: Clickable link to profile using user id?
    });
</script>

<style scoped>
    .like {
        max-width: 50px;
        max-height: 50px;
    }
    .box-card {
        min-width: 480px;
        max-height: 100px!important;
        height: 100px!important;
    }
    .grid-content {
        border-radius: 4px;
        min-height: 36px;
    }
</style>
