FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/ContentRelay.Shared/ContentRelay.Shared.csproj", "src/ContentRelay.Shared/"]
COPY ["src/Mock/OrderListMetadataProducer/OrderListMetadataProducer.csproj", "src/Mock/OrderListMetadataProducer/"]
RUN dotnet restore "src/Mock/OrderListMetadataProducer/OrderListMetadataProducer.csproj"
COPY . .
WORKDIR "/src/src/Mock/OrderListMetadataProducer"
# RUN dotnet build "OrderListMetadataProducer.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "OrderListMetadataProducer.csproj" -c $BUILD_CONFIGURATION -o /app/publish --no-restore

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "OrderListMetadataProducer.dll"]
