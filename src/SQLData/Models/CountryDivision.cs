namespace SQLData.Models;

/// <summary>
/// Lookup table representing the world regions as defined by the ISO 3166-2 standard.
/// </summary>
public partial class CountryDivision
{

	/// <summary>
	/// Identifier of the country division using the ISO 3166-2 Alpha-2 code.
	/// </summary>
	public string Code { get; set; } = null!;

	public string CountryCode { get; set; } = null!;

	/// <summary>
	/// Name of the country using the ISO 3166-2 Subdivision Name.
	/// </summary>
	public string Name { get; set; } = null!;

	/// <summary>
	/// The category name of the country division.
	/// </summary>
	public string CategoryName { get; set; } = null!;

	/// <summary>
	/// Flag indicating whether the country division record is enabled.
	/// </summary>
	public bool IsEnabled { get; set; }

	public virtual Country Country { get; set; } = null!;

	public virtual ICollection<Shindig> Shindigs { get; set; } = [];

}