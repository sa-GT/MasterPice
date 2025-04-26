using System;
using System.Collections.Generic;

namespace MasterPice.Models;

public partial class Question
{
    public int QuestionId { get; set; }

    public int? QuizId { get; set; }

    public string QuestionText { get; set; } = null!;

    public string? QuestionType { get; set; }

    public int? CoursesId { get; set; }

    public virtual Course? Courses { get; set; }
}
