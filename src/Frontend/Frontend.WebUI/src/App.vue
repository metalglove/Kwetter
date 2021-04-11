<template>
    <top-bar />
    <router-view />
</template>

<script lang="ts">
    import { defineComponent } from 'vue';
    import TopBar from '@/components/TopBar.vue';

    export default defineComponent({
        name: 'App',
        components: {
            TopBar
        }
    });
</script>

<style>
    * {
        font-family: 'Nunito';
    }
    body {
        max-width: 1240px!important;
        margin: auto;
    }
</style>
