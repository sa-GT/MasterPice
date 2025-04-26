using System;
using System.Collections.Generic;

namespace MasterPice.Models;

public partial class OrderDetail
{
    public int OrderDetailId { get; set; }

    public int OrderId { get; set; }

    public int StudentId { get; set; }

    public int CourseId { get; set; }

    public virtual Course Course { get; set; } = null!;

    public virtual Order Order { get; set; } = null!;

    public virtual Student Student { get; set; } = null!;
}
