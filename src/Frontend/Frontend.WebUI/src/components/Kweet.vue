<template>
    <el-card class="box-card">
        <el-row>
            <el-col :span="2">
                <div class="grid-content">
                    <div class="block">
                        <el-avatar :size="50" :src="kweet.userProfilePictureUrl" />
                    </div>
                </div>
            </el-col>
            <el-col :span="20">
                <div class="displayName">
                    {{ kweet.userDisplayName }}
                </div>
                <div class="grid-content">
                    {{ kweet.message }}
                </div>
                <div class="dateTime">
                    {{ new Date(kweet.createdDateTime).toLocaleString() }}
                </div>
            </el-col>
            <el-col :span="2">
                <el-badge :value="kweet.likeCount" class="likeCountBadge">
                    <img class="like" v-show="kweet.liked" @click="unlike" src="/assets/heart icon fill.png" />
                    <img class="like" v-show="!kweet.liked" @click="like" src="/assets/heart icon.png" />
                </el-badge>
            </el-col>
        </el-row>
    </el-card>
</template>

<script lang="ts">
    import { defineComponent, PropType } from 'vue';
    import { Kweet } from '@/modules/Kweet/Kweet';
    import Response from '@/models/cqrs/Response';
    import { ElMessage } from 'element-plus';

    export default defineComponent({
        name: 'Kweet',
        props: {
            kweet: Object as PropType<Kweet>,
            userId: String as PropType<string>
        },
        methods: {
            async unlike() {
                const kweetId: string = this.$props.kweet!.id;
                const userId: string = this.$props.userId!;
                const response: Response = await this.$kweetService.unlikeKweet(kweetId, userId);
                if (response.success) {
                    this.$props.kweet!.liked = false;
                    this.$props.kweet!.likeCount -= 1;
                    ElMessage({
                        message: 'The kweet is unliked.',
                        type: 'success'
                    });
                } else if (response.errors.includes('Service unreachable.')) {
                    ElMessage({
                        message: 'The kweet service is currently unreachable. Try again later.',
                        type: 'warning'
                    });
                } else {
                    ElMessage({
                        message: 'The kweet is not unliked. Try again later.',
                        type: 'error'
                    });
                }
            },
            async like() {
                const kweetId: string = this.$props.kweet!.id;
                const userId: string = this.$props.userId!;
                const response: Response = await this.$kweetService.likeKweet(kweetId, userId);
                if (response.success) {
                    this.$props.kweet!.liked = true;
                    this.$props.kweet!.likeCount += 1;
                    ElMessage({
                        message: 'The kweet is liked.',
                        type: 'success'
                    });
                } else if (response.errors.includes('Service unreachable.')) {
                    ElMessage({
                        message: 'The kweet service is currently unreachable. Try again later.',
                        type: 'warning'
                    });
                } else {
                    ElMessage({
                        message: 'The kweet is not liked. Try again later.',
                        type: 'error'
                    });
                }
            }
        }
        // TODO: Clickable link to profile using user id?
    });
</script>

<style scoped>
    .likeCountBadge {
        margin-top: 10px;
        margin-right: 40px;
    }
    .displayName {
        text-decoration: solid;
        font-weight: bolder;
        text-align: left;
    }
    .dateTime {
        font-size: smaller;
        font-weight: 100;
        text-align: right;
        margin-right: -50px;
    }
    .like {
        max-width: 50px;
        max-height: 50px;
    }
    .box-card {
        min-width: 480px;
        max-height: 160px!important;
    }
    .grid-content {
        border-radius: 4px;
        min-height: 36px;
        text-align: left;
    }
</style>
