#main.tf
provider "azurerm" {
  version = "=1.38.0"
}

provider "random" {
  version = ">=2.2"
}

resource "random_string" "random" {
  length = 16
  special = false
}

resource "random_password" "password" {
  length = 16
  special = true
  override_special = "_%@"
}

locals {
  environment = "${lookup(var.workspace_to_environment_map, terraform.workspace, "dev")}"
  sqlSize = "${var.environment_to_sqlsize_map[local.environment]}"
  planTier = "${var.environment_to_plantier_map[local.environment]}"
  planSize = "${var.environment_to_plansize_map[local.environment]}"
  aspEnv = "${var.environment_to_aspenv_map[local.environment]}"
  dbadmin = "${random_string.random.result}"
  dbpassword = "${random_password.password.result}"
  sqlServerName = "${var.project}sqlserver${local.environment}"
  sqlDbName = "${var.project}sqldb${local.environment}"
  storageName = "${var.project}storageacc${local.environment}"
  keyvaultName = "${var.project}-keyvault-${local.environment}"
  keyvaultDbKey = "SqlConnection"
  keyvaultStorKey = "StorageConnection"
}

resource "azurerm_resource_group" "rg" {
  name     = "${var.project}-${local.environment}"
  location = var.location
  tags     = var.tags
}

resource "azurerm_container_registry" "acr" {
  name                     = "${var.project}containerregistry${local.environment}"
  resource_group_name      = azurerm_resource_group.rg.name
  location                 = azurerm_resource_group.rg.location
  sku                      = "Basic"
  admin_enabled            = true
}

data "azurerm_container_registry" "acr" {
  name                     = azurerm_container_registry.acr.name
  resource_group_name      = azurerm_resource_group.rg.name
}

data "azurerm_client_config" "current" {}

resource "azurerm_sql_server" "sqlserver" {
  name                         = local.sqlServerName
  resource_group_name          = azurerm_resource_group.rg.name
  location                     = azurerm_resource_group.rg.location
  version                      = "12.0"
  administrator_login          = local.dbadmin
  administrator_login_password = local.dbpassword
}

resource "azurerm_sql_firewall_rule" "sqlfirewall" {
  name                = "Allow All Azure Service"
  resource_group_name = azurerm_sql_server.sqlserver.resource_group_name
  server_name         = azurerm_sql_server.sqlserver.name
  start_ip_address    = "0.0.0.0"
  end_ip_address      = "0.0.0.0"
}

resource "azurerm_sql_database" "sqldb" {
  name                = local.sqlDbName
  resource_group_name = azurerm_resource_group.rg.name
  location            = azurerm_resource_group.rg.location
  server_name         = azurerm_sql_server.sqlserver.name
  edition             = local.sqlSize
}

resource "azurerm_storage_account" "storage" {
  name                     = local.storageName
  resource_group_name      = azurerm_resource_group.rg.name
  location                 = azurerm_resource_group.rg.location
  account_tier             = "Standard"
  account_replication_type = "LRS"
  account_kind             = "BlobStorage"
}

data "azurerm_storage_account" "storage" {
  name                     = azurerm_storage_account.storage.name
  resource_group_name      = azurerm_resource_group.rg.name
}

resource "azurerm_cdn_profile" "cdnprofile" {
  name                = "${var.project}-cdnprofile-${local.environment}"
  resource_group_name = azurerm_resource_group.rg.name
  location            = azurerm_resource_group.rg.location
  sku                 = "Standard_Microsoft"
}

resource "azurerm_cdn_endpoint" "cdnendpoint" {
  name                = "${var.project}-cdnendpoint-${local.environment}"
  profile_name        = azurerm_cdn_profile.cdnprofile.name
  resource_group_name = azurerm_resource_group.rg.name
  location            = azurerm_resource_group.rg.location

  origin {
    name      = "StorageCDN"
    host_name = "${local.storageName}.blob.core.windows.net"
  }
}

