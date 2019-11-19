$tag                 = "latest"
$image               = "myfooddoc-mock-cms-app"

$registryName        = "myfooddocmockregistry"
$registryServer      = "$registryName.azurecr.io"
$registryPw          = "ZUI+NMC9bpT3Rd+5lhoLWPpVZeZzUat8"

$tag = $registryServer + "/" + $image + ":" + $tag

docker login $registryServer -u $registryName -p $registryPw

docker build --tag "$tag" -f ../cms/MyFoodDoc.CMS/app/Dockerfile ../cms/MyFoodDoc.CMS/app
docker push $tag