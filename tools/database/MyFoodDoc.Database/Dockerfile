FROM busybox AS base
WORKDIR /

FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build
WORKDIR /src
COPY ["tools/database/MyFoodDoc.Database/MyFoodDoc.Database.csproj", "tools/database/MyFoodDoc.Database/"]
RUN dotnet restore "tools/database/MyFoodDoc.Database/MyFoodDoc.Database.csproj"
COPY . .
WORKDIR "/src/tools/database/MyFoodDoc.Database/"
RUN dotnet tool install dotnet-ef --tool-path "/tools/dotnet-ef"
ENV DEFAULT_DATABASE_CONNECTION="1"
RUN "/tools/dotnet-ef/dotnet-ef" migrations script --idempotent -o "/migrations.sql"
RUN sed -i "1iSET QUOTED_IDENTIFIER ON\n\n\GO\n" "/migrations.sql"

FROM base AS final
WORKDIR /
COPY --from=build "/migrations.sql" "/migrations.sql"
CMD ["sleep","5000"]