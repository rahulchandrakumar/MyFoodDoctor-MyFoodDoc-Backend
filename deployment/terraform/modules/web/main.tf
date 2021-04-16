#######################################################
# Variables & Locals
#######################################################
variable "location" {
  default = "West Europe"
}
variable "projectname" {}
variable "stage" {}

variable "sqldb_tier" {}
variable "sqldb_service_objective" {}

variable "storageaccount_replication_type" {}
variable "storageaccount_tier" {}

variable "apiserver_plantier" {}
variable "apiserver_plansize" {}
variable "apiserver_plancapacity" {}

variable "apiapp_alwayson" { type = bool }
variable "apiapp_aspenv" {}

variable "keyvault_id" {}
variable "keyvault_name" {}

variable "containerregistry_url" {}
variable "containerregistry_admin_username" {}
variable "containerregistry_admin_password" {}
variable "app_store_verify_receipt_url" {}

data "azurerm_client_config" "current" {}

#######################################################
# Tags
#######################################################
locals {
  # see example of how to add additional tags per resource later: https://www.terraform.io/docs/configuration-0-11/locals.html#examples
  resource_tags = {
    "Project"     = "${var.projectname}"
    "Environment" = "${var.stage}"
    "Scope"       = "web"
  }
}

#######################################################
# Resources/Providers
#######################################################
provider "azurerm" {
  # Whilst version is optional, we /strongly recommend/ using it to pin the version of the Provider being used
  version         = "=1.40.0"
  # we need this, as in customer subscription only certain resource providers were registered...
  # without it, any terraform plan/apply would fail with a 403 on trying to access e.g. Media Services
  # see https://github.com/hashicorp/terraform/issues/18180#issuecomment-394369502
  skip_provider_registration = false
  subscription_id = "d3359015-e21d-4560-8542-0eb0655d9f8f"
}

provider "random" {
  version = ">=2.2"
}

#######################################################
# Random
#######################################################
resource "random_string" "random" {
  length = 16
  special = false
}

resource "random_password" "password" {
  length = 16
  special = true
  override_special = "_%@"
}

#######################################################
# Resource Group
#######################################################
resource "azurerm_resource_group" "rg" {
  name     = "${var.projectname}-${var.stage}"
  location = var.location
  tags     = local.resource_tags
}

#######################################################
# Database
#######################################################
locals {
  dbadmin = random_string.random.result
  dbpassword = random_password.password.result
  sqlServerName = "${var.projectname}sqlserver${var.stage}"
  sqlDbName = "${var.projectname}sqldb${var.stage}"
  keyvaultDbKey = "SqlConnection-${var.stage}"
}

# create Azure SQL Server
resource "azurerm_sql_server" "sqlserver" {
  name                         = local.sqlServerName
  resource_group_name          = azurerm_resource_group.rg.name
  location                     = azurerm_resource_group.rg.location
  version                      = "12.0"
  administrator_login          = local.dbadmin
  administrator_login_password = local.dbpassword
  tags                         = local.resource_tags

  lifecycle {
    #see https://www.terraform.io/docs/configuration/resources.html#ignore_changes
    ignore_changes = [
      administrator_login,
      administrator_login_password
    ]
  }
}

# create Azure SQL DB
resource "azurerm_sql_database" "sqldb" {
  name                              = local.sqlDbName
  resource_group_name               = azurerm_resource_group.rg.name
  location                          = azurerm_resource_group.rg.location
  server_name                       = azurerm_sql_server.sqlserver.name
  edition                           = var.sqldb_tier
  requested_service_objective_name  = var.sqldb_service_objective
  tags                              = local.resource_tags
}

# save database connection string into the KeyVault
resource "azurerm_key_vault_secret" "dbsecret" {
  key_vault_id = var.keyvault_id
  name         = local.keyvaultDbKey
  value        = "Server=tcp:${local.sqlServerName}.database.windows.net,1433;Initial Catalog=${local.sqlDbName};Persist Security Info=False;User ID=${local.dbadmin};Password=${local.dbpassword};MultipleActiveResultSets=True;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;"
}

#######################################################
# Storage Account
#######################################################
locals {
  storageName = "${var.projectname}storageacc${var.stage}"
  keyvaultStorKey = "StorageConnection-${var.stage}"
}

resource "azurerm_storage_account" "storage" {
  name                     = local.storageName
  resource_group_name      = azurerm_resource_group.rg.name
  location                 = azurerm_resource_group.rg.location
  account_tier             = var.storageaccount_tier
  account_replication_type = var.storageaccount_replication_type
  account_kind             = "BlobStorage"
  tags                     = local.resource_tags
}

