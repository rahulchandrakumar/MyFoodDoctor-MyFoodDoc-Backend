FROM mcr.microsoft.com/dotnet/core/aspnet:3.1 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build
WORKDIR /src
COPY ["src/MyFoodDoc.CMS/MyFoodDoc.CMS.csproj", "src/MyFoodDoc.CMS/"]
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