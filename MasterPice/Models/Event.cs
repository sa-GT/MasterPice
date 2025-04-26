using System;
using System.Collections.Generic;

namespace MasterPice.Models;

public partial class Event
{
    public int EventId { get; set; }

    public string? Title { get; set; }

    public string? Description { get; set; }

    public string? EventType { get; set; }

    public DateTime StartDate { get; set; }

    public DateTime EndDate { get; set; }

    public string? Location { get; set; }

    public string? CreatedBy { get; set; }

    public string? EventCover { get; set; }

    public virtual ICollection<EventFeedback> EventFeedbacks { get; set; } = new List<EventFeedback>();

    public virtual ICollection<EventRegistration> EventRegistrations { get; set; } = new List<EventRegistration>();
}
