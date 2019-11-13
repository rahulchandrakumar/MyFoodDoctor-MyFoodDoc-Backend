$tag                 = "latest"
$image               = "myfooddoc-mock-cms-app"

$registryName        = "myfooddocmockcmsregistry"
$registryServer      = "$registryName.azurecr.io"
$registryPw          = "bgxBJ7IUJj/7BmI4AGvpRMeZb0muWrbb"

$tag = $registryServer + "/" + $image + ":" + $tag

docker login $registryServer -u $registryName -p $registryPw

docker build --tag "$tag" -f ../cms/MyFoodDoc.CMS/app/Dockerfile ../cms/MyFoodDoc.CMS/app
docker push $tag