#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["Bhasha.Web/Bhasha.Web.csproj", "Bhasha.Web/"]
RUN dotnet restore "Bhasha.Web/Bhasha.Web.csproj"
COPY . .
WORKDIR "/src/Bhasha.Web"
RUN dotnet build "Bhasha.Web.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Bhasha.Web.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Bhasha.Web.dll"]
