using System;
using System.Collections.Generic;

namespace MasterPice.Models;

public partial class Student
{
    public int StudentId { get; set; }

    public string FullName { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string PasswordHash { get; set; } = null!;

    public string? ProfilePicture { get; set; }

    public string? Interests { get; set; }

    public DateTime? CreatedAt { get; set; }

    public string? CoverPicture { get; set; }

    public string? IsTeaching { get; set; }

    public virtual ICollection<AssignmentSubmission> AssignmentSubmissions { get; set; } = new List<AssignmentSubmission>();

    public virtual ICollection<Cart> Carts { get; set; } = new List<Cart>();

    public virtual ICollection<EnrollmentUser> EnrollmentUsers { get; set; } = new List<EnrollmentUser>();

    public virtual ICollection<EventFeedback> EventFeedbacks { get; set; } = new List<EventFeedback>();

    public virtual ICollection<EventRegistration> EventRegistrations { get; set; } = new List<EventRegistration>();

    public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();

    public virtual ICollection<Payment> Payments { get; set; } = new List<Payment>();
}
