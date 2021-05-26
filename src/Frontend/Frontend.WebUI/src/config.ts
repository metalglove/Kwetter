// @ts-ignore
import Configuration from './configuration.js';

export const firebaseConfig = {
    apiKey: Configuration.value('VUE_APP_APIKEY'),
    authDomain: Configuration.value('VUE_APP_AUTH_DOMAIN'),
    projectId: Configuration.value('VUE_APP_PROJECT_ID'),
    storageBucket: Configuration.value('VUE_APP_STORAGE_BUCKET'),
    messagingSenderId: Configuration.value('VUE_APP_MESSAGING_SENDER_ID'),
    appId: Configuration.value('VUE_APP_APP_ID')
};

export const GATEWAY_API_URL: string = Configuration.value('VUE_APP_GATEWAY_API_URL');
export const GATEWAY_WS_API_URL: string = Configuration.value('VUE_APP_GATEWAY_WS_API_URL');
