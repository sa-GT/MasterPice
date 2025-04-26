using System;
using System.Collections.Generic;

namespace MasterPice.Models;

public partial class Order
{
    public int OrderId { get; set; }

    public int PaymentId { get; set; }

    public int CartId { get; set; }

    public virtual Cart Cart { get; set; } = null!;

    public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();

    public virtual Payment Payment { get; set; } = null!;
}
