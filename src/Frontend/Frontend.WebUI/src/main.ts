import { createApp } from 'vue';
import App from './App.vue';

// Config

// Plugins
import { createKwetterRouter, KwetterRoute } from './plugins/vuerouter';
import { rootStore } from './modules';
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
    .use(createKwetterRouter(routes))
    .mount('#app');
