FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

LABEL org.opencontainers.image.source https://github.com/moraisLuismNet/eCommerceDs_Docker

COPY ["eCommerceDs.csproj", "./"]
RUN dotnet restore

COPY . .
RUN dotnet build -c Release -o /app/build

FROM build AS publish
RUN dotnet publish -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=publish /app/publish .

RUN mkdir -p /app/wwwroot/img && \
    chmod 755 /app/wwwroot/img

ENTRYPOINT ["dotnet", "eCommerceDs.dll"]