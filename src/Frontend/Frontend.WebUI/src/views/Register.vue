<template>
    <div>
        <div class="register">
            <el-card>
                <h2>Register</h2>
                <el-form class="register-form"
                         :model="user"
                         :rules="rules"
                         :ref="setFormRef"
                         @submit.native.prevent="register">
                    <el-form-item prop="username">
                        <el-input type="text"
                                  placeholder="username"
                                  v-model="user.username"
                                  maxlength="32"
                                  show-word-limit prefix-icon="fas fa-user">
                        </el-input>
                    </el-form-item>
                    <el-form-item>
                        <el-button :loading="loading"
                                   class="register-button"
                                   type="primary"
                                   native-type="submit"
                                   block>Sign up with Google</el-button>
                    </el-form-item>
                </el-form>
            </el-card>
        </div>
    </div>
</template>

<script lang="ts">
    import { defineComponent, reactive, ref, unref } from 'vue';
    import QueryResponse from '@/models/cqrs/QueryResponse';
    import CommandResponse from '@/models/cqrs/CommandResponse';
    import VerifyUserNameUniquenessDto from '@/models/dtos/VerifyUserNameUniquenessDto';
    import { ElMessage, ElForm } from 'element-plus';
    import firebase from 'firebase/app';
    import 'firebase/auth';
    import { toUserFromIdToken, User } from '@/modules/User/User';
    import { UserActionTypes } from '@/modules/User/User.actions';

    export default defineComponent({
        name: 'Register',
        data() {
            const form = ref<InstanceType<typeof ElForm>>();
            const setFormRef = (el: InstanceType<typeof ElForm>) => {
                form.value = el;
            };
            let user = ref({ username: '' });
            let rules = reactive({
                username: [
                    {
                        required: true,
                        message: "The user name is required",
                        trigger: "blur"
                    },
                    {
                        min: 4,
                        message: "The user name length should be at least 3 characters.",
                        trigger: "blur"
                    },
                    {
                        max: 32,
                        message: "The user name length may not exceed 32 characters.",
                        trigger: "blur"
                    },
                    {
                        message: "The user name must be alphanumeric.",
                        trigger: "blur",
                        validator: this.isAlphanumeric
                    },
                    {
                        message: "The user name is already taken. Try something else.",
                        trigger: "blur",
                        validator: this.uniqueValidator
                    }
                ]
            });
            let loading = ref(false);
            return {
                setFormRef,
                form,
                user,
                rules,
                loading
            }
        },
        methods: {
            isAlphanumeric(rule: object, value: string, callback: (error?: Error) => void): void {
                if (!value.match(/^[A-Za-z0-9]+$/)) {
                    callback(new Error('The user name must be alphanumeric.'));
                    return;
                } else {
                    callback();
                }
            },
            async uniqueValidator(rule: object, value: string, callback: (error?: Error) => void): Promise<void> {
                if (this.loading) {
                    callback();
                    return;
                }
                this.loading = true;
                const response: QueryResponse<VerifyUserNameUniquenessDto> = await this.$authorizationService.VerifyUserNameUniqueness(value);
                this.loading = false;
                if (response.success) {
                    if (!response.data.isUnique) {
                        callback(new Error('The user name must be unique.'));
                    } else {
                        callback();
                    }
                } else {
                    ElMessage({
                        message: 'Authorization service is currently unavailable. Try again later.',
                        type: 'warning'
                    });
                }
            },
            async register(): Promise<void> {
                if (this.loading)
                    return;
                let success: boolean | undefined = false;
                try {
                    success = await this.form?.validate()
                } catch (_) { }
                if (!success) {
                    return;
                }
                this.loading = true;
                await firebase
                    .auth()
                    .signInWithPopup(new firebase.auth.GoogleAuthProvider())
                    .then(async (result) => {
                        let idToken = await result.user!.getIdToken();
                        let user: User = toUserFromIdToken(idToken);
                        if (user.userId == null) {
                            const claimsResponse: CommandResponse = await this.$authorizationService.SetClaims(idToken, this.user.username);
                            if (claimsResponse.success) {
                                // get new token with claims set!
                                idToken = await result.user!.getIdToken(true);
                                user = toUserFromIdToken(idToken);
                                this.$store.dispatch(`user/${UserActionTypes.SET_USER}`, user);
                                this.$router.push('/Home');
                                return ElMessage({
                                    message: 'Successfully registered!',
                                    type: 'success'
                                });
                            }
                        } else {
                            await firebase.auth().signOut();
                            return ElMessage({
                                message: 'You are already a user. Please sign in normally.',
                                type: 'info'
                            });
                        }
                        await firebase.auth().signOut();
                        return ElMessage({
                            message: 'Registration failed, try again later.',
                            type: 'error'
                        });
                    }, (error) => {
                        ElMessage({
                            message: 'Registration failed, try again later.',
                            type: 'error'
                        });
                        console.log(error);
                    });
                this.loading = false;
            }
        }
    });
</script>

<style scoped>
    .register {
        flex: 1;
        display: flex;
        justify-content: center;
        align-items: center;
    }

    .register-button {
        width: 100%;
        margin-top: 40px;
    }

    .register-form {
        width: 290px;
    }
</style>
