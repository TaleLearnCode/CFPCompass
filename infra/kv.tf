# #############################################################################
# Key Vault
# #############################################################################

resource "azurerm_key_vault" "cfp_compass" {
	name                          = "kv-CFPCompass-${var.environment}-${local.region_short}"
	location                      = var.location
	resource_group_name           = azurerm_resource_group.cfp_compass.name
	tenant_id                     = var.tenant_id
	sku_name                      = "standard"
	purge_protection_enabled      = true
	soft_delete_retention_days    = 7
	enable_rbac_authorization     = true
	public_network_access_enabled = true
}

resource "azurerm_role_assignment" "kv_chadgreen" {
	scope                = azurerm_key_vault.cfp_compass.id
	role_definition_name = "Key Vault Administrator"
	principal_id         = var.admin_object_id
}