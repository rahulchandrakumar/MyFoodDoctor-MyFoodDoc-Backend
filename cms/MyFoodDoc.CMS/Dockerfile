FROM mcr.microsoft.com/dotnet/core/aspnet:3.1 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build
WORKDIR /src
COPY ["cms/MyFoodDoc.CMS/MyFoodDoc.CMS.csproj", "cms/MyFoodDoc.CMS/"]
RUN dotnet restore "cms/MyFoodDoc.CMS/MyFoodDoc.CMS.csproj"
COPY . .
WORKDIR "/src/cms/MyFoodDoc.CMS"
RUN dotnet build "MyFoodDoc.CMS.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "MyFoodDoc.CMS.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MyFoodDoc.CMS.dll"]