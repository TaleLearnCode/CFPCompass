using Microsoft.EntityFrameworkCore;

namespace ConsoleApp.Models;

public partial class CFPCompassContext(DbContextOptions<CFPCompassContext> options) : DbContext(options)
{

	public static CFPCompassContext CreateDbContext(string connectionString)
	{
		var optionsBuilder = new DbContextOptionsBuilder<CFPCompassContext>();
		optionsBuilder.UseSqlServer(connectionString);
		return new CFPCompassContext(optionsBuilder.Options);
	}

	public virtual DbSet<Cfp> Cfps { get; set; }

	public virtual DbSet<Cfptype> Cfptypes { get; set; }

	public virtual DbSet<Country> Countries { get; set; }

	public virtual DbSet<CountryDivision> CountryDivisions { get; set; }

	public virtual DbSet<Language> Languages { get; set; }

	public virtual DbSet<Shindig> Shindigs { get; set; }

	public virtual DbSet<ShindigStatus> ShindigStatuses { get; set; }

	public virtual DbSet<ShindigType> ShindigTypes { get; set; }

	public virtual DbSet<TimeZone> TimeZones { get; set; }

	public virtual DbSet<WorldRegion> WorldRegions { get; set; }

	protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
	{
		if (!optionsBuilder.IsConfigured)
			throw new InvalidOperationException("The connection string must be provided via DbContextOptions.");
	}

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		modelBuilder.Entity<Cfp>(entity =>
		{
			entity.HasKey(e => e.ShindigPermalink).HasName("pkcCFP");

			entity.ToTable("CFP");

			entity.Property(e => e.ShindigPermalink)
							.HasMaxLength(200)
							.IsUnicode(false);
			entity.Property(e => e.AdditionalBenefits).HasMaxLength(2000);
			entity.Property(e => e.CfptypeId).HasColumnName("CFPTypeId");
			entity.Property(e => e.Cfpurl)
							.HasMaxLength(200)
							.IsUnicode(false)
							.HasColumnName("CFPURL");
			entity.Property(e => e.EventFeesCovered).HasDefaultValue(true);

			entity.HasOne(d => d.Cfptype).WithMany(p => p.Cfps)
							.HasForeignKey(d => d.CfptypeId)
							.OnDelete(DeleteBehavior.ClientSetNull)
							.HasConstraintName("fkCFP_CFPType");

			entity.HasOne(d => d.Shindig).WithOne(p => p.Cfp)
							.HasForeignKey<Cfp>(d => d.ShindigPermalink)
							.OnDelete(DeleteBehavior.ClientSetNull)
							.HasConstraintName("fkCFP_Shindig");
		});

		modelBuilder.Entity<Cfptype>(entity =>
		{
			entity.HasKey(e => e.CfptypeId).HasName("pkcCFPType");

			entity.ToTable("CFPType");

			entity.Property(e => e.CfptypeId)
							.ValueGeneratedNever()
							.HasColumnName("CFPTypeId");
			entity.Property(e => e.CfptypeDescription)
							.HasMaxLength(500)
							.HasColumnName("CFPTypeDescription");
			entity.Property(e => e.CfptypeName)
							.HasMaxLength(100)
							.HasColumnName("CFPTypeName");
		});

		modelBuilder.Entity<Country>(entity =>
		{
			entity.HasKey(e => e.CountryCode).HasName("pkcCountry");

			entity.ToTable("Country", tb => tb.HasComment("Lookup table representing the countries as defined by the ISO 3166-1 standard."));

			entity.HasIndex(e => e.WorldRegionCode, "idxCountry_WorldRegionCode");

			entity.Property(e => e.CountryCode)
							.HasMaxLength(2)
							.IsUnicode(false)
							.IsFixedLength()
							.HasComment("Identifier of the country using the ISO 3166-1 Alpha-2 code.");
			entity.Property(e => e.CountryName)
							.HasMaxLength(100)
							.IsUnicode(false)
							.HasComment("Name of the country using the ISO 3166-1 Country Name.");
			entity.Property(e => e.DivisionName)
							.HasMaxLength(100)
							.IsUnicode(false)
							.HasComment("The primary name of the country's divisions.");
			entity.Property(e => e.HasDivisions).HasComment("Flag indicating whether the country has divisions (states, provinces, etc.)");
			entity.Property(e => e.IsEnabled).HasComment("Flag indicating whether the country record is enabled.");
			entity.Property(e => e.M49code)
							.HasMaxLength(3)
							.IsUnicode(false)
							.IsFixedLength()
							.HasComment("Identifier of the country using the United Nations M49 standard.")
							.HasColumnName("M49Code");
			entity.Property(e => e.WorldRegionCode)
							.HasMaxLength(3)
							.IsUnicode(false)
							.IsFixedLength()
							.HasComment("Identifier of the world region where the country is located.");

			entity.HasOne(d => d.WorldRegionCodeNavigation).WithMany(p => p.Countries)
							.HasForeignKey(d => d.WorldRegionCode)
							.OnDelete(DeleteBehavior.ClientSetNull)
							.HasConstraintName("fkCountry_WorldRegion");
		});

