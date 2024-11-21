FROM node:21-alpine AS build

WORKDIR /app

COPY ./TaskMasterUI/package.json ./TaskMasterUI/package-lock.json ./

RUN npm i

COPY ./TaskMasterUI/. .

RUN npm run build

FROM nginx:stable-alpine

RUN rm -rf /usr/share/nginx/html/*

COPY --from=build /app/dist /usr/share/nginx/html
COPY ./TaskMasterUI/nginx.conf /etc/nginx/conf.d/default.conf

EXPOSE 80

CMD ["nginx", "-g", "daemon off;"]
