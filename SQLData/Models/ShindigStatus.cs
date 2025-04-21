namespace SQLData.Models;

/// <summary>
/// Represents a status of a shindig.
/// </summary>
public partial class ShindigStatus
{

	/// <summary>
	/// The identifier of the shindig status record.
	/// </summary>
	public int Id { get; set; }

	/// <summary>
	/// The name of the shindig status.
	/// </summary>
	public string Name { get; set; } = null!;

	/// <summary>
	/// A description of the shindig status.
	/// </summary>
	public string? Description { get; set; }

	/// <summary>
	/// The sorting order of the shindig status.
	/// </summary>
	public int SortOrder { get; set; }

	/// <summary>
	/// Flag indicating whether the shindig status is enabled.
	/// </summary>
	public bool IsEnabled { get; set; }

	public virtual ICollection<Shindig> Shindigs { get; set; } = [];

}