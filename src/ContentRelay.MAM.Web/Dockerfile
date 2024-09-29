FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/ContentRelay.Shared/ContentRelay.Shared.csproj", "src/ContentRelay.Shared/"]
COPY ["src/ContentRelay.MAM.Application/ContentRelay.MAM.Application.csproj", "src/ContentRelay.MAM.Application/"]
COPY ["src/ContentRelay.MAM.Domain/ContentRelay.MAM.Domain.csproj", "src/ContentRelay.MAM.Domain/"]
COPY ["src/ContentRelay.MAM.Infrastructure/ContentRelay.MAM.Infrastructure.csproj", "src/ContentRelay.MAM.Infrastructure/"]
COPY ["src/ContentRelay.MAM.Web/ContentRelay.MAM.Web.csproj", "src/ContentRelay.MAM.Web/"]
RUN dotnet restore "src/ContentRelay.MAM.Web/ContentRelay.MAM.Web.csproj"
COPY . .
WORKDIR "/src/src/ContentRelay.MAM.Web"
#RUN dotnet build "ContentRelay.MAM.Web.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "ContentRelay.MAM.Web.csproj" -c $BUILD_CONFIGURATION -o /app/publish --no-restore

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ContentRelay.MAM.Web.dll"]
