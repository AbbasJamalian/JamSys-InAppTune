#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["JamSys.InAppTune.Host/JamSys.InAppTune.Host.csproj", "JamSys.InAppTune.Host/"]
RUN dotnet restore "JamSys.InAppTune.Host/JamSys.InAppTune.Host.csproj"
COPY . .
WORKDIR "/src/JamSys.InAppTune.Host"
RUN dotnet build "JamSys.InAppTune.Host.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "JamSys.InAppTune.Host.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "JamSys.InAppTune.Host.dll"]