		modelBuilder.Entity<CountryDivision>(entity =>
		{
			entity.HasKey(e => new { e.CountryCode, e.CountryDivisionCode }).HasName("pkcCountryDivision");

			entity.ToTable("CountryDivision", tb => tb.HasComment("Lookup table representing the world regions as defined by the ISO 3166-2 standard."));

			entity.Property(e => e.CountryCode)
							.HasMaxLength(2)
							.IsUnicode(false)
							.IsFixedLength();
			entity.Property(e => e.CountryDivisionCode)
							.HasMaxLength(3)
							.IsUnicode(false)
							.IsFixedLength()
							.HasComment("Identifier of the country division using the ISO 3166-2 Alpha-2 code.");
			entity.Property(e => e.CategoryName)
							.HasMaxLength(100)
							.IsUnicode(false)
							.HasComment("The category name of the country division.");
			entity.Property(e => e.CountryDivisionName)
							.HasMaxLength(100)
							.IsUnicode(false)
							.HasComment("Name of the country using the ISO 3166-2 Subdivision Name.");
			entity.Property(e => e.IsEnabled).HasComment("Flag indicating whether the country division record is enabled.");

			entity.HasOne(d => d.CountryCodeNavigation).WithMany(p => p.CountryDivisions)
							.HasForeignKey(d => d.CountryCode)
							.OnDelete(DeleteBehavior.ClientSetNull)
							.HasConstraintName("fkCountryDivision_Country");
		});

		modelBuilder.Entity<Language>(entity =>
		{
			entity.HasKey(e => e.LanguageCode).HasName("pkcLangauge");

			entity.ToTable("Language", tb => tb.HasComment("Represents a spoken/written language."));

			entity.Property(e => e.LanguageCode)
							.HasMaxLength(2)
							.IsUnicode(false)
							.IsFixedLength()
							.HasComment("Identifier of the language.");
			entity.Property(e => e.IsEnabled).HasComment("Flag indicating whether the language is enabled.");
			entity.Property(e => e.LanguageName)
							.HasMaxLength(100)
							.HasComment("Name of the language.");
			entity.Property(e => e.NativeName)
							.HasMaxLength(100)
							.HasComment("Native name of the language.");
		});

		modelBuilder.Entity<Shindig>(entity =>
		{
			entity.HasKey(e => e.Permalink).HasName("pkcShindig");

			entity.ToTable("Shindig");

			entity.Property(e => e.Permalink)
							.HasMaxLength(200)
							.IsUnicode(false);
			entity.Property(e => e.City).HasMaxLength(100);
			entity.Property(e => e.CountryCode)
							.HasMaxLength(2)
							.IsUnicode(false)
							.IsFixedLength();
			entity.Property(e => e.CountryDivisionCode)
							.HasMaxLength(3)
							.IsUnicode(false)
							.IsFixedLength();
			entity.Property(e => e.IsEnabled).HasDefaultValue(true);
			entity.Property(e => e.ShindigDescription).HasMaxLength(2000);
			entity.Property(e => e.ShindigImageUrl)
							.HasMaxLength(200)
							.IsUnicode(false);
			entity.Property(e => e.ShindigName).HasMaxLength(200);
			entity.Property(e => e.ShindigUrl)
							.HasMaxLength(200)
							.IsUnicode(false);
			entity.Property(e => e.TimeZoneId)
							.HasMaxLength(100)
							.IsUnicode(false);
			entity.Property(e => e.Venue).HasMaxLength(200);

			entity.HasOne(d => d.CountryCodeNavigation).WithMany(p => p.Shindigs)
							.HasForeignKey(d => d.CountryCode)
							.OnDelete(DeleteBehavior.ClientSetNull)
							.HasConstraintName("fkShindig_Country");

			entity.HasOne(d => d.ShindigStatus).WithMany(p => p.Shindigs)
							.HasForeignKey(d => d.ShindigStatusId)
							.OnDelete(DeleteBehavior.ClientSetNull)
							.HasConstraintName("fkShindig_ShindigStatus");

			entity.HasOne(d => d.ShindigType).WithMany(p => p.Shindigs)
							.HasForeignKey(d => d.ShindigTypeId)
							.OnDelete(DeleteBehavior.ClientSetNull)
							.HasConstraintName("fkShindig_ShindigType");

			entity.HasOne(d => d.TimeZone).WithMany(p => p.Shindigs)
							.HasForeignKey(d => d.TimeZoneId)
							.OnDelete(DeleteBehavior.ClientSetNull)
							.HasConstraintName("fkShindig_TimeZone");

			entity.HasOne(d => d.CountryDivision).WithMany(p => p.Shindigs)
							.HasForeignKey(d => new { d.CountryCode, d.CountryDivisionCode })
							.HasConstraintName("fkShindig_CountryDivision");
		});

