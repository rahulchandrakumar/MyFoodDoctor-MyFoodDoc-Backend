$tag                 = "latest"
$image               = "mfd-func"

$registryName        = "mfdcontainers"
$registryServer      = "$registryName.azurecr.io"
$registryPw          = "cwyUs0oKvHibAIEJSSAuo=WlfYG82Jvr"

$tag = $registryServer + "/" + $image + ":" + $tag

docker login $registryServer -u $registryName -p $registryPw

docker build --tag "$tag" -f ../src/MyFoodDoc.Functions/Dockerfile ../
docker push $tag