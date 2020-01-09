#######################################################
# Terraform remote state
#######################################################
terraform {
  # we use azure blob storage as backing store for terraform state
  backend "azurerm" {
    resource_group_name  = "mfd-terraform"
    storage_account_name = "mfdtfstate"
    container_name       = "terraform"
    # make sure you adjust this per new variant, we can't use a variable here...
    # first string part should match this filename
    key = "shared.terraform.tfstate"
    # no access key here.. follow https://docs.microsoft.com/en-us/azure/terraform/terraform-backend#configure-state-backend
    # set ARM_ACCESS_KEY env variable to storage account key via `setx ARM_ACCESS_KEY <storage account key>`
  }
}


#######################################################
# Module + Config
#######################################################
module "global" {
  source = "../../modules/shared"

  projectname = "mfd"
}


#######################################################
# Outputs
#######################################################
output "containerregistry_url" {
  value = module.global.containerregistry_url
}
output "containerregistry_admin_username" {
  value = module.global.containerregistry_admin_username
}
output "containerregistry_admin_password" {
  value = module.global.containerregistry_admin_password
}
output "keyvault_id" {
  value = module.global.keyvault_id
}
output "keyvault_name" {
  value = module.global.keyvault_name
}
