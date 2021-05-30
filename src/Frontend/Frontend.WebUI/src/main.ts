import { createApp } from 'vue';
import App from '@/App.vue';

// Config
import { firebaseConfig, GATEWAY_API_URL, GATEWAY_WS_API_URL } from '@/config';

// Plugins
import { createKwetterRouter, KwetterRoute } from '@/plugins/vuerouter';
import { rootStore } from '@/modules';
import ElementPlus from 'element-plus';
import firebase from 'firebase/app';
import 'firebase/auth';

// Services
import IHttpCommunicator from '@/interfaces/IHttpCommunicator';
import HttpCommunicator from '@/utils/HttpCommunicator';
import IAuthorizationService from '@/interfaces/IAuthorizationService';
import AuthorizationService from '@/services/AuthorizationService';
import IKweetService from '@/interfaces/IKweetService';
import KweetService from '@/services/KweetService';
import ITimelineService from '@/interfaces/ITimelineService';
import TimelineService from '@/services/TimelineService';
import { INotificationService } from './interfaces/INotificationService';
import NotificationService from './services/NotificationService';

// Handlers
import UserMentionedHandler from './handlers/UserMentionedHandler';
import KweetLikedHandler from './handlers/KweetLikedHandler';
import UserFollowedHandler from './handlers/UserFollowedHandler';

// Styling
import 'typeface-nunito';
import 'element-plus/lib/theme-chalk/index.css';

const firebaseApp: firebase.app.App = firebase.initializeApp(firebaseConfig);

const routes: KwetterRoute[] = [
    { name: 'Home', props: { msg: 'Welcome to Home!' } },
    { name: 'Register' },
    { name: 'Timeline' }
];

const httpCommunicator: IHttpCommunicator = new HttpCommunicator(GATEWAY_API_URL, firebaseApp);
const authorizationService: IAuthorizationService = new AuthorizationService(httpCommunicator);
const kweetService: IKweetService = new KweetService(httpCommunicator);
const timelineService: ITimelineService = new TimelineService(httpCommunicator);
const notificationService: INotificationService = new NotificationService(GATEWAY_WS_API_URL);
notificationService.addEventHandler(new UserMentionedHandler());
notificationService.addEventHandler(new KweetLikedHandler());
notificationService.addEventHandler(new UserFollowedHandler());

const app = createApp(App);

// Adds the plugins
app.use(ElementPlus);
app.use(rootStore);
app.use(createKwetterRouter(routes));

// Provides the services to the components by definining them globally
app.config.globalProperties.$authorizationService = authorizationService;
app.config.globalProperties.$kweetService = kweetService;
app.config.globalProperties.$timelineService = timelineService;
app.config.globalProperties.$firebaseAuth = firebaseApp;

// Mounts the app
app.mount('#app');
