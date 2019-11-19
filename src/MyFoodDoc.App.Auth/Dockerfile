FROM mcr.microsoft.com/dotnet/core/aspnet:3.0-buster-slim AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/core/sdk:3.0-buster AS build
WORKDIR /src
COPY ["src/MyFoodDoc.App.Auth/MyFoodDoc.App.Auth.csproj", "src/MyFoodDoc.App.Auth/"]
COPY ["src/MyFoodDoc.App.Application/MyFoodDoc.App.Application.csproj", "src/MyFoodDoc.App.Application/"]
COPY ["src/MyFoodDoc.Core/MyFoodDoc.Core.csproj", "src/MyFoodDoc.Core/"]
COPY ["src/MyFoodDoc.Application/MyFoodDoc.Application.csproj", "src/MyFoodDoc.Application/"]
COPY ["src/MyFoodDoc.App.Infrastructure/MyFoodDoc.App.Infrastructure.csproj", "src/MyFoodDoc.App.Infrastructure/"]
COPY ["src/MyFoodDoc.Infrastructure/MyFoodDoc.Infrastructure.csproj", "src/MyFoodDoc.Infrastructure/"]
RUN dotnet restore "src/MyFoodDoc.App.Auth/MyFoodDoc.App.Auth.csproj"
COPY . .
WORKDIR "/src/src/MyFoodDoc.App.Auth"
RUN dotnet build "MyFoodDoc.App.Auth.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "MyFoodDoc.App.Auth.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MyFoodDoc.App.Auth.dll"]