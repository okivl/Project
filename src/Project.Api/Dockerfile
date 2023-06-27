FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["src/Project.Api/Project.Api.csproj", "src/Project.Api/"]
COPY ["src/Project.Core/Project.Core.csproj", "src/Project.Core/"]
COPY ["src/Project.Infrastructure/Project.Infrastructure.csproj", "src/Project.Infrastructure/"]
COPY ["src/Project.Entities/Project.Entities.csproj", "src/Project.Entities/"]
RUN dotnet restore "src/Project.Api/Project.Api.csproj"
COPY . .
WORKDIR "src/Project.Api"
RUN dotnet build "Project.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Project.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Project.Api.dll"]