resource "azurerm_app_service_plan" "appserviceplan" {
  name                = "${var.project}-appserviceplan-${local.environment}"
  resource_group_name = azurerm_resource_group.rg.name
  location            = azurerm_resource_group.rg.location
  kind                = "Linux"
  reserved            = true # Mandatory for Linux plans

  sku {
    tier = local.planTier
    size = local.planSize
  }
}

resource "azurerm_app_service" "cms" {
  name                = "${var.project}-app-cms-${local.environment}"
  location            = azurerm_app_service_plan.appserviceplan.location
  resource_group_name = azurerm_app_service_plan.appserviceplan.resource_group_name
  app_service_plan_id = azurerm_app_service_plan.appserviceplan.id

  site_config {
    http2_enabled      = "true"
    websockets_enabled = "true"
    linux_fx_version   = "DOCKER|${data.azurerm_container_registry.acr.name}.azurecr.io/${var.project}-cms-${local.environment}:latest"
  }

  app_settings = {
    ASPNETCORE_ENVIRONMENT              = local.aspEnv
    WEBSITES_ENABLE_APP_SERVICE_STORAGE = "false"
    DOCKER_REGISTRY_SERVER_URL          = "https://${data.azurerm_container_registry.acr.name}.azurecr.io"
    DOCKER_REGISTRY_SERVER_USERNAME     = data.azurerm_container_registry.acr.admin_username
    DOCKER_REGISTRY_SERVER_PASSWORD     = data.azurerm_container_registry.acr.admin_password
    DOCKER_CUSTOM_IMAGE_NAME            = "${var.project}-cms-${local.environment}"
    DOCKER_ENABLE_CI                    = "true"
    CDN                                 = "https://${var.project}-cdnendpoint-${local.environment}.azureedge.net"
  }

  connection_string {
    name  = "DefaultConnection"
    type  = "SQLServer"
    value = "@Microsoft.KeyVault(SecretUri=https://${local.keyvaultName}.vault.azure.net/secrets/${local.keyvaultDbKey}/)"
  }

  connection_string {
    name  = "BlobStorageConnectionString"
    type  = "Custom"
    value = "@Microsoft.KeyVault(SecretUri=https://${local.keyvaultName}.vault.azure.net/secrets/${local.keyvaultStorKey}/)"
  }

  identity {
    type = "SystemAssigned"
  }
}

resource "azurerm_app_service" "auth" {
  name                = "${var.project}-app-auth-${local.environment}"
  location            = azurerm_app_service_plan.appserviceplan.location
  resource_group_name = azurerm_app_service_plan.appserviceplan.resource_group_name
  app_service_plan_id = azurerm_app_service_plan.appserviceplan.id

  site_config {
    http2_enabled     = "true"
    linux_fx_version  = "DOCKER|${data.azurerm_container_registry.acr.name}.azurecr.io/${var.project}-auth-${local.environment}:latest"
  }

  app_settings = {
    ASPNETCORE_ENVIRONMENT              = local.aspEnv
    WEBSITES_ENABLE_APP_SERVICE_STORAGE = "false"
    DOCKER_REGISTRY_SERVER_URL          = "https://${data.azurerm_container_registry.acr.name}.azurecr.io"
    DOCKER_REGISTRY_SERVER_USERNAME     = data.azurerm_container_registry.acr.admin_username
    DOCKER_REGISTRY_SERVER_PASSWORD     = data.azurerm_container_registry.acr.admin_password
    DOCKER_CUSTOM_IMAGE_NAME            = "${var.project}-auth-${local.environment}:latest"
    DOCKER_ENABLE_CI                    = "true"
    DEFAULT_DATABASE_CONNECTION         = "@Microsoft.KeyVault(SecretUri=https://${local.keyvaultName}.vault.azure.net/secrets/${local.keyvaultDbKey}/)"
  }

  identity {
    type = "SystemAssigned"
  }
}

