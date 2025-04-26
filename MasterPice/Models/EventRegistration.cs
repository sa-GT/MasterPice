using System;
using System.Collections.Generic;

namespace MasterPice.Models;

public partial class EventRegistration
{
    public int RegistrationId { get; set; }

    public int? EventId { get; set; }

    public int? StudentId { get; set; }

    public DateTime? RegisteredAt { get; set; }

    public virtual Event? Event { get; set; }

    public virtual Student? Student { get; set; }
}
