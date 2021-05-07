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
    import QueryResponse from '@/models/cqrs/QueryResponse';
    import { ElMessage } from 'element-plus';

    export default defineComponent({
        name: 'Timeline',
        data(): { kweets: KweetType[], counter: number, loading: boolean, pageSize: number, pageNumber: number, noMore: boolean } {
            return {
                kweets: [],
                pageNumber: 0,
                pageSize: 10,
                counter: 0,
                loading: false,
                noMore: false
            };
        },
        computed: {
            disabled(): boolean {
                return this.$data.loading || this.$data.noMore;
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
            async load(): Promise<void> {
                if (this.$data.loading == true)
                    return;
                this.$data.loading = true;
                const queryResponse: QueryResponse<KweetType[]> = await this.$timelineService.paginateKweets(this.$data.pageNumber, this.$data.pageSize);
                if (queryResponse.success) {
                    if (queryResponse.data.length == 0) {
                        this.$data.noMore = true;
                        ElMessage({
                            message: 'You have reached the end of your timeline!',
                            type: 'info'
                        });
                    } else {
                        this.$data.kweets.push(...queryResponse.data);
                        this.$data.pageNumber += 1;
                    }
                } else if (queryResponse.errors.includes('Service unreachable.')) {
                    this.$data.noMore = true;
                    ElMessage({
                        message: 'The timeline service is currently unreachable. Try again later.',
                        type: 'warning'
                    });
                }
                this.$data.loading = false;
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
        max-height: 160px!important;
    }
</style>
