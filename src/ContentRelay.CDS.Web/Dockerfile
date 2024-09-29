FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/ContentRelay.CDS.Web/ContentRelay.CDS.Web.csproj", "src/ContentRelay.CDS.Web/"]
RUN dotnet restore "src/ContentRelay.CDS.Web/ContentRelay.CDS.Web.csproj"
COPY . .
WORKDIR "/src/src/ContentRelay.CDS.Web"
RUN dotnet build "ContentRelay.CDS.Web.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "ContentRelay.CDS.Web.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ContentRelay.CDS.Web.dll"]
