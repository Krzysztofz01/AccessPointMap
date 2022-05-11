FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["AccessPointMap.API/AccessPointMap.API.csproj", "AccessPointMap.API/"]
COPY ["AccessPointMap.Infrastructure.MySql/AccessPointMap.Infrastructure.MySql.csproj", "AccessPointMap.Infrastructure.MySql/"]
COPY ["AccessPointMap.Domain/AccessPointMap.Domain.csproj", "AccessPointMap.Domain/"]
COPY ["AccessPointMap.Domain.Core/AccessPointMap.Domain.Core.csproj", "AccessPointMap.Domain.Core/"]
COPY ["AccessPointMap.Infrastructure.Core/AccessPointMap.Infrastructure.Core.csproj", "AccessPointMap.Infrastructure.Core/"]
COPY ["AccessPointMap.Application.Integration.Wigle/AccessPointMap.Application.Integration.Wigle.csproj", "AccessPointMap.Application.Integration.Wigle/"]
COPY ["AccessPointMap.Application.Integration.Core/AccessPointMap.Application.Integration.Core.csproj", "AccessPointMap.Application.Integration.Core/"]
COPY ["AccessPointMap.Application.Oui.Core/AccessPointMap.Application.Oui.Core.csproj", "AccessPointMap.Application.Oui.Core/"]
COPY ["AccessPointMap.Application.Oui.MacTwoVendor/AccessPointMap.Application.Oui.MacTwoVendor.csproj", "AccessPointMap.Application.Oui.MacTwoVendor/"]
COPY ["AccessPointMap.Application.Integration.Aircrackng/AccessPointMap.Application.Integration.Aircrackng.csproj", "AccessPointMap.Application.Integration.Aircrackng/"]
COPY ["AccessPointMap.Application/AccessPointMap.Application.csproj", "AccessPointMap.Application/"]
RUN dotnet restore "AccessPointMap.API/AccessPointMap.API.csproj"
COPY . .
WORKDIR "/src/AccessPointMap.API"
RUN dotnet build "AccessPointMap.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "AccessPointMap.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

# Mac2Vendor OUI Database
ADD https://mac2vendor.com/download/oui-database.sqlite .

ENTRYPOINT ["dotnet", "AccessPointMap.API.dll"]