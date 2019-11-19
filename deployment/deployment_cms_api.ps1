$tag                 = "latest"
$image               = "myfooddoc-mock-cms-api"

$registryName        = "myfooddocmockregistry"
$registryServer      = "$registryName.azurecr.io"
$registryPw          = "ZUI+NMC9bpT3Rd+5lhoLWPpVZeZzUat8"

$tag = $registryServer + "/" + $image + ":" + $tag

docker login $registryServer -u $registryName -p $registryPw

docker build --tag "$tag" -f ../cms/MyFoodDoc.CMS/Dockerfile ../
docker push $tag