FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["Services/NotificationService/API/Kwetter.Services.NotificationService.API.csproj", "Services/NotificationService/API/"]
COPY ["Services/Common/API/Kwetter.Services.Common.API.csproj", "Services/Common/API/"]
COPY ["Services/Common/Infrastructure/Kwetter.Services.Common.Infrastructure.csproj", "Services/Common/Infrastructure/"]
COPY ["Services/Common/Application/Kwetter.Services.Common.Application.csproj", "Services/Common/Application/"]
COPY ["Services/Common/Domain/Kwetter.Services.Common.Domain.csproj", "Services/Common/Domain/"]
COPY ["Services/NotificationService/Infrastructure/Kwetter.Services.NotificationService.Infrastructure.csproj", "Services/NotificationService/Infrastructure/"]
COPY ["Services/NotificationService/Domain/Kwetter.Services.NotificationService.Domain.csproj", "Services/NotificationService/Domain/"]
RUN dotnet restore "Services/NotificationService/API/Kwetter.Services.NotificationService.API.csproj"
COPY . .
WORKDIR "/src/Services/NotificationService/API"
RUN dotnet build "Kwetter.Services.NotificationService.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Kwetter.Services.NotificationService.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Kwetter.Services.NotificationService.API.dll"]