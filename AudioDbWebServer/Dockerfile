#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

#Depending on the operating system of the host machines(s) that will build or run the containers, the image specified in the FROM statement may need to be changed.
#For more information, please see https://aka.ms/containercompat

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["AudioDbWebServer/AudioDbWebServer.csproj", "AudioDbWebServer/"]
RUN dotnet restore "AudioDbWebServer/AudioDbWebServer.csproj"
COPY . .
WORKDIR "/src/AudioDbWebServer"
RUN dotnet build "AudioDbWebServer.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "AudioDbWebServer.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "AudioDbWebServer.dll"]