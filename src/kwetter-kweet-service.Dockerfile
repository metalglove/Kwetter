FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["Services/KweetService/API/Kwetter.Services.KweetService.API.csproj", "Services/KweetService/API/"]
COPY ["Services/Common/API/Kwetter.Services.Common.API.csproj", "Services/Common/API/"]
COPY ["Services/Common/Infrastructure/Kwetter.Services.Common.Infrastructure.csproj", "Services/Common/Infrastructure/"]
COPY ["Services/Common/Application/Kwetter.Services.Common.Application.csproj", "Services/Common/Application/"]
COPY ["Services/Common/Domain/Kwetter.Services.Common.Domain.csproj", "Services/Common/Domain/"]
COPY ["Services/KweetService/Infrastructure/Kwetter.Services.KweetService.Infrastructure.csproj", "Services/KweetService/Infrastructure/"]
COPY ["Services/KweetService/Domain/Kwetter.Services.KweetService.Domain.csproj", "Services/KweetService/Domain/"]
RUN dotnet restore "Services/KweetService/API/Kwetter.Services.KweetService.API.csproj"
COPY . .
WORKDIR "/src/Services/KweetService/API"
RUN dotnet build "Kwetter.Services.KweetService.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Kwetter.Services.KweetService.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Kwetter.Services.KweetService.API.dll"]