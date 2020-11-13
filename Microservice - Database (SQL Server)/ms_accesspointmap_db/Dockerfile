FROM mcr.microsoft.com/mssql/server

ENV ACCEPT_EULA=Y
ENV SA_PASSWORD=Mypassword123
ENV TZ=Europe/Amsterdam

COPY ./setup.sql ./setup.sql
COPY ./setup_database.sh ./setup_database.sh
COPY ./entrypoint.sh ./entrypoint.sh

EXPOSE 1433

CMD /bin/bash ./entrypoint.sh