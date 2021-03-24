FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /
COPY ["Bhasha.Web/Bhasha.Web.csproj", "Bhasha.Web/"]
COPY ["Bhasha.Common/Bhasha.Common.csproj", "Bhasha.Common/"]
COPY ["Bhasha.Common.MongoDB/Bhasha.Common.MongoDB.csproj", "Bhasha.Common.MongoDB/"]

RUN dotnet restore "Bhasha.Web/Bhasha.Web.csproj"
COPY . .

WORKDIR "/Bhasha.Web"
RUN dotnet build "Bhasha.Web.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Bhasha.Web.csproj" -c Release -o /app/publish

FROM base AS final

# Copy files of JS React App
COPY Bhasha.Web.Client/build/static/js/ /app/wwwroot/static/js/
COPY Bhasha.Web.Client/build/static/css/ /app/wwwroot/static/css/
COPY Bhasha.Web.Client/build/ /app/wwwroot/

RUN ls -la /app/wwwroot
RUN ls -la /app/wwwroot/static/js
RUN ls -la /app/wwwroot/static/css

WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Bhasha.Web.dll"]
