# #############################################################################
# Providers
# #############################################################################

terraform {
  required_providers {
    azurerm = {
      source  = "hashicorp/azurerm"
      version = "~>4.0"
    }
    azapi = {
      source  = "Azure/azapi"
      version = "~>2.3"
    }
  }
  #backend "azurerm" {
  #}
}

provider "azurerm" {
	subscription_id = var.subscription_id
  tenant_id = var.tenant_id
  features {
		resource_group {
			prevent_deletion_if_contains_resources = true
		}
    key_vault {
      purge_soft_delete_on_destroy    = true
      recover_soft_deleted_key_vaults = true
    }
  }
}

provider "azapi" {
}