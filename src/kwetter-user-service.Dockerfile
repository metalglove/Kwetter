FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["Services/UserService/API/Kwetter.Services.UserService.API.csproj", "Services/UserService/API/"]
COPY ["Services/Common/API/Kwetter.Services.Common.API.csproj", "Services/Common/API/"]
COPY ["Services/Common/Infrastructure/Kwetter.Services.Common.Infrastructure.csproj", "Services/Common/Infrastructure/"]
COPY ["Services/Common/Application/Kwetter.Services.Common.Application.csproj", "Services/Common/Application/"]
COPY ["Services/Common/Domain/Kwetter.Services.Common.Domain.csproj", "Services/Common/Domain/"]
COPY ["Services/UserService/Infrastructure/Kwetter.Services.UserService.Infrastructure.csproj", "Services/UserService/Infrastructure/"]
COPY ["Services/UserService/Domain/Kwetter.Services.UserService.Domain.csproj", "Services/UserService/Domain/"]
RUN dotnet restore "Services/UserService/API/Kwetter.Services.UserService.API.csproj"
COPY . .
WORKDIR "/src/Services/UserService/API"
RUN dotnet build "Kwetter.Services.UserService.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Kwetter.Services.UserService.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Kwetter.Services.UserService.API.dll"]