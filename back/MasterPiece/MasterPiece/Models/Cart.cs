using System;
using System.Collections.Generic;

namespace MasterPiece.Models;

public partial class Cart
{
    public int CartId { get; set; }

    public int? UserId { get; set; }

    public virtual User? User { get; set; }
}
