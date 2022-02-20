#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["AudioDbWebServer/AudioDbWebServer.csproj", "AudioDbWebServer/"]
RUN dotnet restore "AudioDbWebServer/AudioDbWebServer.csproj"
COPY . .
WORKDIR "/src/AudioDbWebServer"
RUN dotnet build "AudioDbWebServer.csproj" -c Release -o /app/build --no-restore

FROM build AS publish
RUN dotnet publish "AudioDbWebServer.csproj" -c Release -o /app/publish --no-restore

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "AudioDbWebServer.dll"]