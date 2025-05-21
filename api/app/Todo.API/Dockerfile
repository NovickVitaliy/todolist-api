FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["app/Todo.API/Todo.API.csproj", "app/Todo.API/"]
COPY ["app/Todo.Application/Todo.Application.csproj", "app/Todo.Application/"]
COPY ["shared/Shared.CQRS/Shared.CQRS.csproj", "shared/Shared.CQRS/"]
COPY ["app/Todo.Bussiness/Todo.Bussiness.csproj", "app/Todo.Bussiness/"]
COPY ["app/Todo.DataAccess/Todo.DataAccess.csproj", "app/Todo.DataAccess/"]
COPY ["shared/Shared/Shared.csproj", "shared/Shared/"]
COPY ["app/Todo.Domain/Todo.Domain.csproj", "app/Todo.Domain/"]
RUN dotnet restore "app/Todo.API/Todo.API.csproj"
COPY . .
WORKDIR "/src/app/Todo.API"
RUN dotnet build "./Todo.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Todo.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Todo.API.dll"]