data "azurerm_storage_account" "storage" {
  name                     = azurerm_storage_account.storage.name
  resource_group_name      = azurerm_resource_group.rg.name
}

resource "azurerm_cdn_profile" "cdnprofile" {
  name                = "${var.projectname}-cdnprofile-${var.stage}"
  resource_group_name = azurerm_resource_group.rg.name
  location            = azurerm_resource_group.rg.location
  sku                 = "Standard_Microsoft"
  tags                = local.resource_tags
}

resource "azurerm_cdn_endpoint" "cdnendpoint" {
  name                = "${var.projectname}-cdnendpoint-${var.stage}"
  profile_name        = azurerm_cdn_profile.cdnprofile.name
  resource_group_name = azurerm_resource_group.rg.name
  location            = azurerm_resource_group.rg.location
  tags                = local.resource_tags
  origin_host_header  = "${local.storageName}.blob.core.windows.net"

  origin {
    name      = "StorageCDN"
    host_name = "${local.storageName}.blob.core.windows.net"
  }
}

resource "azurerm_key_vault_secret" "storsecret" {
  key_vault_id = var.keyvault_id
  name         = local.keyvaultStorKey
  value        = "DefaultEndpointsProtocol=https;AccountName=${local.storageName};AccountKey=${data.azurerm_storage_account.storage.primary_access_key};EndpointSuffix=core.windows.net"
}

########################################################
# Application Insights
########################################################
resource "azurerm_application_insights" "appinsights" {
  name                = "${var.projectname}-appinsights-${var.stage}"
  location            = azurerm_resource_group.rg.location
  resource_group_name = azurerm_resource_group.rg.name
  application_type    = "web"
  tags                = local.resource_tags
}

########################################################
# Api WebApp
########################################################
locals {
  authAppName = "${var.projectname}-app-auth-${var.stage}"
}

# create app service plan (linux!)
resource "azurerm_app_service_plan" "appserviceplan" {
  name                = "${var.projectname}-plan-${var.stage}"
  resource_group_name = azurerm_resource_group.rg.name
  location            = azurerm_resource_group.rg.location
  kind                = "Linux"
  # important: if you are going to use docker, the property "reserved" is mendatory to be set to true!
  reserved = true
  tags     = local.resource_tags

  sku {
    tier     = var.apiserver_plantier
    size     = var.apiserver_plansize
    capacity = var.apiserver_plancapacity
  }
}

resource "azurerm_app_service" "cms" {
  name                = "${var.projectname}-app-cms-${var.stage}"
  location            = azurerm_app_service_plan.appserviceplan.location
  resource_group_name = azurerm_app_service_plan.appserviceplan.resource_group_name
  app_service_plan_id = azurerm_app_service_plan.appserviceplan.id
  tags                = local.resource_tags

  https_only          = true
  site_config {
    http2_enabled      = "true"
    websockets_enabled = "true"
    linux_fx_version   = "DOCKER|${var.containerregistry_url}/${var.projectname}-cms:latest"
    always_on          = var.apiapp_alwayson
  }

  app_settings = {
    ASPNETCORE_ENVIRONMENT                      = var.apiapp_aspenv
    APPINSIGHTS_INSTRUMENTATIONKEY              = azurerm_application_insights.appinsights.instrumentation_key
    WEBSITES_ENABLE_APP_SERVICE_STORAGE         = "false"
    BLOB_STORAGE_CONNECTION                     = "@Microsoft.KeyVault(SecretUri=https://${var.keyvault_name}.vault.azure.net/secrets/${local.keyvaultStorKey}/)"
    CDN                                         = "https://${var.projectname}-cdnendpoint-${var.stage}.azureedge.net"
    DEFAULT_DATABASE_CONNECTION                 = "@Microsoft.KeyVault(SecretUri=https://${var.keyvault_name}.vault.azure.net/secrets/${local.keyvaultDbKey}/)"
    DOCKER_REGISTRY_SERVER_URL                  = var.containerregistry_url
    DOCKER_REGISTRY_SERVER_USERNAME             = var.containerregistry_admin_username
    DOCKER_REGISTRY_SERVER_PASSWORD             = var.containerregistry_admin_password
    DOCKER_CUSTOM_IMAGE_NAME                    = "${var.projectname}-cms"
    DOCKER_ENABLE_CI                            = "false"
    FAT_SECRET_IDENTITY_SERVER_SCOPE            = "basic"
    FAT_SECRET_IDENTITY_SERVER_GRANT_TYPE       = "client_credentials"
    FAT_SECRET_IDENTITY_SERVER_CLIENT_SECRET    = ""
    FAT_SECRET_IDENTITY_SERVER_CLIENT_ID        = ""
    FAT_SECRET_IDENTITY_SERVER_ADDRESS          = "https://oauth.fatsecret.com"
    FAT_SECRET_ADDRESS                          = "https://platform.fatsecret.com/rest/server.api"
    FAT_SECRET_CONSUMER_KEY                     = ""
    FAT_SECRET_CONSUMER_SECRET                  = ""
    TZ                                          = "Europe/Berlin"
  }

  identity {
    type = "SystemAssigned"
  }

  lifecycle {
    #see https://www.terraform.io/docs/configuration/resources.html#ignore_changes
    ignore_changes = [
      site_config,
      app_settings["DOCKER_CUSTOM_IMAGE_NAME"],
      app_settings["FAT_SECRET_IDENTITY_SERVER_CLIENT_ID"],
      app_settings["FAT_SECRET_IDENTITY_SERVER_CLIENT_SECRET"],
      app_settings["FAT_SECRET_CONSUMER_KEY"],
      app_settings["FAT_SECRET_CONSUMER_SECRET"]
    ]
  }
}

