FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["Services/AuthorizationService/API/Kwetter.Services.AuthorizationService.API.csproj", "Services/AuthorizationService/API/"]
COPY ["Services/Common/API/Kwetter.Services.Common.API.csproj", "Services/Common/API/"]
COPY ["Services/Common/Infrastructure/Kwetter.Services.Common.Infrastructure.csproj", "Services/Common/Infrastructure/"]
COPY ["Services/Common/Application/Kwetter.Services.Common.Application.csproj", "Services/Common/Application/"]
COPY ["Services/Common/Domain/Kwetter.Services.Common.Domain.csproj", "Services/Common/Domain/"]
COPY ["Services/AuthorizationService/Infrastructure/Kwetter.Services.AuthorizationService.Infrastructure.csproj", "Services/AuthorizationService/Infrastructure/"]
COPY ["Services/AuthorizationService/Domain/Kwetter.Services.AuthorizationService.Domain.csproj", "Services/AuthorizationService/Domain/"]
RUN dotnet restore "Services/AuthorizationService/API/Kwetter.Services.AuthorizationService.API.csproj"
COPY . .
WORKDIR "/src/Services/AuthorizationService/API"
RUN dotnet build "Kwetter.Services.AuthorizationService.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Kwetter.Services.AuthorizationService.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Kwetter.Services.AuthorizationService.API.dll"]