using System;
using System.Collections.Generic;

namespace MasterPiece.Models;

public partial class Contact
{
    public int ContactId { get; set; }

    public string Name { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string? Subject { get; set; }

    public string? Message { get; set; }

    public DateOnly? SentDate { get; set; }

    public string? AdminResponse { get; set; }

    public DateOnly? ResponseDate { get; set; }

    public string? Status { get; set; }
}
