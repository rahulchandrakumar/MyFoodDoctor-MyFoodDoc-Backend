#global_variables.tf
variable "project" {
  type    = string
  default = "mfd"
}

variable "location" {
  type        = string
  default     = "westeurope"
  description = "Specify a location see: az account list-locations -o table"
}

variable "tags" {
  type        = map(string)
  description = "A list of tags associated to all resources"

  default = {
    maintained_by = "terraform"
  }
}

variable "workspace_to_environment_map" {
  type = map(string)
  default = {
    dev     = "dev"
    qa      = "qa"
    staging = "staging"
    prod    = "prod"
  }
}

variable "environment_to_sqlsize_map" {
  type = map(string)
  default = {
    dev     = "Basic"
    qa      = "Basic"
    staging = "Standard"
    prod    = "Standard"
  }
}

variable "environment_to_plantier_map" {
  type = map(string)
  default = {
    dev     = "Basic"
    qa      = "Basic"
    staging = "Basic"
    prod    = "Standard"
  }
}

variable "environment_to_plansize_map" {
  type = map(string)
  default = {
    dev     = "B1"
    qa      = "B1"
    staging = "B2"
    prod    = "S2"
  }
}

variable "environment_to_aspenv_map" {
  type = map(string)
  default = {
    dev     = "Development"
    qa      = "Staging"
    staging = "Production"
    prod    = "Production"
  }
}