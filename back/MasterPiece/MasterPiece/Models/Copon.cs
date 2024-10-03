using System;
using System.Collections.Generic;

namespace MasterPiece.Models;

public partial class Copon
{
    public int CoponId { get; set; }

    public string? Name { get; set; }

    public decimal? DiscountAmount { get; set; }

    public DateTime? ExpiryDate { get; set; }

    public DateTime? CreatedAt { get; set; }

    public bool? IsUsed { get; set; }

    public int? UserId { get; set; }

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();

    public virtual User? User { get; set; }
}
