import { createApp } from 'vue';
import App from '@/App.vue';

// Config
import { authConfig, GATEWAY_API_URL } from '@/config';

// Plugins
import { createKwetterRouter, KwetterRoute } from '@/plugins/vuerouter';
import VueGapi from 'vue-gapi';
import { rootStore } from '@/modules';
import ElementPlus from 'element-plus';

// Services
import IAuthorizationService from "@/interfaces/IAuthorizationService";
import AuthorizationService from "@/services/AuthorizationService";
import IKweetService from '@/interfaces/IKweetService';

// Styling
import 'typeface-nunito';
import 'element-plus/lib/theme-chalk/index.css';
import KweetService from '@/services/KweetService';
import IHttpCommunicator from '@/interfaces/IHttpCommunicator';
import HttpCommunicator from '@/utils/HttpCommunicator';

const routes: KwetterRoute[] = [
    { name: 'Home', props: { msg: 'Welcome to Home!' } },
    { name: 'Timeline' }
];


const authorizationService: IAuthorizationService = new AuthorizationService(`${GATEWAY_API_URL}/Authorization`);

const httpCommunicator: IHttpCommunicator = new HttpCommunicator(GATEWAY_API_URL);

const kweetService: IKweetService = new KweetService(httpCommunicator);

const app = createApp(App);

// Adds the plugins
app.use(ElementPlus);
app.use(rootStore);
app.use(VueGapi, authConfig);
app.use(createKwetterRouter(routes));

// Provides the services to the components by definining them globally
app.config.globalProperties.$authorizationService = authorizationService;
app.config.globalProperties.$kweetService = kweetService;

// Mounts the app
app.mount('#app');
