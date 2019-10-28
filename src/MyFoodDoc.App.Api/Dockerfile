FROM mcr.microsoft.com/dotnet/core/aspnet:3.0-buster-slim AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/core/sdk:3.0-buster AS build
WORKDIR /src
COPY ["src/MyFoodDoc.Application.Api/MyFoodDoc.Application.Api.csproj", "src/MyFoodDoc.Application.Api/"]
RUN dotnet restore "src/MyFoodDoc.Application.Api/MyFoodDoc.Application.Api.csproj"
COPY . .
WORKDIR "/src/src/MyFoodDoc.Application.Api"
RUN dotnet build "MyFoodDoc.Application.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "MyFoodDoc.Application.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MyFoodDoc.Application.Api.dll"]