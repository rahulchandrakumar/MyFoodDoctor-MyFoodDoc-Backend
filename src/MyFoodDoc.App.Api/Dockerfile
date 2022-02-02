#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app

RUN apt-get update && apt-get install -y libfontconfig1

EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["src/MyFoodDoc.App.Api/MyFoodDoc.App.Api.csproj", "src/MyFoodDoc.App.Api/"]
COPY ["src/MyFoodDoc.Core/MyFoodDoc.Core.csproj", "src/MyFoodDoc.Core/"]
COPY ["src/MyFoodDoc.App.Application/MyFoodDoc.App.Application.csproj", "src/MyFoodDoc.App.Application/"]
COPY ["src/MyFoodDoc.Application/MyFoodDoc.Application.csproj", "src/MyFoodDoc.Application/"]
COPY ["src/MyFoodDoc.GooglePlayStoreClient/MyFoodDoc.GooglePlayStoreClient.csproj", "src/MyFoodDoc.GooglePlayStoreClient/"]
COPY ["src/MyFoodDoc.FatSecretClient/MyFoodDoc.FatSecretClient.csproj", "src/MyFoodDoc.FatSecretClient/"]
COPY ["src/MyFoodDoc.AokClient/MyFoodDoc.AokClient.csproj", "src/MyFoodDoc.AokClient/"]
COPY ["src/MyFoodDoc.AppStoreClient/MyFoodDoc.AppStoreClient.csproj", "src/MyFoodDoc.AppStoreClient/"]
COPY ["src/MyFoodDoc.App.Infrastructure/MyFoodDoc.App.Infrastructure.csproj", "src/MyFoodDoc.App.Infrastructure/"]
COPY ["src/MyFoodDoc.Infrastructure/MyFoodDoc.Infrastructure.csproj", "src/MyFoodDoc.Infrastructure/"]
RUN dotnet restore "src/MyFoodDoc.App.Api/MyFoodDoc.App.Api.csproj"
COPY . .
WORKDIR "/src/src/MyFoodDoc.App.Api"
RUN dotnet build "MyFoodDoc.App.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "MyFoodDoc.App.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MyFoodDoc.App.Api.dll"]