resource "azurerm_app_service" "api" {
  name                = "${var.project}-app-api-${local.environment}"
  location            = azurerm_app_service_plan.appserviceplan.location
  resource_group_name = azurerm_app_service_plan.appserviceplan.resource_group_name
  app_service_plan_id = azurerm_app_service_plan.appserviceplan.id

  site_config {
    http2_enabled      = "true"
    linux_fx_version   = "DOCKER|${data.azurerm_container_registry.acr.name}.azurecr.io/${var.project}-api-${local.environment}:latest"
  }

  app_settings = {
    ASPNETCORE_ENVIRONMENT              = local.aspEnv
    WEBSITES_ENABLE_APP_SERVICE_STORAGE = "false"
    DOCKER_REGISTRY_SERVER_URL          = "https://${data.azurerm_container_registry.acr.name}.azurecr.io"
    DOCKER_REGISTRY_SERVER_USERNAME     = data.azurerm_container_registry.acr.admin_username
    DOCKER_REGISTRY_SERVER_PASSWORD     = data.azurerm_container_registry.acr.admin_password
    DOCKER_CUSTOM_IMAGE_NAME            = "${var.project}-api-${local.environment}"
    DOCKER_ENABLE_CI                    = "true"
    DEFAULT_DATABASE_CONNECTION         = "@Microsoft.KeyVault(SecretUri=https://${local.keyvaultName}.vault.azure.net/secrets/${local.keyvaultDbKey}/)"
    IDENTITY_SERVER_CLIENT              = "myfooddoc_app"
    IDENTITY_SERVER_SCOPE               = "myfooddoc_api offline_access"
    IDENTITY_SERVER_ADDRESS             = "https://${var.project}auth-${local.environment}.azurewebsites.net"
  }

  identity {
    type = "SystemAssigned"
  }
}

resource "azurerm_key_vault" "keyvault" {
  name                = local.keyvaultName
  location            = azurerm_app_service_plan.appserviceplan.location
  resource_group_name = azurerm_app_service_plan.appserviceplan.resource_group_name
  tenant_id           = data.azurerm_client_config.current.tenant_id
  sku_name            = "standard"

  #cms app policy
  access_policy {
    tenant_id = data.azurerm_client_config.current.tenant_id
    object_id = azurerm_app_service.cms.identity.0.principal_id
    secret_permissions = [
      "get",
    ]
  }

  #api app policy
  access_policy {
    tenant_id = data.azurerm_client_config.current.tenant_id
    object_id = azurerm_app_service.api.identity.0.principal_id
    secret_permissions = [
      "get",
    ]
  }

  #auth app policy
  access_policy {
    tenant_id = data.azurerm_client_config.current.tenant_id
    object_id = azurerm_app_service.auth.identity.0.principal_id
    secret_permissions = [
      "get",
    ]
  }

  #terraform/devops policy
  access_policy {
    tenant_id = data.azurerm_client_config.current.tenant_id
    object_id = data.azurerm_client_config.current.object_id
    application_id = data.azurerm_client_config.current.client_id
    secret_permissions = [
      "set", "get", "delete",
    ]
  }
}

resource "azurerm_key_vault_secret" "dbsecret" {
  key_vault_id = azurerm_key_vault.keyvault.id
  name         = local.keyvaultDbKey
  value        = "Server=tcp:${local.sqlServerName}.database.windows.net,1433;Initial Catalog=${local.sqlDbName};Persist Security Info=False;User ID=${local.dbadmin};Password=${local.dbpassword};MultipleActiveResultSets=True;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;"
}

resource "azurerm_key_vault_secret" "storsecret" {
  key_vault_id = azurerm_key_vault.keyvault.id
  name         = local.keyvaultStorKey
  value        = "DefaultEndpointsProtocol=https;AccountName=${local.storageName};AccountKey=${data.azurerm_storage_account.storage.primary_access_key};EndpointSuffix=core.windows.net"
}