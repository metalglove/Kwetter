export const authConfig = {
    apiKey: process.env.VUE_APP_GOOGLE_API_KEY,
    clientId: process.env.VUE_APP_GOOGLE_CLIENT_ID,
    discoveryDocs: ['https://people.googleapis.com/$discovery/rest?version=v1'],
    scope: 'openid profile email'
};

export const GATEWAY_API_URL: string = process.env.VUE_APP_GATEWAY_API_URL!;
