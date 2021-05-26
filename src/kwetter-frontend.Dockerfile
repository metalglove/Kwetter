FROM node:16.2-alpine as build

ARG VUE_APP_APIKEY
ARG VUE_APP_GATEWAY_WS_API_URL
ARG VUE_APP_AUTH_DOMAIN
ARG VUE_APP_PROJECT_ID
ARG VUE_APP_STORAGE_BUCKET
ARG VUE_APP_MESSAGING_SENDER_ID
ARG VUE_APP_APP_ID
ARG VUE_APP_PROJECT_ID
ARG VUE_APP_GATEWAY_API_URL

ENV VUE_APP_APIKEY=$VUE_APP_APIKEY
ENV VUE_APP_AUTH_DOMAIN=$VUE_APP_AUTH_DOMAIN
ENV VUE_APP_PROJECT_ID=$VUE_APP_PROJECT_ID
ENV VUE_APP_STORAGE_BUCKET=$VUE_APP_STORAGE_BUCKET
ENV VUE_APP_MESSAGING_SENDER_ID=$VUE_APP_MESSAGING_SENDER_ID
ENV VUE_APP_APP_ID=$VUE_APP_APP_ID
ENV VUE_APP_PROJECT_ID=$VUE_APP_PROJECT_ID
ENV VUE_APP_GATEWAY_API_URL=$VUE_APP_GATEWAY_API_URL
ENV VUE_APP_GATEWAY_WS_API_URL=$VUE_APP_GATEWAY_WS_API_URL

COPY Frontend/Frontend.WebUI/src /app/src
COPY Frontend/Frontend.WebUI/public /app/public
COPY Frontend/Frontend.WebUI/package*.json /app/
COPY Frontend/Frontend.WebUI/babel.config.js /app/
COPY Frontend/Frontend.WebUI/tsconfig.json /app/

WORKDIR /app

RUN npm install
RUN npm run build

FROM nginx:stable-alpine as production

RUN mkdir /app

COPY --from=build /app/dist /usr/share/nginx/html
COPY nginx.conf /temp/prod.conf
RUN envsubst /app < /temp/prod.conf > /etc/nginx/conf.d/default.conf

EXPOSE 80

COPY Frontend/Frontend.WebUI/entrypoint.sh /entrypoint.sh
RUN chmod +x /entrypoint.sh
CMD ["sh", "/entrypoint.sh"]
