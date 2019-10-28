FROM mcr.microsoft.com/dotnet/core/aspnet:3.0-buster-slim AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/core/sdk:3.0-buster AS build
WORKDIR /src
COPY ["src/MyFoodDoc.Auth.Api/MyFoodDoc.Auth.Api.csproj", "src/MyFoodDoc.Auth.Api/"]
RUN dotnet restore "src/MyFoodDoc.Auth.Api/MyFoodDoc.Auth.Api.csproj"
COPY . .
WORKDIR "/src/src/MyFoodDoc.Auth.Api"
RUN dotnet build "MyFoodDoc.Auth.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "MyFoodDoc.Auth.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MyFoodDoc.Auth.Api.dll"]