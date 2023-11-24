FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["SistemaVenta.Api/SistemaVenta.Api.csproj", "SistemaVenta.Api/"]
RUN dotnet restore "SistemaVenta.Api/SistemaVenta.Api.csproj"
COPY . .
WORKDIR "/src/SistemaVenta.Api"
RUN dotnet build "SistemaVenta.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "SistemaVenta.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "SistemaVenta.Api.dll"]
