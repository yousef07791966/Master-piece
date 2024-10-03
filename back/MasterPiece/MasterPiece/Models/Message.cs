using System;
using System.Collections.Generic;

namespace MasterPiece.Models;

public partial class Message
{
    public int Id { get; set; }

    public string? Sender { get; set; }

    public string? Recipient { get; set; }

    public string? MessageContent { get; set; }

    public DateTime? Timestamp { get; set; }
}
