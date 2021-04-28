import { createApp } from 'vue';
import App from '@/App.vue';

// Config
import { firebaseConfig, GATEWAY_API_URL } from '@/config';

// Plugins
import { createKwetterRouter, KwetterRoute } from '@/plugins/vuerouter';
import { rootStore } from '@/modules';
import ElementPlus from 'element-plus';
import firebase from 'firebase/app';
import 'firebase/auth';

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

const firebaseApp: firebase.app.App = firebase.initializeApp(firebaseConfig);

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
app.use(createKwetterRouter(routes));

// Provides the services to the components by definining them globally
app.config.globalProperties.$authorizationService = authorizationService;
app.config.globalProperties.$kweetService = kweetService;
app.config.globalProperties.$firebaseAuth = firebaseApp;

// Mounts the app
app.mount('#app');
