# #############################################################################
# Azure SQL Server
# #############################################################################

# -----------------------------------------------------------------------------
# Variables
# -----------------------------------------------------------------------------

variable "sql_admin_username" {
	type        = string
	description = "SQL Server administrator username."
}

variable "sql_admin_password" {
	type        = string
	description = "SQL Server administrator password."
	sensitive   = true
}

# -----------------------------------------------------------------------------
# Resources
# -----------------------------------------------------------------------------

resource "azurerm_mssql_server" "cfp_compass" {
	name                         = lower("sql-CFPCompass-${var.environment}-${local.region_short}")
	resource_group_name          = azurerm_resource_group.cfp_compass.name
	location                     = azurerm_resource_group.cfp_compass.location
	version                      = "12.0"
	administrator_login          = var.sql_admin_username
	administrator_login_password = var.sql_admin_password
	minimum_tls_version          = "1.2"
	public_network_access_enabled = true
	azuread_administrator {
		login_username = var.admin_login_username
		object_id      = var.admin_object_id
	}
}

resource "azurerm_mssql_database" "cfp_compass" {
  name      = "sqldb-CFPCompass-${var.environment}-${local.region_short}"
  server_id = azurerm_mssql_server.cfp_compass.id
	collation = "SQL_Latin1_General_CP1_CI_AS"
	sku_name  = "GP_S_Gen5_1"
	max_size_gb = 2
	enclave_type = "VBS"
}