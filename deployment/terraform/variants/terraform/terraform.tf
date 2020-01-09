provider "azurerm" {
  # Whilst version is optional, we /strongly recommend/ using it to pin the version of the Provider being used
  version         = "=1.38.0"
  # we need this, as in customer subscription only certain resource providers were registered...
  # without it, any terraform plan/apply would fail with a 403 on trying to access e.g. Media Services
  # see https://github.com/hashicorp/terraform/issues/18180#issuecomment-394369502
  skip_provider_registration = false
}

locals {
  projectname = "mfd"
  storageName = "${local.projectname}tfstate"
  storageContainer = "terraform"
}

resource "azurerm_resource_group" "rg" {
  name     = "${local.projectname}-terraform"
  location = "West Europe"
}

resource "azurerm_storage_account" "terraform" {
  name                     = local.storageName
  resource_group_name      = azurerm_resource_group.rg.name
  location                 = azurerm_resource_group.rg.location
  account_tier             = "Standard"
  account_replication_type = "LRS"
  account_kind             = "BlobStorage"
  access_tier              = "Cool"

  tags = {
    environment = "terraform"
  }
}

resource "azurerm_storage_container" "container" {
  name                  = local.storageContainer
  storage_account_name  = azurerm_storage_account.terraform.name
  container_access_type = "private"
}

data "azurerm_storage_account" "storage" {
  name                     = azurerm_storage_account.terraform.name
  resource_group_name      = azurerm_resource_group.rg.name
}

#######################################################
# Outputs
#######################################################
output "storage_accesskey" {
  value = data.azurerm_storage_account.storage.primary_access_key
}