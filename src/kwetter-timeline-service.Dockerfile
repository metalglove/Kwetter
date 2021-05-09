FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["Services/TimelineService/API/Kwetter.Services.TimelineService.API.csproj", "Services/TimelineService/API/"]
COPY ["Services/Common/API/Kwetter.Services.Common.API.csproj", "Services/Common/API/"]
COPY ["Services/Common/Infrastructure/Kwetter.Services.Common.Infrastructure.csproj", "Services/Common/Infrastructure/"]
COPY ["Services/Common/Application/Kwetter.Services.Common.Application.csproj", "Services/Common/Application/"]
COPY ["Services/Common/Domain/Kwetter.Services.Common.Domain.csproj", "Services/Common/Domain/"]
COPY ["Services/TimelineService/Infrastructure/Kwetter.Services.TimelineService.Infrastructure.csproj", "Services/TimelineService/Infrastructure/"]
COPY ["Services/TimelineService/Domain/Kwetter.Services.TimelineService.Domain.csproj", "Services/TimelineService/Domain/"]
RUN dotnet restore "Services/TimelineService/API/Kwetter.Services.TimelineService.API.csproj"
COPY . .
WORKDIR "/src/Services/TimelineService/API"
RUN dotnet build "Kwetter.Services.TimelineService.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Kwetter.Services.TimelineService.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Kwetter.Services.TimelineService.API.dll"]