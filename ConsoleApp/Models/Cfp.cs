namespace ConsoleApp.Models;

public partial class Cfp
{
	public string ShindigPermalink { get; set; } = null!;

	public int CfptypeId { get; set; }

	public DateOnly StartDate { get; set; }

	public DateOnly EndDate { get; set; }

	public string Cfpurl { get; set; } = null!;

	public bool TravelExpensesCovered { get; set; }

	public bool AccomodationsProvided { get; set; }

	public bool EventFeesCovered { get; set; }

	public string? AdditionalBenefits { get; set; }

	public virtual Cfptype Cfptype { get; set; } = null!;

	public virtual Shindig Shindig { get; set; } = null!;
}
