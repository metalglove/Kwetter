export const firebaseConfig = {
    apiKey: process.env.VUE_APP_APIKEY,
    authDomain: process.env.VUE_APP_AUTH_DOMAIN,
    projectId: process.env.VUE_APP_PROJECT_ID,
    storageBucket: process.env.VUE_APP_STORAGE_BUCKET,
    messagingSenderId: process.env.VUE_APP_MESSAGING_SENDER_ID,
    appId: process.env.VUE_APP_APP_ID
};

export const GATEWAY_API_URL: string = process.env.VUE_APP_GATEWAY_API_URL!;
export const GATEWAY_WS_API_URL: string = process.env.VUE_APP_GATEWAY_WS_API_URL!;
