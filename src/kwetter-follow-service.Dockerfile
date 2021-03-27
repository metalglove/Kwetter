FROM mcr.microsoft.com/dotnet/aspnet:5.0-buster-slim AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:5.0-buster-slim AS build
WORKDIR /src
COPY ["Services/FollowService/API/Kwetter.Services.FollowService.API.csproj", "Services/FollowService/API/"]
COPY ["Services/Common/API/Kwetter.Services.Common.API.csproj", "Services/Common/API/"]
COPY ["Services/Common/Infrastructure/Kwetter.Services.Common.Infrastructure.csproj", "Services/Common/Infrastructure/"]
COPY ["Services/Common/Domain/Kwetter.Services.Common.Domain.csproj", "Services/Common/Domain/"]
COPY ["Services/Common/EventBus/Kwetter.Services.Common.EventBus.csproj", "Services/Common/EventBus/"]
COPY ["Services/FollowService/Domain/Kwetter.Services.FollowService.Domain.csproj", "Services/FollowService/Domain/"]
COPY ["Services/FollowService/Infrastructure/Kwetter.Services.FollowService.Infrastructure.csproj", "Services/FollowService/Infrastructure/"]
RUN dotnet restore "Services/FollowService/API/Kwetter.Services.FollowService.API.csproj"
COPY . .
WORKDIR "/src/Services/FollowService/API"
RUN dotnet build "Kwetter.Services.FollowService.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Kwetter.Services.FollowService.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Kwetter.Services.FollowService.API.dll"]