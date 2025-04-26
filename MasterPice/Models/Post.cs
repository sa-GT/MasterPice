using System;
using System.Collections.Generic;

namespace MasterPice.Models;

public partial class Post
{
    public int PostId { get; set; }

    public int? UserId { get; set; }

    public int? CourseId { get; set; }

    public int? TeacherId { get; set; }

    public string? Title { get; set; }

    public string? Content { get; set; }

    public DateTime? CreatedAt { get; set; }
}
