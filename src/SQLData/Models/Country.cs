namespace SQLData.Models;

/// <summary>
/// Lookup table representing the countries as defined by the ISO 3166-1 standard.
/// </summary>
public partial class Country
{

	/// <summary>
	/// Identifier of the country using the ISO 3166-1 Alpha-2 code.
	/// </summary>
	public string Code { get; set; } = null!;

	/// <summary>
	/// Name of the country using the ISO 3166-1 Country Name.
	/// </summary>
	public string Name { get; set; } = null!;

	/// <summary>
	/// Identifier of the world region where the country is located.
	/// </summary>
	public string WorldRegionCode { get; set; } = null!;

	/// <summary>
	/// Identifier of the country using the United Nations M49 standard.
	/// </summary>
	public string M49code { get; set; } = null!;

	/// <summary>
	/// Flag indicating whether the country has divisions (states, provinces, etc.)
	/// </summary>
	public bool HasDivisions { get; set; }

	/// <summary>
	/// The primary name of the country&apos;s divisions.
	/// </summary>
	public string? DivisionName { get; set; }

	/// <summary>
	/// Flag indicating whether the country record is enabled.
	/// </summary>
	public bool IsEnabled { get; set; }

	public virtual ICollection<CountryDivision> CountryDivisions { get; set; } = [];

	public virtual ICollection<Shindig> Shindigs { get; set; } = [];

	public virtual WorldRegion WorldRegion { get; set; } = null!;

}