<template>
    <div>
        <h1>ALIAS {{ msg }}</h1>
    </div>
</template>

<script lang="ts">
    import { defineComponent } from 'vue';

    export default defineComponent({
        name: 'Alias',
        props: {
            msg: String
        }
    });
</script>

<style>
</style>
