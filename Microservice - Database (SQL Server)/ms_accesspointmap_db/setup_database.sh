#!/usr/bin/env bash
sleep 20
./opt/mssql-tools/bin/sqlcmd -S localhost -U SA -P Mypassword123 -i setup.sql