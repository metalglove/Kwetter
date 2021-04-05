//import { config as configDotenv } from 'dotenv';
//import { resolve } from 'path';

//switch (process.env.NODE_ENV) {
//    case 'development':
//        configDotenv({
//            path: resolve(__dirname, '/../.env.development')
//        });
//        break;
//    // Add other environments...
//    default:
//        break;
//}

export const authConfig = {
    apiKey: process.env.VUE_APP_GOOGLE_API_KEY,
    clientId: process.env.VUE_APP_GOOGLE_CLIENT_ID,
    discoveryDocs: ['https://people.googleapis.com/$discovery/rest?version=v1'],
    scope: 'openid profile email'
};