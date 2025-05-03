// Begin the main program
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

			string? connectionString = config["SQL:ConnectionString"];
			if (string.IsNullOrEmpty(connectionString))
				throw new Exception("SQL Connection string is not found in Azure App Configuration.");

			ctx.Status("Retrieving CFP data");
			using CFPCompassContext context = CFPCompassContext.CreateDbContext(connectionString);
			List<CFP> allCfps = context.Cfps
					.AsNoTracking()
					.Include(c => c.Shindig)
					.ThenInclude(x => x.Country)
					.ToList();

			// Gather distinct years based on the event (Shindig) start date			
			IOrderedEnumerable<int> years = allCfps
					.Select(c => c.EndDate.Year)
					.Distinct()
					.OrderByDescending(y => y);

			// Generate and upload primary CFP listing page (including links to yearly files)
			ctx.Status("Generating Current CFP Listing");
			string openCfpMarkdown = GenerateOpenCFPListing(allCfps, years);
			List<string> uploadResponses = new()
				{
						await UploadFileToGitHub("cfp_tracker.md", openCfpMarkdown, config)
				};

			// Generate and upload yearly CFP listing pages
			foreach (int year in years)
			{
				ctx.Status($"Generating CFP Listing for {year}");
				string yearContent = GenerateYearlyCFPListing(allCfps, year);
				string fileName = $"cfp_tracker_{year}.md";
				uploadResponses.Add(await UploadFileToGitHub(fileName, yearContent, config));
			}

			uploadResponse = string.Join("\n", uploadResponses);
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

/// <summary>
/// Generates a markdown listing for currently open CFPs (those whose deadline is today or in the future).
/// </summary>
string GenerateOpenCFPListing(List<CFP> allCfps, IEnumerable<int> years)
{
	DateOnly today = DateOnly.FromDateTime(DateTime.Today);
	List<CFP> openCfps = allCfps.Where(c => c.EndDate >= today).OrderBy(c => c.EndDate).ToList();

	StringBuilder sb = new();
	sb.AppendLine("# Open Call for Speakers");
	sb.AppendLine();
	sb.AppendLine("This listing includes only the currently open CFPs. Check back later for updates!");
	sb.AppendLine();
	sb.AppendLine("| Conference | Country | City | Conference Start | Conference End | CFP | Benefits | CFP Start | Deadline |");
	sb.AppendLine("| ---------- | ------- | ---- | ---------------- | -------------- | --- | -------- | --------- | -------- |");

	foreach (CFP cfp in openCfps)
	{
		string countryName = cfp.Shindig.Country.Name == "United States of America"
				? "United States"
				: cfp.Shindig.Country.Name;
		string cityWithDivision = cfp.Shindig.Country.Name == "United States of America"
				? $"{cfp.Shindig.City}, {cfp.Shindig.CountryDivisionCode}"
				: cfp.Shindig.City;
		string benefits = $"{(cfp.AreTravelExpensesCovered ? "✈️" : "")} " +
											$"{(cfp.AreAccomodationsProvided ? "🏨" : "")} " +
											$"{(cfp.AreEventFeesCovered ? "🎟️" : "")} " +
											$"{(string.IsNullOrEmpty(cfp.AdditionalBenefits) ? "" : cfp.AdditionalBenefits)}".Trim();

		sb.AppendLine($"| [{cfp.Shindig.Name}]({cfp.Shindig.Url}) | {countryName} | {cityWithDivision} | {cfp.Shindig.StartDate:yyyy-MM-dd} | {cfp.Shindig.EndDate:yyyy-MM-dd} | [CFP]({cfp.CFPUrl}) | {benefits} | {cfp.StartDate:yyyy-MM-dd} | {cfp.EndDate:yyyy-MM-dd} |");
	}

	// Append links to historical CFP pages
	sb.AppendLine();
	sb.AppendLine("## Archived CFPs by Year");
	sb.AppendLine();
	foreach (int year in years.OrderByDescending(y => y))
	{
		sb.AppendLine($"- [CFP Tracker {year}](cfp_tracker_{year}.md)");
	}

	return sb.ToString();
}

