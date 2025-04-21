namespace SQLData.Models;

/// <summary>
/// Represents a spoken/written language.
/// </summary>
public partial class Language
{

	/// <summary>
	/// Identifier of the language.
	/// </summary>
	public string Code { get; set; } = null!;

	/// <summary>
	/// Name of the language.
	/// </summary>
	public string Name { get; set; } = null!;

	/// <summary>
	/// Native name of the language.
	/// </summary>
	public string NativeName { get; set; } = null!;

	/// <summary>
	/// Flag indicating whether the language is enabled.
	/// </summary>
	public bool IsEnabled { get; set; }

}