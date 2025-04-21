namespace ConsoleApp.Models;

public partial class CFPType
{

	public int Id { get; set; }

	public string Name { get; set; } = null!;

	public string? Description { get; set; }

	public int SortOrder { get; set; }

	public bool IsEnabled { get; set; }

	public virtual ICollection<CFP> CFPs { get; set; } = [];

}