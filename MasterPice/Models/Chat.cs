using System;
using System.Collections.Generic;

namespace MasterPice.Models;

public partial class Chat
{
    public int ChatId { get; set; }

    public int? SenderId { get; set; }

    public int? ReceiverId { get; set; }

    public int? TeacherId { get; set; }

    public int? ReceiverTid { get; set; }

    public string Message { get; set; } = null!;

    public DateTime? SentAt { get; set; }
}
