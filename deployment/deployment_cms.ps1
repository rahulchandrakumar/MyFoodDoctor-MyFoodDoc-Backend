$tag                 = "latest"
$image               = "myfooddoc-mock-cms"

$registryName        = "myfooddocmockcmsregistry"
$registryServer      = "$registryName.azurecr.io"
$registryPw          = "bgxBJ7IUJj/7BmI4AGvpRMeZb0muWrbb"

$tag = $registryServer + "/" + $image + ":" + $tag

docker login $registryServer -u $registryName -p $registryPw

docker build --tag "$tag" -f ../src/MyFoodDoc.CMS/Dockerfile ../
docker push $tag