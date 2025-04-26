using System;
using System.Collections.Generic;

namespace MasterPice.Models;

public partial class Cart
{
    public int CartId { get; set; }

    public int StudentId { get; set; }

    public DateTime? CreatedAt { get; set; }

    public virtual ICollection<CartItem> CartItems { get; set; } = new List<CartItem>();

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();

    public virtual ICollection<Payment> Payments { get; set; } = new List<Payment>();

    public virtual Student Student { get; set; } = null!;
}
