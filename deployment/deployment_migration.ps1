docker build --tag "mfd-dbmigration:latest" -f ../tools/database/MyFoodDoc.Database/Dockerfile ../
docker run -d "mfd-dbmigration:latest"
$CONTAINER_ID = (docker ps -alq)
docker cp ${CONTAINER_ID}:/migrations.sql .
docker stop -t 0 $CONTAINER_ID