FROM mcr.microsoft.com/dotnet/aspnet:5.0-buster-slim AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:5.0-buster-slim AS build
WORKDIR /src
COPY ["Kwetter.Services/Kwetter.Services.UserService/Kwetter.Services.UserService.API/Kwetter.Services.UserService.API.csproj", "Kwetter.Services/Kwetter.Services.UserService/Kwetter.Services.UserService.API/"]
COPY ["Kwetter.Services/Kwetter.Services.Common/Kwetter.Services.Common.API/Kwetter.Services.Common.API.csproj", "Kwetter.Services/Kwetter.Services.Common/Kwetter.Services.Common.API/"]
COPY ["Kwetter.Services/Kwetter.Services.Common/Kwetter.Services.Common.Infrastructure/Kwetter.Services.Common.Infrastructure.csproj", "Kwetter.Services/Kwetter.Services.Common/Kwetter.Services.Common.Infrastructure/"]
COPY ["Kwetter.Services/Kwetter.Services.Common/Kwetter.Services.Common.Domain/Kwetter.Services.Common.Domain.csproj", "Kwetter.Services/Kwetter.Services.Common/Kwetter.Services.Common.Domain/"]
COPY ["Kwetter.Services/Kwetter.Services.Common/Kwetter.Services.Common.EventBus/Kwetter.Services.Common.EventBus.csproj", "Kwetter.Services/Kwetter.Services.Common/Kwetter.Services.Common.EventBus/"]
COPY ["Kwetter.Services/Kwetter.Services.UserService/Kwetter.Services.UserService.Domain/Kwetter.Services.UserService.Domain.csproj", "Kwetter.Services/Kwetter.Services.UserService/Kwetter.Services.UserService.Domain/"]
COPY ["Kwetter.Services/Kwetter.Services.UserService/Kwetter.Services.UserService.Infrastructure/Kwetter.Services.UserService.Infrastructure.csproj", "Kwetter.Services/Kwetter.Services.UserService/Kwetter.Services.UserService.Infrastructure/"]
RUN dotnet restore "Kwetter.Services/Kwetter.Services.UserService/Kwetter.Services.UserService.API/Kwetter.Services.UserService.API.csproj"
COPY . .
WORKDIR "/src/Kwetter.Services/Kwetter.Services.UserService/Kwetter.Services.UserService.API"
RUN dotnet build "Kwetter.Services.UserService.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Kwetter.Services.UserService.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Kwetter.Services.UserService.API.dll"]