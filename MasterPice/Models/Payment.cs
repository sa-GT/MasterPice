using System;
using System.Collections.Generic;

namespace MasterPice.Models;

public partial class Payment
{
    public int PaymentId { get; set; }

    public int CartId { get; set; }

    public int StudentId { get; set; }

    public string? Address { get; set; }

    public string? Email { get; set; }

    public string? FullName { get; set; }

    public string? PhoneNumber { get; set; }

    public string? PaymentMethod { get; set; }

    public decimal? TotalCost { get; set; }

    public bool? IsPayed { get; set; }

    public DateTime? PaidAt { get; set; }

    public virtual Cart Cart { get; set; } = null!;

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();

    public virtual Student Student { get; set; } = null!;
}
