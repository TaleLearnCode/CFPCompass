# #############################################################################
# Resource Group
# #############################################################################


resource "azurerm_resource_group" "cfp_compass" {
	name     = "rg-CFPCompass-${var.environment}-${local.region_short}"
	location = var.location
}