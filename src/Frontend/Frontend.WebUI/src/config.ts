// export const authConfig = {
//     apiKey: process.env.VUE_APP_GOOGLE_API_KEY,
//     clientId: process.env.VUE_APP_GOOGLE_CLIENT_ID,
//     discoveryDocs: ['https://people.googleapis.com/$discovery/rest?version=v1'],
//     scope: 'openid profile email'
// };

export const firebaseConfig = {
    apiKey: process.env.VUE_APP_APIKEY,
    authDomain: process.env.VUE_APP_AUTH_DOMAIN,
    projectId: process.env.VUE_APP_PROJECT_ID,
    storageBucket: process.env.VUE_APP_STORAGE_BUCKET,
    messagingSenderId: process.env.VUE_APP_MESSAGING_SENDER_ID,
    appId: process.env.VUE_APP_APP_ID
};

export const GATEWAY_API_URL: string = process.env.VUE_APP_GATEWAY_API_URL!;
