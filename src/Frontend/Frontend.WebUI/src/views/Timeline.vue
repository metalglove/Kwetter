<template>
    <el-container>
        <el-container>
            <el-header>
                <kweet-composer @posted="addKweet" class="kweetComposer" />
            </el-header>
            <el-main>
                <ul class="infinite-list" 
                    v-infinite-scroll="load" 
                    style="overflow:auto"
                    infinite-scroll-disabled="disabled">
                    <li v-for="kweet in kweets" class="infinite-list-item">
                        <kweet :kweet="kweet"/>
                    </li>
                </ul>
            </el-main>
        </el-container>
        <el-aside>Aside</el-aside>
    </el-container>
</template>

<script lang="ts">
    import { defineComponent } from 'vue';
    import KweetComposer from '@/components/KweetComposer.vue';
    import Kweet from '@/components/Kweet.vue';
    import { Kweet as KweetType } from '@/modules/Kweet/Kweet';

    export default defineComponent({
        name: 'Timeline',
        data(): { kweets: KweetType[], counter: number, loading: boolean } {
            return {
                kweets: [],
                counter: 0,
                loading: false
            };
        },
        computed: {
            noMore(): boolean {
                return false;
            },
            disabled(): boolean {
                return this.$data.loading || this.noMore;
            }
        },
        components: {
            KweetComposer,
            Kweet
        },
        methods: {
            addKweet(event: KweetType): void {
                this.$data.kweets.unshift(event);
            },
            load(): void {
                const kweet: KweetType = {
                    id: `${this.$data.counter += 1}`,
                    avatar: 'https://avatars.githubusercontent.com/u/24389604?v=4',
                    createdAt: '0',
                    liked: false,
                    message: 'Kwetterwette rHelloKwette rHelloKwetter HelloKwetterHe lloKwetterHel loKwetterHelloKwe tterHelloKwetter HelloKwetter Hello!',
                    userId: 'some id'
                };
                this.$data.kweets.push(kweet);
            }
        }
    });
</script>

<style scoped>
    .kweetComposer {
        margin: 10px;
    }

    .el-header {
        background-color: #B3C0D1;
        color: #333;
        margin: 1px;
        text-align: center;
        min-height: 125px!important;
        height: 125px!important;
        max-height: 125px!important;
    }

    .el-aside {
        background-color: #D3DCE6;
        margin: 1px;
        color: #333;
        text-align: center;
        line-height: 200px;
        width: 30% !important;
    }

    .el-main {
        background-color: #E9EEF3;
        color: #333;
        margin: 1px;
        height: 100%;
        text-align: center;
    }

    .infinite-list {
        height: calc(100vh - 230px);
        padding: 0;
        margin: 0;
        list-style: none;
    }
    .infinite-list-item {
        padding: 5px!important;
        max-height: 100px!important;
    }
</style>
