$tag                 = "latest"
$image               = "myfooddoc-mock-func"

$registryName        = "myfooddocmockregistry"
$registryServer      = "$registryName.azurecr.io"
$registryPw          = "ZUI+NMC9bpT3Rd+5lhoLWPpVZeZzUat8"

$tag = $registryServer + "/" + $image + ":" + $tag

docker login $registryServer -u $registryName -p $registryPw

docker build --tag "$tag" -f ../src/MyFoodDoc.FatSecretSynchronization/Dockerfile ../
docker push $tag