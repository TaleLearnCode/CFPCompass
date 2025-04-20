# #############################################################################
# Project-Level Variables and Locals
# #############################################################################

variable "tenant_id" {
	type        = string
	description = "Identifier of the Azure tenant."
}

variable "subscription_id" {
  type        = string
  description = "Identifier of the Azure subscription."
}

variable "environment" {
  type        = string
  description = "Environment for the resources."
}

variable "location" {
  type        = string
  description = "Location for the resources."
}

variable "admin_login_username" {
	type        = string
	description = "Administrator login username."
}

variable "admin_object_id" {
	type        = string
	description = "Administrator object ID."
}