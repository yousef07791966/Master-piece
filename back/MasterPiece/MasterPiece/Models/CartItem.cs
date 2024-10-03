using System;
using System.Collections.Generic;

namespace MasterPiece.Models;

public partial class CartItem
{
    public int CartItemId { get; set; }

    public int? ProductId { get; set; }

    public int? UserId { get; set; }

    public int? Quantity { get; set; }

    public virtual Product? Product { get; set; }

    public virtual User? User { get; set; }
}
