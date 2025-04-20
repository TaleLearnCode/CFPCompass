using Azure.Identity;
using ConsoleApp.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.AzureAppConfiguration;
using Octokit;
using Spectre.Console;
using System.Text;

DefaultAzureCredential _azureCredentials = new();
string environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Local";

AnsiConsole.Write(
	new FigletText("CFP Compass Uploader")
		.Centered()
		.Color(Color.Green));

string uploadResponse = string.Empty;

await AnsiConsole.Status()
	.Start("Loading configuration...", async ctx =>
	{
		ctx.Spinner(Spinner.Known.Dots);
		ctx.SpinnerStyle(Style.Parse("green bold"));
		IConfigurationRoot config = GetConfiguration();
		ctx.Status("Generating CFP Listing");
		GenerateCFPListing(config);
		ctx.Status("Uploading CFP Listing");
		uploadResponse = await UploadCFPListingAsync(config);
	});

AnsiConsole.WriteLine();
AnsiConsole.MarkupLine(uploadResponse);

IConfigurationRoot GetConfiguration()
{
	Uri appConfigEndpoint = new("https://appcs-cfpcompass-dev-use2.azconfig.io");
	IConfigurationRoot config = new ConfigurationBuilder()
		.AddAzureAppConfiguration(options =>
		{
			options.Connect(appConfigEndpoint, _azureCredentials)
			.ConfigureKeyVault(kv =>
			{
				kv.SetCredential(_azureCredentials);
			})
			.Select(KeyFilter.Any, environment)
			.Select(KeyFilter.Any, null);
		})
		.Build();
	return config;
}

void GenerateCFPListing(IConfigurationRoot config)
{

	string? connectionString = config["SQL:ConnectionString"];
	if (string.IsNullOrEmpty(connectionString))
	{
		Console.WriteLine("SQL Connection string is not found in Azure App Configuration.");
		return;
	}

	using CFPCompassContext context = CFPCompassContext.CreateDbContext(connectionString);
	List<Cfp> callsForSpeakers = [.. context.Cfps
		.AsNoTracking()
		.Include(c => c.Shindig)
		.ThenInclude(x => x.CountryCodeNavigation)];

	StringBuilder sb = new();
	sb.AppendLine("# Call for Speakers Tracker");
	sb.AppendLine();
	sb.AppendLine("Welcome to my **Call for Speakers Tracker**. This page serves as a personal record for keeping track of various call-for-speaker opportunities at software development conferences. Although the repository is public, this section is primarily for my record-keeping and planning. If you're curious about the process, here’s a quick guide on reading and using the table below.");
	sb.AppendLine();
	sb.AppendLine("| Conference | Country | City | Conference Start | Conference End | CFP | Benefits | CFP Start | Deadline |");
	sb.AppendLine("| ---------- | ------- | ---- | ---------------- | -------------- | --- | -------- | --------- | -------- |");
	foreach (Cfp cfp in callsForSpeakers)
		sb.AppendLine($"| [{cfp.Shindig.ShindigName}]({cfp.Shindig.ShindigUrl}) | {(cfp.Shindig.CountryCodeNavigation.CountryName == "United States of America" ? "United States" : cfp.Shindig.CountryCodeNavigation.CountryName)} | {(cfp.Shindig.CountryCodeNavigation.CountryName == "United States of America" ? $"{cfp.Shindig.City}, {cfp.Shindig.CountryDivisionCode}" : cfp.Shindig.City)} | {cfp.Shindig.StartDate:yyyy-MM-dd} | {cfp.Shindig.EndDate:yyyy-MM-dd} | [CFP]({cfp.Cfpurl}) | {(cfp.TravelExpensesCovered ? "✈️" : "")} {(cfp.AccomodationsProvided ? "🏨" : "")} {(cfp.EventFeesCovered ? "🎟️" : "")} {(string.IsNullOrEmpty(cfp.AdditionalBenefits) ? "" : cfp.AdditionalBenefits)} | {cfp.StartDate:yyyy-MM-dd} | {cfp.EndDate:yyyy-MM-dd} |");

	// Write the markdown to a file
	string filePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "cfp_tracker.md");
	File.WriteAllText(filePath, sb.ToString());

}

async Task<string> UploadCFPListingAsync(IConfigurationRoot config)
{
	const string owner = "TaleLearnCode";
	const string repoName = "CFPCompass";
	const string filePathInRepo = "cfp_tracker.md";

	string? token = config["GitHub:Token"];
	if (string.IsNullOrEmpty(token))
		return "[bold white on red]GitHub personal access token is not found in Azure App Configuration.[/]";

	try
	{

		// Read the local markdown file
		string localFilePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "cfp_tracker.md");
		string markdownContent = await File.ReadAllTextAsync(localFilePath);

		GitHubClient github = new(new ProductHeaderValue("CFPCompassUploader"))
		{
			Credentials = new Credentials(token)
		};

		try
		{
			IReadOnlyList<RepositoryContent> existingFileContents = await github.Repository.Content.GetAllContents(owner, repoName, filePathInRepo);
			RepositoryContent existingFile = existingFileContents[0];

			// If the file exists, update it with new content.
			UpdateFileRequest updateRequest = new("Update CFP tracker markdown", markdownContent, existingFile.Sha);

			RepositoryContentChangeSet updateResponse = await github.Repository.Content.UpdateFile(owner, repoName, filePathInRepo, updateRequest);
			return $"[bold white on green]File updated successfully[/] [link={updateResponse.Content.HtmlUrl}]CFP Listing[/]";

		}
		catch (Octokit.NotFoundException)
		{
			// If the file was not found, create it
			CreateFileRequest createRequest = new("Add CFP tracker markdown", markdownContent);

			RepositoryContentChangeSet createResponse = await github.Repository.Content.CreateFile(owner, repoName, filePathInRepo, createRequest);
			return $"[bold white on green]File created successfully[/] [link={createResponse.Content.HtmlUrl}]CFP Listing[/]";
		}

	}
	catch (Exception ex)
	{
		//Console.WriteLine("An error occurred: " + ex.Message);
		return $"[bold white on red]An error occurred: {ex.Message}[/]";
	}
}