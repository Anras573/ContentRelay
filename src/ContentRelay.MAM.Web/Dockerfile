FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/ContentRelay.MAM.Web/ContentRelay.MAM.Web.csproj", "src/ContentRelay.MAM.Web/"]
RUN dotnet restore "src/ContentRelay.MAM.Web/ContentRelay.MAM.Web.csproj"
COPY . .
WORKDIR "/src/src/ContentRelay.MAM.Web"
RUN dotnet build "ContentRelay.MAM.Web.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "ContentRelay.MAM.Web.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ContentRelay.MAM.Web.dll"]
