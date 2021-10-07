FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY AccessPointMap.sln ./
COPY ["AccessPointMap.Web/AccessPointMap.Web.csproj", "AccessPointMap.Web/"]
COPY ["AccessPointMap.Service/AccessPointMap.Service.csproj", "AccessPointMap.Service/"]
COPY ["AccessPointMap.Repository/AccessPointMap.Repository.csproj", "AccessPointMap.Repository/"]
COPY ["AccessPointMap.Domain/AccessPointMap.Domain.csproj", "AccessPointMap.Domain/"]
RUN dotnet restore "AccessPointMap.Web/AccessPointMap.Web.csproj"
COPY . .
WORKDIR "/src/AccessPointMap.Web"
RUN dotnet build "AccessPointMap.Web.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "AccessPointMap.Web.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "AccessPointMap.Web.dll"]