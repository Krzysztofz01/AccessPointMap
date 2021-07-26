FROM mariadb:latest

ENV MYSQL_ROOT_PASSWORD=Admin.Admin.Admin.123
ENV MYSQL_USER=apmadmin
ENV MYSQL_PASSWORD=Admin.Admin.Admin.123
ENV MYSQL_DATABASE=master

COPY init.sql /docker-entrypoint-initdb.d/1.sql

EXPOSE 3306