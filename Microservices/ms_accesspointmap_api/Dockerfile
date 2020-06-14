FROM node:latest

COPY . /src

WORKDIR /src

RUN npm install --production

EXPOSE 3011

CMD npm start