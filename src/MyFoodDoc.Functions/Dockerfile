#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/azure-functions/dotnet-isolated:4-dotnet-isolated6.0-appservice AS base
WORKDIR /home/site/wwwroot
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["src/MyFoodDoc.Functions/MyFoodDoc.Functions.csproj", "src/MyFoodDoc.Functions/"]
COPY ["src/MyFoodDoc.Core/MyFoodDoc.Core.csproj", "src/MyFoodDoc.Core/"]
COPY ["src/MyFoodDoc.GooglePlayStoreClient/MyFoodDoc.GooglePlayStoreClient.csproj", "src/MyFoodDoc.GooglePlayStoreClient/"]
COPY ["src/MyFoodDoc.Application/MyFoodDoc.Application.csproj", "src/MyFoodDoc.Application/"]
COPY ["src/MyFoodDoc.FatSecretClient/MyFoodDoc.FatSecretClient.csproj", "src/MyFoodDoc.FatSecretClient/"]
COPY ["src/MyFoodDoc.FirebaseClient/MyFoodDoc.FirebaseClient.csproj", "src/MyFoodDoc.FirebaseClient/"]
COPY ["src/MyFoodDoc.AppStoreClient/MyFoodDoc.AppStoreClient.csproj", "src/MyFoodDoc.AppStoreClient/"]
COPY ["src/MyFoodDoc.Infrastructure/MyFoodDoc.Infrastructure.csproj", "src/MyFoodDoc.Infrastructure/"]

# https://github.com/Azure/azure-functions-docker/issues/600
RUN apt-get update -y \
  && apt-get install -y apt-transport-https wget \
  && wget https://packages.microsoft.com/config/debian/11/packages-microsoft-prod.deb -O packages-microsoft-prod.deb \
  && dpkg -i packages-microsoft-prod.deb \
  && apt-get update -y \
  && apt-get install -y aspnetcore-runtime-6.0

RUN dotnet restore "src/MyFoodDoc.Functions/MyFoodDoc.Functions.csproj"
COPY . .

WORKDIR "/src/src/MyFoodDoc.Functions"
RUN dotnet build "MyFoodDoc.Functions.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "MyFoodDoc.Functions.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /home/site/wwwroot
COPY --from=publish /app/publish .
ENV AzureWebJobsScriptRoot=/home/site/wwwroot \
    AzureFunctionsJobHost__Logging__Console__IsEnabled=true