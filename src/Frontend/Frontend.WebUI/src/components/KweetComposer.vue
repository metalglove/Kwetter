<template>
    <el-card class="input">
        <el-row >
            <el-col :span="20">
                <el-input type="textarea" v-model="message"/>
            </el-col>
            <el-col :span="4">
                <el-button type="primary" class="post" @click="postKweet">POST</el-button>
            </el-col>
        </el-row>
    </el-card>
</template>

<script lang="ts">
    import { defineComponent } from 'vue';
    import { mapGetters } from 'vuex';
    import Guid from '@/utils/Guid';
    import { UserGetterTypes } from '@/modules/User/User.getters';
    import { User } from '@/modules/User/User';
    import Response from '@/models/cqrs/Response';
    import { Kweet } from '@/modules/Kweet/Kweet';
    import { ElMessage } from "element-plus";

    export default defineComponent({
        name: 'KweetComposer',
        emits: ['posted'],
        data() {
            return {
                message: ''
            }
        },
        computed: {
            ...mapGetters('user', [UserGetterTypes.GET_USER])
        },
        methods: {
            async postKweet() {
                const message: string = this.$data.message;
                if (message.length < 3) {
                    ElMessage({
                        message: 'The message needs to have at least 3 characters.',
                        type: 'error'
                    });
                    return;
                }
                if (message.length > 140) {
                    ElMessage({
                        message: 'The message exceeded the maximum message length of 140.',
                        type: 'error'
                    });
                    return;
                }

                const user: User = this.GET_USER as User;
                const kweetId: string = Guid.newGuid().toString()
                const userId: string = user.userId;
                const response: Response = await this.$kweetService.postKweet(kweetId, message, userId);
                if (response.success) {
                    const kweet: Kweet = {
                        avatar: user.profile.picture,
                        id: kweetId,
                        liked: false,
                        message: message,
                        userId: userId,
                        createdAt: 'now?'
                    };
                    this.$emit('posted', kweet);
                    ElMessage({
                        message: 'Successfully posted the kweet!',
                        type: 'success'
                    });
                }
                else {
                    ElMessage({
                        message: 'The kweet failed to post. Try again later.',
                        type: 'error'
                    });
                }
                this.$data.message = '';
            }
        }
    });
</script>

<style scoped>
    .post {
        height: 100%!important;
    }
</style>
