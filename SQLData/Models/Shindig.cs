namespace SQLData.Models;

public partial class Shindig
{

	public string Permalink { get; set; } = null!;

	public int ShindigTypeId { get; set; }

	public int ShindigStatusId { get; set; }

	public string Name { get; set; } = null!;

	public string CountryCode { get; set; } = null!;

	public string? CountryDivisionCode { get; set; }

	public string City { get; set; } = null!;

	public string Venue { get; set; } = null!;

	public DateOnly StartDate { get; set; }

	public DateOnly EndDate { get; set; }

	public string TimeZoneId { get; set; } = null!;

	public string? Description { get; set; }

	public string? Url { get; set; }

	public string? ImageUrl { get; set; }

	public bool IsEnabled { get; set; }

	public virtual CFP? CFP { get; set; }

	public virtual Country Country { get; set; } = null!;

	public virtual CountryDivision? CountryDivision { get; set; }

	public virtual ShindigStatus ShindigStatus { get; set; } = null!;

	public virtual ShindigType ShindigType { get; set; } = null!;

	public virtual TimeZone TimeZone { get; set; } = null!;

}