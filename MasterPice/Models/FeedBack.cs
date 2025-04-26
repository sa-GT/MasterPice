using System;
using System.Collections.Generic;

namespace MasterPice.Models;

public partial class FeedBack
{
    public string? FeedbackContent { get; set; }

    public string? Username { get; set; }

    public int FeedbackId { get; set; }

    public string? Email { get; set; }
}
