using System;
using System.Collections.Generic;

namespace MasterPice.Models;

public partial class Section
{
    public int SectionId { get; set; }

    public int CourseId { get; set; }

    public string Title { get; set; } = null!;

    public virtual Course Course { get; set; } = null!;

    public virtual ICollection<SectionContent> SectionContents { get; set; } = new List<SectionContent>();
}
