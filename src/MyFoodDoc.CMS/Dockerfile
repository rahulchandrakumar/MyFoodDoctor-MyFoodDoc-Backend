#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["src/MyFoodDoc.CMS/MyFoodDoc.CMS.csproj", "src/MyFoodDoc.CMS/"]
COPY ["src/MyFoodDoc.Core/MyFoodDoc.Core.csproj", "src/MyFoodDoc.Core/"]
COPY ["src/MyFoodDoc.FatSecretClient/MyFoodDoc.FatSecretClient.csproj", "src/MyFoodDoc.FatSecretClient/"]
COPY ["src/MyFoodDoc.CMS.Application/MyFoodDoc.CMS.Application.csproj", "src/MyFoodDoc.CMS.Application/"]
COPY ["src/MyFoodDoc.Application/MyFoodDoc.Application.csproj", "src/MyFoodDoc.Application/"]
COPY ["src/MyFoodDoc.CMS.Infrastructure/MyFoodDoc.CMS.Infrastructure.csproj", "src/MyFoodDoc.CMS.Infrastructure/"]
COPY ["src/MyFoodDoc.Infrastructure/MyFoodDoc.Infrastructure.csproj", "src/MyFoodDoc.Infrastructure/"]
RUN dotnet restore "src/MyFoodDoc.CMS/MyFoodDoc.CMS.csproj"
COPY . .
WORKDIR "/src/src/MyFoodDoc.CMS"
RUN dotnet build "MyFoodDoc.CMS.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "MyFoodDoc.CMS.csproj" -c Release -o /app/publish

FROM node:12-alpine as webbuild
WORKDIR /app
COPY src/MyFoodDoc.CMS/app/package*.json ./
RUN npm install
COPY src/MyFoodDoc.CMS/app/ .
RUN npm run build

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
COPY --from=webbuild /app/dist ./app/dist
ENTRYPOINT ["dotnet", "MyFoodDoc.CMS.dll"]