resource "azurerm_app_service" "auth" {
  name                = local.authAppName
  location            = azurerm_app_service_plan.appserviceplan.location
  resource_group_name = azurerm_app_service_plan.appserviceplan.resource_group_name
  app_service_plan_id = azurerm_app_service_plan.appserviceplan.id
  tags                = local.resource_tags

  https_only          = true
  site_config {
    http2_enabled     = "true"
    linux_fx_version  = "DOCKER|${var.containerregistry_url}/${var.projectname}-auth:latest"
    always_on         = var.apiapp_alwayson
  }

  app_settings = {
    ASPNETCORE_ENVIRONMENT              = var.apiapp_aspenv
    APPINSIGHTS_INSTRUMENTATIONKEY      = azurerm_application_insights.appinsights.instrumentation_key
    WEBSITES_ENABLE_APP_SERVICE_STORAGE = "false"
    DOCKER_REGISTRY_SERVER_URL          = var.containerregistry_url
    DOCKER_REGISTRY_SERVER_USERNAME     = var.containerregistry_admin_username
    DOCKER_REGISTRY_SERVER_PASSWORD     = var.containerregistry_admin_password
    DOCKER_CUSTOM_IMAGE_NAME            = "${var.projectname}-auth"
    DOCKER_ENABLE_CI                    = "false"
    DEFAULT_DATABASE_CONNECTION         = "@Microsoft.KeyVault(SecretUri=https://${var.keyvault_name}.vault.azure.net/secrets/${local.keyvaultDbKey}/)"
    IDENTITY_SERVER_ISSUER_URI          = "https://${local.authAppName}.azurewebsites.net"
  }

  identity {
    type = "SystemAssigned"
  }

  lifecycle {
    #see https://www.terraform.io/docs/configuration/resources.html#ignore_changes
    ignore_changes = [
      site_config,
      app_settings["DOCKER_CUSTOM_IMAGE_NAME"]
    ]
  }
}

