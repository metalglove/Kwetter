import { createApp } from 'vue';
import App from '@/App.vue';

// Config
import { authConfig, GATEWAY_API_URL } from '@/config';

// Plugins
import { createKwetterRouter, KwetterRoute } from './plugins/vuerouter';
import VueGapi from 'vue-gapi';
import { rootStore } from '@/modules';
import ElementPlus from 'element-plus';

// Services
import IAuthorizationService from "@/interfaces/IAuthorizationService";
import AuthorizationService from "@/services/AuthorizationService";

// Styling
import 'typeface-nunito';
import 'element-plus/lib/theme-chalk/index.css';

const routes: KwetterRoute[] = [
    { name: 'Home', props: { msg: 'Welcome to HOME!' }},
    { name: 'Alias', props: { msg: 'Welcome to ALIAS!' }}
];


const authorizationService: IAuthorizationService = new AuthorizationService(`${GATEWAY_API_URL}/Authorization`);

const app = createApp(App);

// Adds the plugins
app.use(ElementPlus);
app.use(rootStore);
app.use(VueGapi, authConfig);
app.use(createKwetterRouter(routes));

// Provides the services to the components by definining them globally
app.config.globalProperties.$authorizationService = authorizationService;

// Mounts the app
app.mount('#app');
