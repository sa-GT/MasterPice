using System;
using System.Collections.Generic;

namespace MasterPice.Models;

public partial class Course
{
    public int CourseId { get; set; }

    public string Title { get; set; } = null!;

    public string? Description { get; set; }

    public int? TeacherId { get; set; }

    public string? Category { get; set; }

    public string? Level { get; set; }

    public decimal? Price { get; set; }

    public DateTime? CreatedAt { get; set; }

    public string? VideoLink { get; set; }

    public int? EnrolledStudentsCount { get; set; }

    public int? LessonCount { get; set; }

    public string? CourseCover { get; set; }

    public string? IsApproved { get; set; }

    public virtual ICollection<Assignment> Assignments { get; set; } = new List<Assignment>();

    public virtual ICollection<CartItem> CartItems { get; set; } = new List<CartItem>();

    public virtual ICollection<EnrollmentUser> EnrollmentUsers { get; set; } = new List<EnrollmentUser>();

    public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();

    public virtual ICollection<Question> Questions { get; set; } = new List<Question>();

    public virtual ICollection<Section> Sections { get; set; } = new List<Section>();

    public virtual Teacher? Teacher { get; set; }
}
