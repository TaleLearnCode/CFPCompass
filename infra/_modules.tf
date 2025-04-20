# #############################################################################
# Modules
# #############################################################################

locals {
	region_short = module.azure_region.region.region_short
}

module "azure_region" {
  source  = "TaleLearnCode/regions/azurerm"
  version = "0.0.1-pre"
	azure_region = var.location
}