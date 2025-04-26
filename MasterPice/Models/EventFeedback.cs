using System;
using System.Collections.Generic;

namespace MasterPice.Models;

public partial class EventFeedback
{
    public int FeedbackId { get; set; }

    public int? EventId { get; set; }

    public int? StudentId { get; set; }

    public int? Rating { get; set; }

    public string? Comments { get; set; }

    public DateTime? CreatedAt { get; set; }

    public virtual Event? Event { get; set; }

    public virtual Student? Student { get; set; }
}
