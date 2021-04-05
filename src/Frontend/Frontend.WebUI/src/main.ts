import { createApp } from 'vue';
import App from '@/App.vue';

// Config
import { authConfig } from '@/config';

// Plugins
import { createKwetterRouter, KwetterRoute } from './plugins/vuerouter';
import VueGapi from 'vue-gapi';
import { rootStore } from '@/modules';
import ElementPlus from 'element-plus';

// Services

// Styling
import 'typeface-nunito';
import 'element-plus/lib/theme-chalk/index.css';

const routes: KwetterRoute[] = [
    { name: 'Home', props: { msg: 'Welcome to HOME!' }},
    { name: 'Alias', props: { msg: 'Welcome to ALIAS!' }}
];

createApp(App)
    .use(ElementPlus)
    .use(rootStore)
    .use(VueGapi, authConfig)
    .use(createKwetterRouter(routes))
    .mount('#app');
