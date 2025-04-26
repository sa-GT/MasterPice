using System;
using System.Collections.Generic;

namespace MasterPice.Models;

public partial class AssignmentSubmission
{
    public int SubmissionId { get; set; }

    public int? AssignmentId { get; set; }

    public int? StudnetsId { get; set; }

    public string? SubmissionFile { get; set; }

    public DateTime? SubmittedAt { get; set; }

    public int? Score { get; set; }

    public virtual Assignment? Assignment { get; set; }

    public virtual Student? Studnets { get; set; }
}
