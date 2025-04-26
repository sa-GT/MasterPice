using System;
using System.Collections.Generic;

namespace MasterPice.Models;

public partial class Comment
{
    public int CommentId { get; set; }

    public int? PostId { get; set; }

    public int? UserId { get; set; }

    public int? TeacherId { get; set; }

    public string? CommentText { get; set; }

    public DateTime? CreatedAt { get; set; }
}
