FROM mcr.microsoft.com/mssql/server

ENV ACCEPT_EULA=Y
ENV SA_PASSWORD=Admin.Admin.Admin.123
ENV TZ=Europe/Amsterdam

COPY ./init.sql ./init.sql
COPY ./setup.sh ./setup.sh
COPY ./entrypoint.sh ./entrypoint.sh

EXPOSE 1433

CMD /bin/bash ./entrypoint.sh