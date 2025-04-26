using System;
using System.Collections.Generic;

namespace MasterPice.Models;

public partial class EnrollmentUser
{
    public int EnrollmentId { get; set; }

    public int? StudentId { get; set; }

    public int? CourseId { get; set; }

    public DateTime? EnrolledAt { get; set; }

    public bool? IsCompleted { get; set; }

    public virtual Course? Course { get; set; }

    public virtual Student? Student { get; set; }
}
