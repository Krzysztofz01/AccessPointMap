FROM node:14 AS build
WORKDIR /usr/src/app
COPY . .
RUN npm install
# RUN npm run ng build -- --prod --base-href /projects/accesspointmap/ --deploy-url /projects/accesspointmap/
RUN npm run ng build -- --prod

FROM nginx:stable-alpine
COPY nginx.conf /etc/nginx/nginx.conf
COPY --from=build /usr/src/app/dist/AccessPointMap /usr/share/nginx/html