/// <summary>
/// Generates a markdown listing for a specific year (all CFPs for events whose start date is in that year).
/// </summary>
string GenerateYearlyCFPListing(List<CFP> allCfps, int year)
{
	// Filter CFPs that belong to the specified year (using the event's start date)
	List<CFP> yearCfps = allCfps
			.Where(c => c.EndDate.Year == year)
			.OrderByDescending(c => c.Shindig.StartDate)
			.ToList();

	StringBuilder sb = new();
	sb.AppendLine($"# CFP Tracker for {year}");
	sb.AppendLine();
	sb.AppendLine($"This archive includes all CFPs (both open and closed) for events happening in {year}.");
	sb.AppendLine();
	sb.AppendLine("| Conference | Country | City | Conference Start | Conference End | CFP | Benefits | CFP Start | Deadline |");
	sb.AppendLine("| ---------- | ------- | ---- | ---------------- | -------------- | --- | -------- | --------- | -------- |");

	foreach (CFP cfp in yearCfps)
	{
		string countryName = cfp.Shindig.Country.Name == "United States of America"
				? "United States"
				: cfp.Shindig.Country.Name;
		string cityWithDivision = cfp.Shindig.Country.Name == "United States of America"
				? $"{cfp.Shindig.City}, {cfp.Shindig.CountryDivisionCode}"
				: cfp.Shindig.City;
		string benefits = $"{(cfp.AreTravelExpensesCovered ? "✈️" : "")} " +
											$"{(cfp.AreAccomodationsProvided ? "🏨" : "")} " +
											$"{(cfp.AreEventFeesCovered ? "🎟️" : "")} " +
											$"{(string.IsNullOrEmpty(cfp.AdditionalBenefits) ? "" : cfp.AdditionalBenefits)}".Trim();

		sb.AppendLine($"| [{cfp.Shindig.Name}]({cfp.Shindig.Url}) | {countryName} | {cityWithDivision} | {cfp.Shindig.StartDate:yyyy-MM-dd} | {cfp.Shindig.EndDate:yyyy-MM-dd} | [CFP]({cfp.CFPUrl}) | {benefits} | {cfp.StartDate:yyyy-MM-dd} | {cfp.EndDate:yyyy-MM-dd} |");
	}

	return sb.ToString();
}

/// <summary>
/// Uploads (creates or updates) a file in the GitHub repository using Octokit.
/// </summary>
async Task<string> UploadFileToGitHub(string filePathInRepo, string markdownContent, IConfigurationRoot config)
{
	const string owner = "TaleLearnCode";
	const string repoName = "CFPCompass";

	string? token = config["GitHub:Token"];
	if (string.IsNullOrEmpty(token))
		return "[bold white on red]GitHub personal access token is not found in Azure App Configuration.[/]";

	try
	{
		GitHubClient github = new(new ProductHeaderValue("CFPCompassUploader"))
		{
			Credentials = new Credentials(token)
		};

		try
		{
			// Try to get the file (if it exists)
			IReadOnlyList<RepositoryContent> existingFileContents = await github.Repository.Content.GetAllContents(owner, repoName, filePathInRepo);
			RepositoryContent existingFile = existingFileContents[0];

			// Update the file with new content.
			UpdateFileRequest updateRequest = new("Update CFP tracker markdown", markdownContent, existingFile.Sha);
			RepositoryContentChangeSet updateResponse = await github.Repository.Content.UpdateFile(owner, repoName, filePathInRepo, updateRequest);
			return $"[bold white on green]File '{filePathInRepo}' updated successfully[/] [link={updateResponse.Content.HtmlUrl}]CFP Listing[/]";
		}
		catch (Octokit.NotFoundException)
		{
			// If the file doesn't exist, create it.
			CreateFileRequest createRequest = new("Add CFP tracker markdown", markdownContent);
			RepositoryContentChangeSet createResponse = await github.Repository.Content.CreateFile(owner, repoName, filePathInRepo, createRequest);
			return $"[bold white on green]File '{filePathInRepo}' created successfully[/] [link={createResponse.Content.HtmlUrl}]CFP Listing[/]";
		}
	}
	catch (Exception ex)
	{
		return $"[bold white on red]An error occurred: {ex.Message}[/]";
	}
}