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
                <img class="like" v-show="kweet.liked" @click="unlike" src="/assets/heart icon fill.png" />
                <img class="like" v-show="!kweet.liked" @click="like" src="/assets/heart icon.png"  />
            </el-col>
        </el-row>
    </el-card>
</template>

<script lang="ts">
    import { defineComponent, PropType } from 'vue';
    import { Kweet } from '@/modules/Kweet/Kweet';
    import Response from '@/models/cqrs/Response';
    import { ElMessage } from "element-plus";

    export default defineComponent({
        name: 'Kweet',
        props: {
            kweet: Object as PropType<Kweet>
        },
        methods: {
            async unlike() {
                this.$props.kweet!.liked = false;
                const kweetId: string = this.$props.kweet!.id;
                const userId: string = this.$props.kweet!.userId;
                const response: Response = await this.$kweetService.unlikeKweet(kweetId, userId);
                if (response.success)
                    ElMessage({
                        message: 'The kweet is unliked.',
                        type: 'success'
                    });
                else
                    ElMessage({
                        message: 'The kweet is not unliked. Try again later.',
                        type: 'error'
                    });
            },
            async like() {
                this.$props.kweet!.liked = true;
                const kweetId: string = this.$props.kweet!.id;
                const userId: string = this.$props.kweet!.userId;
                const response: Response = await this.$kweetService.likeKweet(kweetId, userId);
                if (response.success)
                    ElMessage({
                        message: 'The kweet is liked.',
                        type: 'success'
                    });
                else
                    ElMessage({
                        message: 'The kweet is not liked. Try again later.',
                        type: 'error'
                    });
            }
        }
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