resource "azurerm_app_service" "api" {
  name                = "${var.projectname}-app-api-${var.stage}"
  location            = azurerm_app_service_plan.appserviceplan.location
  resource_group_name = azurerm_app_service_plan.appserviceplan.resource_group_name
  app_service_plan_id = azurerm_app_service_plan.appserviceplan.id
  tags                = local.resource_tags

  https_only          = true
  site_config {
    http2_enabled      = "true"
    linux_fx_version   = "DOCKER|${var.containerregistry_url}/${var.projectname}-api:latest"
    always_on          = var.apiapp_alwayson
  }

  app_settings = {
    ASPNETCORE_ENVIRONMENT                                      = var.apiapp_aspenv
    APPINSIGHTS_INSTRUMENTATIONKEY                              = azurerm_application_insights.appinsights.instrumentation_key
    WEBSITES_ENABLE_APP_SERVICE_STORAGE                         = "false"
    DOCKER_REGISTRY_SERVER_URL                                  = var.containerregistry_url
    DOCKER_REGISTRY_SERVER_USERNAME                             = var.containerregistry_admin_username
    DOCKER_REGISTRY_SERVER_PASSWORD                             = var.containerregistry_admin_password
    DOCKER_CUSTOM_IMAGE_NAME                                    = "${var.projectname}-api"
    DOCKER_ENABLE_CI                                            = "false"
    DEFAULT_DATABASE_CONNECTION                                 = "@Microsoft.KeyVault(SecretUri=https://${var.keyvault_name}.vault.azure.net/secrets/${local.keyvaultDbKey}/)"
    IDENTITY_SERVER_CLIENT                                      = "myfooddoc_app"
    IDENTITY_SERVER_SCOPE                                       = "myfooddoc_api offline_access"
    IDENTITY_SERVER_ADDRESS                                     = "https://${local.authAppName}.azurewebsites.net"
    EMAIL_SERVICE_FROM_ADDRESS                                  = "app@myfooddoctor.de"
    EMAIL_SERVICE_FROM_NAME                                     = "My food doctor"
    EMAIL_SERVICE_SEND_GRID_API_KEY                             = ""
    FAT_SECRET_IDENTITY_SERVER_SCOPE                            = "basic"
    FAT_SECRET_IDENTITY_SERVER_GRANT_TYPE                       = "client_credentials"
    FAT_SECRET_IDENTITY_SERVER_CLIENT_SECRET                    = ""
    FAT_SECRET_IDENTITY_SERVER_CLIENT_ID                        = ""
    FAT_SECRET_IDENTITY_SERVER_ADDRESS                          = "https://oauth.fatsecret.com"
    FAT_SECRET_ADDRESS                                          = "https://platform.fatsecret.com/rest/server.api"
    FAT_SECRET_CONSUMER_KEY                                     = ""
    FAT_SECRET_CONSUMER_SECRET                                  = ""
    APP_STORE_VERIFY_RECEIPT_URL                                = var.app_store_verify_receipt_url
    APP_STORE_VERIFY_RECEIPT_SHARED_SECRET                      = ""
    GOOGLE_PLAY_STORE_PACKAGE_NAME                              = "de.myfooddoctor.app"
    GOOGLE_PLAY_STORE_CERTIFICATE_TYPE                          = "service_account"
    GOOGLE_PLAY_STORE_CERTIFICATE_PROJECT_ID                    = "pc-api-6022083488696963110-239"
    GOOGLE_PLAY_STORE_CERTIFICATE_PRIVATE_KEY_ID                = ""
    GOOGLE_PLAY_STORE_CERTIFICATE_PRIVATE_KEY                   = ""
    GOOGLE_PLAY_STORE_CERTIFICATE_CLIENT_EMAIL                  = "server-purchase@pc-api-6022083488696963110-239.iam.gserviceaccount.com"
    GOOGLE_PLAY_STORE_CERTIFICATE_CLIENT_ID                     = ""
    GOOGLE_PLAY_STORE_CERTIFICATE_AUTH_URI                      = "https://accounts.google.com/o/oauth2/auth"
    GOOGLE_PLAY_STORE_CERTIFICATE_TOKEN_URI                     = "https://oauth2.googleapis.com/token"
    GOOGLE_PLAY_STORE_CERTIFICATE_AUTH_PROVIDER_X509_CERT_URL   = "https://www.googleapis.com/oauth2/v1/certs"
    GOOGLE_PLAY_STORE_CERTIFICATE_CLIENT_X509_CERT_URL          = "https://www.googleapis.com/robot/v1/metadata/x509/server-purchase%40pc-api-6022083488696963110-239.iam.gserviceaccount.com"
    STATISTICS_PERIOD				                            = 4
    STATISTICS_MINIMUM_DAYS                                     = 2
    TZ                                                          = "Europe/Berlin"
  }

  identity {
    type = "SystemAssigned"
  }

  lifecycle {
    #see https://www.terraform.io/docs/configuration/resources.html#ignore_changes
    ignore_changes = [
      site_config,
      app_settings["DOCKER_CUSTOM_IMAGE_NAME"],
      app_settings["APP_STORE_VERIFY_RECEIPT_SHARED_SECRET"],
      app_settings["EMAIL_SERVICE_SEND_GRID_API_KEY"],
      app_settings["FAT_SECRET_IDENTITY_SERVER_CLIENT_ID"],
      app_settings["FAT_SECRET_IDENTITY_SERVER_CLIENT_SECRET"],
      app_settings["FAT_SECRET_CONSUMER_KEY"],
      app_settings["FAT_SECRET_CONSUMER_SECRET"],
      
    ]
  }
}

