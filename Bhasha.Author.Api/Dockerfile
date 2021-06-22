#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src

COPY ["Bhasha.Common/Bhasha.Common.csproj", "Bhasha.Common/"]
RUN dotnet restore "Bhasha.Common/Bhasha.Common.csproj"

COPY ["Bhasha.Common.MongoDB/Bhasha.Common.MongoDB.csproj", "Bhasha.Common.MongoDB/"]
RUN dotnet restore "Bhasha.Common.MongoDB/Bhasha.Common.MongoDB.csproj"

COPY ["Bhasha.Common.Api/Bhasha.Common.Api.csproj", "Bhasha.Common.Api/"]
RUN dotnet restore "Bhasha.Common.Api/Bhasha.Common.Api.csproj"

COPY ["Bhasha.Author.Api/Bhasha.Author.Api.csproj", "Bhasha.Author.Api/"]
RUN dotnet restore "Bhasha.Author.Api/Bhasha.Author.Api.csproj"
COPY . .
WORKDIR "/src/Bhasha.Author.Api"
RUN dotnet build "Bhasha.Author.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Bhasha.Author.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Bhasha.Author.Api.dll"]
