namespace SQLData.Models;

/// <summary>
/// Lookup table representing the world regions as defined by the UN M49 specification.
/// </summary>
public partial class WorldRegion
{

	/// <summary>
	/// Identifier of the world region.
	/// </summary>
	public string Code { get; set; } = null!;

	/// <summary>
	/// Name of the world region.
	/// </summary>
	public string Name { get; set; } = null!;

	/// <summary>
	/// Identifier of the world region parent (for subregions).
	/// </summary>
	public string? ParentId { get; set; }

	/// <summary>
	/// Flag indicating whether the world region is enabled.
	/// </summary>
	public bool IsEnabled { get; set; }

	public virtual ICollection<Country> Countries { get; set; } = [];

	public virtual ICollection<WorldRegion> Children { get; set; } = [];

	public virtual WorldRegion? Parent { get; set; }

}