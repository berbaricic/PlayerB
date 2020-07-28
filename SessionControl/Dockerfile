#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

#FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
#WORKDIR /app
#EXPOSE 80
#EXPOSE 443
#
#FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
#WORKDIR /src
#COPY ["SessionControl/SessionControl.csproj", "SessionControl/"]
#RUN dotnet restore "SessionControl/SessionControl.csproj"
#COPY . .
#WORKDIR "/src/SessionControl"
#RUN dotnet build "SessionControl.csproj" -c Release -o /app/build
#
#FROM build AS publish
#RUN dotnet publish "SessionControl.csproj" -c Release -o /app/publish
#
#FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build-env
#ENV PATH $PATH:/root/.dotnet/tools
#RUN dotnet tool install -g dotnet-ef --version 3.1.1
#
#FROM base AS final
#WORKDIR /app
#COPY --from=publish /app/publish .
#ENTRYPOINT ["dotnet", "SessionControl.dll"]

FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build-env
WORKDIR /app

# Copy csproj and restore as distinct layers
COPY *.csproj ./
RUN dotnet restore

# Copy everything else and build
COPY . ./
RUN dotnet publish -c Release -o out

# Build runtime image
FROM mcr.microsoft.com/dotnet/core/aspnet:3.1
WORKDIR /app
COPY --from=build-env /app/out .
ENTRYPOINT ["dotnet", "aspnetapp.dll"]