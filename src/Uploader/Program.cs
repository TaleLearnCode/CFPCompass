try
{
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

				try
				{
					IConfigurationRoot config = GetConfiguration(_azureCredentials, environment);
					string? connectionString = config["SQL:ConnectionString"];
					if (string.IsNullOrEmpty(connectionString))
						throw new Exception("SQL Connection string is not found in Azure App Configuration.");

					ctx.Status("Retrieving CFP data");
					using CFPCompassContext context = CFPCompassContext.CreateDbContext(connectionString);
					List<CFP> allCfps = [.. context.Cfps
								.AsNoTracking()
								.Include(c => c.Shindig)
								.ThenInclude(x => x.Country)];

					// Gather distinct years based on the Shindig start date
					IOrderedEnumerable<int> years = allCfps
								.Select(c => c.EndDate.Year)
								.Distinct()
								.OrderByDescending(y => y);

					ctx.Status("Generating Current CFP Listing");
					string openCfpMarkdown = GenerateOpenCFPListing(allCfps, years);

					List<string> uploadResponses =
					[
						await UploadFileToGitHub("cfp_tracker.md", openCfpMarkdown, config)
					];

					foreach (int year in years)
					{
						try
						{
							ctx.Status($"Generating CFP Listing for {year}");
							string yearContent = GenerateYearlyCFPListing(allCfps, year);
							string fileName = $"cfp_tracker_{year}.md";
							uploadResponses.Add(await UploadFileToGitHub(fileName, yearContent, config));
						}
						catch (Exception ex)
						{
							AnsiConsole.MarkupLine($"[bold red]Error generating/uploading CFP data for {year}: {ex.Message}[/]");
						}
					}

					uploadResponse = string.Join("\n", uploadResponses);
				}
				catch (Exception ex)
				{
					uploadResponse = $"[bold red]An error occurred while processing CFPs: {ex.Message}[/]";
				}
			});

	AnsiConsole.WriteLine();
	AnsiConsole.MarkupLine(uploadResponse);
}
catch (Exception ex)
{
	AnsiConsole.MarkupLine($"[bold white on red]Critical error: {ex.Message}[/]");
}

IConfigurationRoot GetConfiguration(DefaultAzureCredential azureCredential, string environment)
{
	try
	{
		Uri appConfigEndpoint = new("https://appcs-cfpcompass-dev-use2.azconfig.io");
		return new ConfigurationBuilder()
				.AddAzureAppConfiguration(options =>
				{
					options.Connect(appConfigEndpoint, azureCredential)
									.ConfigureKeyVault(kv =>
								{
									kv.SetCredential(azureCredential);
								})
									.Select(KeyFilter.Any, environment)
									.Select(KeyFilter.Any, null);
				})
				.Build();
	}
	catch (Exception ex)
	{
		throw new Exception($"Failed to load configuration: {ex.Message}");
	}
}


/// <summary>
/// Generates a markdown listing for currently open CFPs (those whose deadline is today or in the future).
/// </summary>
string GenerateOpenCFPListing(List<CFP> allCfps, IEnumerable<int> years)
{
	try
	{
		DateOnly today = DateOnly.FromDateTime(DateTime.Today);
		List<CFP> openCfps = [.. allCfps.Where(c => c.EndDate >= today).OrderBy(c => c.EndDate)];

		StringBuilder sb = new();
		sb.AppendLine("# Open Call for Speakers");
		sb.AppendLine();
		sb.AppendLine("This listing includes only the currently open CFPs. Check back later for updates!");
		sb.AppendLine();
		sb.AppendLine("| Conference | Country | City | Conference Start | Conference End | CFP | Benefits | CFP Start | Deadline |");
		sb.AppendLine("| ---------- | ------- | ---- | ---------------- | -------------- | --- | -------- | --------- | -------- |");

		foreach (CFP cfp in openCfps)
		{
			try
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
			catch (Exception ex)
			{
				AnsiConsole.MarkupLine($"[bold red]Failed to process CFP entry: {ex.Message}[/]");
			}
		}

		sb.AppendLine();
		sb.AppendLine("## Archived CFPs by Year");
		sb.AppendLine();
		foreach (int year in years.OrderByDescending(y => y))
		{
			sb.AppendLine($"- [CFP Tracker {year}](cfp_tracker_{year}.md)");
		}

		return sb.ToString();
	}
	catch (Exception ex)
	{
		throw new Exception($"Error generating CFP Markdown: {ex.Message}");
	}
}

/// <summary>
/// Generates a markdown listing for a specific year (all CFPs for events whose start date is in that year).
/// </summary>
string GenerateYearlyCFPListing(List<CFP> allCfps, int year)
{
	try
	{
		// Filter CFPs that belong to the specified year (based on the event's start date)
		List<CFP> yearCfps = [.. allCfps
				.Where(c => c.Shindig.StartDate.Year == year)
				.OrderBy(c => c.Shindig.StartDate)];

		StringBuilder sb = new();
		sb.AppendLine($"# CFP Tracker for {year}");
		sb.AppendLine();
		sb.AppendLine($"This archive includes all CFPs (both open and closed) for events happening in {year}.");
		sb.AppendLine();
		sb.AppendLine("| Conference | Country | City | Conference Start | Conference End | CFP | Benefits | CFP Start | Deadline |");
		sb.AppendLine("| ---------- | ------- | ---- | ---------------- | -------------- | --- | -------- | --------- | -------- |");

		foreach (CFP cfp in yearCfps)
		{
			try
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
			catch (Exception ex)
			{
				AnsiConsole.MarkupLine($"[bold red]Error processing CFP entry for {cfp.Shindig.Name}: {ex.Message}[/]");
			}
		}

		return sb.ToString();
	}
	catch (Exception ex)
	{
		throw new Exception($"Error generating CFP markdown for year {year}: {ex.Message}");
	}
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
		return "[bold white on red]GitHub personal access token is missing in configuration.[/]";

	try
	{
		GitHubClient github = new(new ProductHeaderValue("CFPCompassUploader"))
		{
			Credentials = new Credentials(token)
		};

		try
		{
			IReadOnlyList<RepositoryContent> existingFileContents = await github.Repository.Content.GetAllContents(owner, repoName, filePathInRepo);
			RepositoryContent existingFile = existingFileContents[0];

			UpdateFileRequest updateRequest = new("Update CFP tracker markdown", markdownContent, existingFile.Sha);
			RepositoryContentChangeSet updateResponse = await github.Repository.Content.UpdateFile(owner, repoName, filePathInRepo, updateRequest);
			return $"[bold green]File '{filePathInRepo}' updated successfully[/] [link={updateResponse.Content.HtmlUrl}]CFP Listing[/]";
		}
		catch (Octokit.NotFoundException)
		{
			CreateFileRequest createRequest = new("Add CFP tracker markdown", markdownContent);
			RepositoryContentChangeSet createResponse = await github.Repository.Content.CreateFile(owner, repoName, filePathInRepo, createRequest);
			return $"[bold green]File '{filePathInRepo}' created successfully[/] [link={createResponse.Content.HtmlUrl}]CFP Listing[/]";
		}
	}
	catch (Exception ex)
	{
		return $"[bold red]Error uploading file '{filePathInRepo}': {ex.Message}[/]";
	}
}
