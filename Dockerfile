FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY ["ShortnerUrl.Api/ShortnerUrl.Api.csproj", "ShortnerUrl.Api/"]
RUN dotnet restore "ShortnerUrl.Api/ShortnerUrl.Api.csproj"

COPY . .
WORKDIR "/src/ShortnerUrl.Api"
RUN dotnet build "ShortnerUrl.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ShortnerUrl.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

COPY --from=publish /app/publish .

ENV ASPNETCORE_URLS=http://+:8080
ENV ASPNETCORE_ENVIRONMENT=Production

ENTRYPOINT ["dotnet", "ShortnerUrl.Api.dll"]