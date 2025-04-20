# #############################################################################
# App Configiratiion
# #############################################################################

resource "azurerm_app_configuration" "cfp_compass" {
	name                     = "appcs-CFPCompass-${var.environment}-${local.region_short}"
	location                 = var.location
	resource_group_name      = azurerm_resource_group.cfp_compass.name
	local_auth_enabled       = true
	public_network_access    = "Enabled"
	purge_protection_enabled = false
	sku                      = "standard"
}

resource "azurerm_role_assignment" "appcs_chadgreen" {
	scope                = azurerm_app_configuration.cfp_compass.id
	role_definition_name = "App Configuration Data Owner"
	principal_id         = var.admin_object_id
}