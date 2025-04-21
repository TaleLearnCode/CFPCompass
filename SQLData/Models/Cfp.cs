namespace SQLData.Models;

public partial class CFP
{

	public string ShindigPermalink { get; set; } = null!;

	public int CFPTypeId { get; set; }

	public DateOnly StartDate { get; set; }

	public DateOnly EndDate { get; set; }

	public string CFPUrl { get; set; } = null!;

	public bool AreTravelExpensesCovered { get; set; }

	public bool AreAccomodationsProvided { get; set; }

	public bool AreEventFeesCovered { get; set; }

	public string? AdditionalBenefits { get; set; }

	public virtual CFPType CFPType { get; set; } = null!;

	public virtual Shindig Shindig { get; set; } = null!;

}
