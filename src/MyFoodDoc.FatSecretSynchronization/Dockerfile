#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/azure-functions/dotnet:3.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build
WORKDIR /src
COPY ["src/MyFoodDoc.FatSecretSynchronization/MyFoodDoc.FatSecretSynchronization.csproj", "src/MyFoodDoc.FatSecretSynchronization/"]
COPY ["src/MyFoodDoc.Core/MyFoodDoc.Core.csproj", "src/MyFoodDoc.Core/"]
COPY ["src/MyFoodDoc.FatSecretClient/MyFoodDoc.FatSecretClient.csproj", "src/MyFoodDoc.FatSecretClient/"]
COPY ["src/MyFoodDoc.Infrastructure/MyFoodDoc.Infrastructure.csproj", "src/MyFoodDoc.Infrastructure/"]
COPY ["src/MyFoodDoc.Application/MyFoodDoc.Application.csproj", "src/MyFoodDoc.Application/"]
RUN dotnet restore "src/MyFoodDoc.FatSecretSynchronization/MyFoodDoc.FatSecretSynchronization.csproj"
COPY . .
WORKDIR "/src/src/MyFoodDoc.FatSecretSynchronization"
RUN dotnet build "MyFoodDoc.FatSecretSynchronization.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "MyFoodDoc.FatSecretSynchronization.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /home/site/wwwroot
COPY --from=publish /app/publish .
ENV AzureWebJobsScriptRoot=/home/site/wwwroot \
    AzureFunctionsJobHost__Logging__Console__IsEnabled=true