resource "azurerm_function_app" "func" {
  name                      = "${var.projectname}-app-func-${var.stage}"
  location                  = azurerm_app_service_plan.appserviceplan.location
  resource_group_name       = azurerm_app_service_plan.appserviceplan.resource_group_name
  app_service_plan_id       = azurerm_app_service_plan.appserviceplan.id
  storage_connection_string = "@Microsoft.KeyVault(SecretUri=https://${var.keyvault_name}.vault.azure.net/secrets/${local.keyvaultStorKey}/)"
  tags                      = local.resource_tags
  version                   = "~3"
  enable_builtin_logging    = true

  site_config {
    linux_fx_version        = "DOCKER|${var.containerregistry_url}/${var.projectname}-func:latest"
    always_on               = var.apiapp_alwayson
    http2_enabled           = true
  }

  app_settings = {
    ASPNETCORE_ENVIRONMENT                                      = var.apiapp_aspenv
    APPINSIGHTS_INSTRUMENTATIONKEY                              = azurerm_application_insights.appinsights.instrumentation_key
    AzureWebJobsStorage                                         = "@Microsoft.KeyVault(SecretUri=https://${var.keyvault_name}.vault.azure.net/secrets/${local.keyvaultStorKey}/)"
    DOCKER_REGISTRY_SERVER_URL                                  = var.containerregistry_url
    DOCKER_REGISTRY_SERVER_USERNAME                             = var.containerregistry_admin_username
    DOCKER_REGISTRY_SERVER_PASSWORD                             = var.containerregistry_admin_password
    DOCKER_CUSTOM_IMAGE_NAME                                    = "${var.projectname}-func"
    DOCKER_ENABLE_CI                                            = "false"
    BLOB_STORAGE_CONNECTION                                     = "@Microsoft.KeyVault(SecretUri=https://${var.keyvault_name}.vault.azure.net/secrets/${local.keyvaultStorKey}/)"
    DEFAULT_DATABASE_CONNECTION                                 = "@Microsoft.KeyVault(SecretUri=https://${var.keyvault_name}.vault.azure.net/secrets/${local.keyvaultDbKey}/)"
    EMAIL_SERVICE_FROM_ADDRESS                                  = "app@myfooddoctor.de"
    EMAIL_SERVICE_FROM_NAME                                     = "My food doctor"
    EMAIL_SERVICE_SEND_GRID_API_KEY                             = ""
    FAT_SECRET_IDENTITY_SERVER_SCOPE                            = "basic"
    FAT_SECRET_IDENTITY_SERVER_GRANT_TYPE                       = "client_credentials"
    FAT_SECRET_IDENTITY_SERVER_CLIENT_SECRET                    = ""
    FAT_SECRET_IDENTITY_SERVER_CLIENT_ID                        = ""
    FAT_SECRET_IDENTITY_SERVER_ADDRESS                          = "https://oauth.fatsecret.com"
    FAT_SECRET_ADDRESS                                          = "https://platform.fatsecret.com/rest/server.api"
    FAT_SECRET_CONSUMER_KEY                                     = ""
    FAT_SECRET_CONSUMER_SECRET                                  = ""
    FIREBASE_TYPE                                               = "service_account"
    FIREBASE_PROJECT_ID                                         = "medicum-myfooddoc"
    FIREBASE_PRIVATE_KEY_ID                                     = ""
    FIREBASE_PRIVATE_KEY                                        = ""
    FIREBASE_CLIENT_EMAIL                                       = "firebase-adminsdk-1mq1y@medicum-myfooddoc.iam.gserviceaccount.com"
    FIREBASE_CLIENT_ID                                          = ""
    FIREBASE_AUTH_URI                                           = "https://accounts.google.com/o/oauth2/auth"
    FIREBASE_TOKEN_URI                                          = "https://oauth2.googleapis.com/token"
    FIREBASE_AUTH_PROVIDER_X509_CERT_URL                        = "https://www.googleapis.com/oauth2/v1/certs"
    FIREBASE_CLIENT_X509_CERT_URL                               = "https://www.googleapis.com/robot/v1/metadata/x509/firebase-adminsdk-1mq1y%40medicum-myfooddoc.iam.gserviceaccount.com"
    APP_STORE_VERIFY_RECEIPT_URL                                = var.app_store_verify_receipt_url
    APP_STORE_VERIFY_RECEIPT_SHARED_SECRET                      = ""
    GOOGLE_PLAY_STORE_PACKAGE_NAME                              = "de.myfooddoctor.app"
    GOOGLE_PLAY_STORE_CERTIFICATE_TYPE                          = "service_account"
    GOOGLE_PLAY_STORE_CERTIFICATE_PROJECT_ID                    = "pc-api-6022083488696963110-239"
    GOOGLE_PLAY_STORE_CERTIFICATE_PRIVATE_KEY_ID                = ""
    GOOGLE_PLAY_STORE_CERTIFICATE_PRIVATE_KEY                   = ""
    GOOGLE_PLAY_STORE_CERTIFICATE_CLIENT_EMAIL                  = "server-purchase@pc-api-6022083488696963110-239.iam.gserviceaccount.com"
    GOOGLE_PLAY_STORE_CERTIFICATE_CLIENT_ID                     = ""
    GOOGLE_PLAY_STORE_CERTIFICATE_AUTH_URI                      = "https://accounts.google.com/o/oauth2/auth"
    GOOGLE_PLAY_STORE_CERTIFICATE_TOKEN_URI                     = "https://oauth2.googleapis.com/token"
    GOOGLE_PLAY_STORE_CERTIFICATE_AUTH_PROVIDER_X509_CERT_URL   = "https://www.googleapis.com/oauth2/v1/certs"
    GOOGLE_PLAY_STORE_CERTIFICATE_CLIENT_X509_CERT_URL          = "https://www.googleapis.com/robot/v1/metadata/x509/server-purchase%40pc-api-6022083488696963110-239.iam.gserviceaccount.com"
    FUNCTIONS_WORKER_RUNTIME                                    = "dotnet"
    WEBSITES_ENABLE_APP_SERVICE_STORAGE                         = "false"
    WEBSITE_HTTPLOGGING_RETENTION_DAYS                          = 14
    WEBSITE_RUN_FROM_PACKAGE                                    = ""
    STATISTICS_PERIOD				                            = 4
    STATISTICS_MINIMUM_DAYS                                     = 2
    TZ                                                          = "Europe/Berlin"
  }

  identity {
    type = "SystemAssigned"
  }

  lifecycle {
    #see https://www.terraform.io/docs/configuration/resources.html#ignore_changes
    ignore_changes = [
      site_config,
      app_settings["DOCKER_CUSTOM_IMAGE_NAME"],
      app_settings["AzureWebJobsStorage"],
      app_settings["APP_STORE_VERIFY_RECEIPT_SHARED_SECRET"],
      app_settings["FAT_SECRET_IDENTITY_SERVER_CLIENT_ID"],
      app_settings["FAT_SECRET_IDENTITY_SERVER_CLIENT_SECRET"],
      app_settings["FAT_SECRET_CONSUMER_KEY"],
      app_settings["FAT_SECRET_CONSUMER_SECRET"],
      app_settings["FIREBASE_CLIENT_ID"],
      app_settings["FIREBASE_PRIVATE_KEY_ID"],
      app_settings["FIREBASE_PRIVATE_KEY"],
      app_settings["GOOGLE_PLAY_STORE_CERTIFICATE_PRIVATE_KEY_ID"],
      app_settings["GOOGLE_PLAY_STORE_CERTIFICATE_PRIVATE_KEY"],
      app_settings["GOOGLE_PLAY_STORE_CERTIFICATE_CLIENT_ID"]
    ]
  }
}

# cms app KeyVault policy
resource "azurerm_key_vault_access_policy" "cms" {
  key_vault_id = var.keyvault_id

  tenant_id = data.azurerm_client_config.current.tenant_id
  object_id = azurerm_app_service.cms.identity.0.principal_id
  secret_permissions = [
    "get",
  ]
}

# auth app KeyVault policy
resource "azurerm_key_vault_access_policy" "auth" {
  key_vault_id = var.keyvault_id

  tenant_id = data.azurerm_client_config.current.tenant_id
  object_id = azurerm_app_service.auth.identity.0.principal_id
  secret_permissions = [
    "get",
  ]
}

# api app KeyVault policy
resource "azurerm_key_vault_access_policy" "api" {
  key_vault_id = var.keyvault_id

  tenant_id = data.azurerm_client_config.current.tenant_id
  object_id = azurerm_app_service.api.identity.0.principal_id
  secret_permissions = [
    "get",
  ]
}

# func app KeyVault policy
resource "azurerm_key_vault_access_policy" "func" {
  key_vault_id = var.keyvault_id

  tenant_id = data.azurerm_client_config.current.tenant_id
  object_id = azurerm_function_app.func.identity.0.principal_id
  secret_permissions = [
    "get",
  ]
}