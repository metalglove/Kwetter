<template>
    <div>
        <h1>HOME {{ msg }}</h1>
    </div>
</template>

<script lang="ts">
    import { defineComponent } from 'vue';

    export default defineComponent({
        name: 'Home',
        props: {
            msg: String
        }
    });
</script>

<style>
</style>
