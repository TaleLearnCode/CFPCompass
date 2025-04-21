namespace SQLData.Models;

/// <summary>
/// Represents the list of time zones as defined by the IANA.
/// </summary>
public partial class TimeZone
{

	/// <summary>
	/// The identifier of the time zone as defined by the IANA.
	/// </summary>
	public string Id { get; set; } = null!;

	/// <summary>
	/// The standard abbreviation for the time zone.
	/// </summary>
	public string StandardAbbreviation { get; set; } = null!;

	/// <summary>
	/// The standard offset for the time zone.
	/// </summary>
	public string StandardOffset { get; set; } = null!;

	/// <summary>
	/// The daylight savings abbreviation for the time zone.
	/// </summary>
	public string? DaylightSavingsAbbreviation { get; set; }

	/// <summary>
	/// The daylight offset for the time zone.
	/// </summary>
	public string? DaylightOffset { get; set; }

	public virtual ICollection<Shindig> Shindigs { get; set; } = [];

}