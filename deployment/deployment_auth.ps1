$tag                 = "latest"
$image               = "myfooddoc-mock-auth-api"

$registryName        = "myfooddocmockregistry"
$registryServer      = "$registryName.azurecr.io"
$registryPw          = "ZUI+NMC9bpT3Rd+5lhoLWPpVZeZzUat8"

$tag = $registryServer + "/" + $image + ":" + $tag

docker login $registryServer -u $registryName -p $registryPw

docker build --tag "$tag" -f ../src/MyFoodDoc.App.Auth/Dockerfile ../
docker push $tag