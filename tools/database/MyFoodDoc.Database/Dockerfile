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
RUN "/tools/dotnet-ef/dotnet-ef" migrations script --idempotent -o "/migrations.sql" --context ApplicationContext ;\
"/tools/dotnet-ef/dotnet-ef" migrations script --idempotent -o "/migrations_identityserver_configuration.sql" --context ConfigurationDbContext ;\
"/tools/dotnet-ef/dotnet-ef" migrations script --idempotent -o "/migrations_identityserver_persistedgrant.sql" --context PersistedGrantDbContext

WORKDIR /
RUN sed -i "1s/^\xEF\xBB\xBF/\xEF\xBB\xBFSET QUOTED_IDENTIFIER ON\n\n\GO\n\n/" migrations.sql ;\
sed -i "1s/^\xEF\xBB\xBF/\xEF\xBB\xBFSET QUOTED_IDENTIFIER ON\n\n\GO\n\n/" migrations_identityserver_configuration.sql ;\
sed -i "1s/^\xEF\xBB\xBF/\xEF\xBB\xBFSET QUOTED_IDENTIFIER ON\n\n\GO\n\n/" migrations_identityserver_persistedgrant.sql

FROM base AS final
WORKDIR /
COPY --from=build "/migrations.sql" "/migrations.sql"
COPY --from=build "/migrations_identityserver_configuration.sql" "/migrations_identityserver_configuration.sql"
COPY --from=build "/migrations_identityserver_persistedgrant.sql" "/migrations_identityserver_persistedgrant.sql"
CMD ["sleep","5000"]