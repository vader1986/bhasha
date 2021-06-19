#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src

COPY ["Bhasha.Common/Bhasha.Common.csproj", "Bhasha.Common/"]
RUN dotnet restore "Bhasha.Common/Bhasha.Common.csproj"

COPY ["Bhasha.Student.Web/Bhasha.Student.Web.csproj", "Bhasha.Student.Web/"]
RUN dotnet restore "Bhasha.Student.Web/Bhasha.Student.Web.csproj"
COPY . .
WORKDIR "/src/Bhasha.Student.Web"
RUN dotnet build "Bhasha.Student.Web.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Bhasha.Student.Web.csproj" -c Release -o /app/publish

FROM nginx:alpine
WORKDIR /var/www/web
COPY --from=publish /app/publish/wwwroot .
COPY Bhasha.Author.Web/nginx.conf /etc/nginx/nginx.conf