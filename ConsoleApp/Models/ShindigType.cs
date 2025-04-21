namespace ConsoleApp.Models;

/// <summary>
/// Represents a type of a shindig.
/// </summary>
public partial class ShindigType
{

	/// <summary>
	/// The identifier of the shindig type record.
	/// </summary>
	public int Id { get; set; }

	/// <summary>
	/// The name of the shindig type.
	/// </summary>
	public string Name { get; set; } = null!;

	/// <summary>
	/// A description of the shindig type.
	/// </summary>
	public string? Description { get; set; }

	/// <summary>
	/// The sorting order of the shindig type.
	/// </summary>
	public int SortOrder { get; set; }

	/// <summary>
	/// Flag indicating whether the shindig type is enabled.
	/// </summary>
	public bool IsEnabled { get; set; }

	public virtual ICollection<Shindig> Shindigs { get; set; } = [];

}