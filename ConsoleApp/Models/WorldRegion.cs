using System;
using System.Collections.Generic;

namespace ConsoleApp.Models;

/// <summary>
/// Lookup table representing the world regions as defined by the UN M49 specification.
/// </summary>
public partial class WorldRegion
{
    /// <summary>
    /// Identifier of the world region.
    /// </summary>
    public string WorldRegionCode { get; set; } = null!;

    /// <summary>
    /// Name of the world region.
    /// </summary>
    public string WorldRegionName { get; set; } = null!;

    /// <summary>
    /// Identifier of the world region parent (for subregions).
    /// </summary>
    public string? ParentId { get; set; }

    /// <summary>
    /// Flag indicating whether the world region is enabled.
    /// </summary>
    public bool IsEnabled { get; set; }

    public virtual ICollection<Country> Countries { get; set; } = new List<Country>();

    public virtual ICollection<WorldRegion> InverseParent { get; set; } = new List<WorldRegion>();

    public virtual WorldRegion? Parent { get; set; }
}