		modelBuilder.Entity<ShindigStatus>(entity =>
		{
			entity.HasKey(e => e.ShindigStatusId).HasName("pkcShindigStatus");

			entity.ToTable("ShindigStatus", tb => tb.HasComment("Represents a status of a shindig."));

			entity.Property(e => e.ShindigStatusId)
							.ValueGeneratedNever()
							.HasComment("The identifier of the shindig status record.");
			entity.Property(e => e.IsEnabled).HasComment("Flag indicating whether the shindig status is enabled.");
			entity.Property(e => e.ShindigStatusDescription)
							.HasMaxLength(500)
							.HasComment("A description of the shindig status.");
			entity.Property(e => e.ShindigStatusName)
							.HasMaxLength(100)
							.HasComment("The name of the shindig status.");
			entity.Property(e => e.SortOrder).HasComment("The sorting order of the shindig status.");
		});

		modelBuilder.Entity<ShindigType>(entity =>
		{
			entity.HasKey(e => e.ShindigTypeId).HasName("pkcShindigType");

			entity.ToTable("ShindigType", tb => tb.HasComment("Represents a type of a shindig."));

			entity.Property(e => e.ShindigTypeId)
							.ValueGeneratedNever()
							.HasComment("The identifier of the shindig type record.");
			entity.Property(e => e.IsEnabled).HasComment("Flag indicating whether the shindig type is enabled.");
			entity.Property(e => e.ShindigTypeDescription)
							.HasMaxLength(500)
							.HasComment("A description of the shindig type.");
			entity.Property(e => e.ShindigTypeName)
							.HasMaxLength(100)
							.HasComment("The name of the shindig type.");
			entity.Property(e => e.SortOrder).HasComment("The sorting order of the shindig type.");
		});

		modelBuilder.Entity<TimeZone>(entity =>
		{
			entity.HasKey(e => e.TimeZoneId).HasName("pkcTimeZone");

			entity.ToTable("TimeZone", tb => tb.HasComment("Represents the list of time zones as defined by the IANA."));

			entity.Property(e => e.TimeZoneId)
							.HasMaxLength(100)
							.IsUnicode(false)
							.HasComment("The identifier of the time zone as defined by the IANA.");
			entity.Property(e => e.DaylightOffset)
							.HasMaxLength(6)
							.IsUnicode(false)
							.IsFixedLength()
							.HasComment("The daylight offset for the time zone.");
			entity.Property(e => e.DaylightSavingsAbbreviation)
							.HasMaxLength(7)
							.IsUnicode(false)
							.IsFixedLength()
							.HasComment("The daylight savings abbreviation for the time zone.");
			entity.Property(e => e.StandardAbbreviation)
							.HasMaxLength(7)
							.IsUnicode(false)
							.IsFixedLength()
							.HasComment("The standard abbreviation for the time zone.");
			entity.Property(e => e.StandardOffset)
							.HasMaxLength(6)
							.IsUnicode(false)
							.IsFixedLength()
							.HasComment("The standard offset for the time zone.");
		});

		modelBuilder.Entity<WorldRegion>(entity =>
		{
			entity.HasKey(e => e.WorldRegionCode).HasName("pkcWorldRegion");

			entity.ToTable("WorldRegion", tb => tb.HasComment("Lookup table representing the world regions as defined by the UN M49 specification."));

			entity.HasIndex(e => e.ParentId, "idxWorldRegion_ParentId");

			entity.Property(e => e.WorldRegionCode)
							.HasMaxLength(3)
							.IsUnicode(false)
							.IsFixedLength()
							.HasComment("Identifier of the world region.");
			entity.Property(e => e.IsEnabled).HasComment("Flag indicating whether the world region is enabled.");
			entity.Property(e => e.ParentId)
							.HasMaxLength(3)
							.IsUnicode(false)
							.IsFixedLength()
							.HasComment("Identifier of the world region parent (for subregions).");
			entity.Property(e => e.WorldRegionName)
							.HasMaxLength(100)
							.IsUnicode(false)
							.HasComment("Name of the world region.");

			entity.HasOne(d => d.Parent).WithMany(p => p.InverseParent)
							.HasForeignKey(d => d.ParentId)
							.HasConstraintName("fkWorldRegion_WorldRegion");
		});

		OnModelCreatingPartial(modelBuilder);
	}

	partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
