FROM node:12.16.1 AS build
WORKDIR /usr/src/app
COPY . .
RUN npm install
RUN npm run ng build -- --prod

FROM nginx:1.17.1-alpine
COPY nginx.conf /etc/nginx/nginx.conf
COPY --from=build /usr/src/app/dist/AccessPointMap /usr/share/nginx/html