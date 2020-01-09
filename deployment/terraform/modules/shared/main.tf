#######################################################
# Variables & Locals
#######################################################
variable "location" {
  default = "West Europe"
}
variable "projectname" {}

locals {
  # see example of how to add additional tags per resource later: https://www.terraform.io/docs/configuration-0-11/locals.html#examples
  resource_tags = {
    "Project"     = var.projectname
    "Environment" = "shared"
    "Scope"       = "shared"
  }
}

data "azurerm_client_config" "current" {}

#######################################################
# Resources/Providers
#######################################################
provider "azurerm" {
  # Whilst version is optional, we /strongly recommend/ using it to pin the version of the Provider being used
  version         = "=1.38.0"
  # we need this, as in customer subscription only certain resource providers were registered...
  # without it, any terraform plan/apply would fail with a 403 on trying to access e.g. Media Services
  # see https://github.com/hashicorp/terraform/issues/18180#issuecomment-394369502
  skip_provider_registration = true
}



#######################################################
# Resource Group
#######################################################
resource "azurerm_resource_group" "rg" {
  name     = "${var.projectname}-shared"
  location = var.location
  tags     = local.resource_tags
}


#######################################################
# Container registry
#######################################################
resource "azurerm_container_registry" "container-registry" {
  name                = "${var.projectname}containers"
  resource_group_name = azurerm_resource_group.rg.name
  location            = azurerm_resource_group.rg.location
  tags                = local.resource_tags

  sku           = "Standard"
  admin_enabled = true
}

#######################################################
# Key Vault
#######################################################
resource "azurerm_key_vault" "keyvault" {
  name                = "${var.projectname}-kv-shared"
  resource_group_name = azurerm_resource_group.rg.name
  location            = azurerm_resource_group.rg.location
  tenant_id           = data.azurerm_client_config.current.tenant_id
  sku_name            = "standard"
  tags                = local.resource_tags
}

resource "azurerm_key_vault_access_policy" "devops" {
  key_vault_id = azurerm_key_vault.keyvault.id

  tenant_id = data.azurerm_client_config.current.tenant_id
  object_id = data.azurerm_client_config.current.object_id
  secret_permissions = [
    "set", "get", "delete",
  ]
}


#######################################################
# Outputs
#######################################################
output "containerregistry_url" {
  value = azurerm_container_registry.container-registry.login_server
}
output "containerregistry_admin_username" {
  value = azurerm_container_registry.container-registry.admin_username
}
output "containerregistry_admin_password" {
  value = azurerm_container_registry.container-registry.admin_password
}
output "keyvault_id" {
  value = azurerm_key_vault.keyvault.id
}
output "keyvault_name" {
  value = azurerm_key_vault.keyvault.name
}