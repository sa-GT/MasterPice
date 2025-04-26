using System;
using System.Collections.Generic;

namespace MasterPice.Models;

public partial class SectionContent
{
    public int ContentId { get; set; }

    public int? SectionId { get; set; }

    public string? Title { get; set; }

    public string? ContentText { get; set; }

    public string? ContentType { get; set; }

    public TimeOnly? Duration { get; set; }

    public string? Description { get; set; }

    public string? Requirement { get; set; }

    public virtual Section? Section { get; set; }
}
