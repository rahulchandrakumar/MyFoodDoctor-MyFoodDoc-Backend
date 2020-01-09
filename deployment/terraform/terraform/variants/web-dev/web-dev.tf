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
    key = "web-dev.terraform.tfstate"
    # no access key here.. follow https://docs.microsoft.com/en-us/azure/terraform/terraform-backend#configure-state-backend
    # set ARM_ACCESS_KEY env variable to storage account key via `setx ARM_ACCESS_KEY <storage account key>`
  }
}

#######################################################
# Remote state injection
#######################################################
data "terraform_remote_state" "shared" {
  backend = "azurerm"
  config = {
    resource_group_name  = "mfd-terraform"
    storage_account_name = "mfdtfstate"
    container_name       = "terraform"
    key                  = "shared.terraform.tfstate"
  }
}

#######################################################
# Module + Config
#######################################################
module "global" {
  source = "../../modules/web"

  projectname = "mfd"
  stage       = "dev2"

  apiserver_plantier     = "Basic"
  apiserver_plansize     = "B1"
  apiserver_plancapacity = 1

  apiapp_alwayson        = false
  apiapp_aspenv          = "Development"

  sqldb_tier             = "Basic"

  storageaccount_replication_type = "LRS"
  storageaccount_tier = "Standard"

  keyvault_id            = data.terraform_remote_state.shared.outputs.keyvault_id
  keyvault_name          = data.terraform_remote_state.shared.outputs.keyvault_name

  containerregistry_url            = data.terraform_remote_state.shared.outputs.containerregistry_url
  containerregistry_admin_username = data.terraform_remote_state.shared.outputs.containerregistry_admin_username
  containerregistry_admin_password = data.terraform_remote_state.shared.outputs.containerregistry_admin_password
}