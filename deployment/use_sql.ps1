docker pull mcr.microsoft.com/mssql-tools
docker run -d mcr.microsoft.com/mssql-tools
$CONTAINER_ID="$(docker ps -alq)"
docker cp migrations.sql ${CONTAINER_ID}:/migrations.sql
docker exec $CONTAINER_ID -it sqlcmd -?
docker stop -t 0 $CONTAINER_ID