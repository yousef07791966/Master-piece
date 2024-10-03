using MasterPiece.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using DinkToPdf;
using MasterPiece.DTO;
using DinkToPdf.Contracts;
using Microsoft.EntityFrameworkCore;


namespace MasterPiece.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly MyDbContext _db;
        private readonly IConverter _converter;
        public OrderController(MyDbContext db, IConverter converter)
        {
            _db = db;
            _converter = converter;
        }

        [HttpGet("download-order/")]
        public IActionResult DownloadOrder()
        {
            var order = _db.Orders.ToList();
            return Ok(order);
        }

        [HttpGet("download-order/{id}")]
        public IActionResult DownloadOrderttt(int id)
        {
            var order = _db.Orders.Where(o => o.UserId == id);

            // Retrieve orders by UserId
            var orderDetails = _db.Orders
                .Join(_db.Users,
                      order => order.UserId,
                      user => user.UserId,
                      (order, user) => new
                      {
                          order.Status,
                          order.Amount,
                          user.Address,
                          user.Name,
                          user.Email,
                          order.UserId,
                          order.OrderId

                      })
                .Where(orderUser => orderUser.UserId == id)
                .ToList();

            return Ok(orderDetails);
        }

        [HttpGet("OrderItem")]

        public IActionResult order(int id)
        {
            var orderItem = _db.OrderItems.Where(c => c.OrderId == id);

            return Ok();
        }


        [HttpPost("CreateOrder/{id:int}")]
        public IActionResult CreateOrder(int id)
        {
            // Get the most recent payment for the user
            var payment = _db.Payments
                                    .Where(p => p.UserId == id)
                                    .OrderByDescending(p => p.PaymentDate)
                                    .FirstOrDefault();

            if (payment == null)
            {
                return NotFound("No payments found for this user.");
            }
            var cartUSer = _db.Carts.Where(p => p.UserId == id).FirstOrDefault();

            // Get cart items for the user
            var cartItemsForUser = _db.CartItems.Where(ci => ci.CartItemId == cartUSer.CartId).ToList();
            if (cartItemsForUser == null || cartItemsForUser.Count == 0)
            {
                return NotFound("No cart items found for this user.");
            }

            // Create the new order
            var order = new Models.Order
            {
    UserId = payment.UserId,
                TransactionId = payment.TransactionId,
                Status = (payment.PaymentStatus == "Approved") ? 1 : 0,  // 1 = Approved, 0 = Not Approved
                Amount = payment.Amount
            };

// Add the order to the Orders table and save it to generate the OrderId
_db.Orders.Add(order);
_db.SaveChanges();  // Save here to get the generated OrderId

// Create OrderItems from CartItems and associate them with the order
foreach (var cartItem in cartItemsForUser)
{
    var orderItem = new OrderItem
    {
        OrderId = order.OrderId,  // Use the generated OrderId
        ProductId = cartItem.ProductId,
        Quantity = cartItem.Quantity
    };

    _db.OrderItems.Add(orderItem);
}

// Remove the cart items after processing
_db.CartItems.RemoveRange(cartItemsForUser);

// Save changes to persist OrderItems and remove CartItems
_db.SaveChanges();

// Map to DTOs before returning the result
var orderDto = new OrderDto
{
    OrderId = order.OrderId,
    UserId = (int)order.UserId,
    TransactionId = order.TransactionId,
    Status = (int)order.Status,
    Amount = (int)order.Amount,
    OrderItems = cartItemsForUser.Select(ci => new OrderItemDto
    {
        ProductId = (int)ci.ProductId,
        Quantity = (int)ci.Quantity
    }).ToList()
};

return Ok(orderDto);  // Return the DTO instead of the entity
        }



        [HttpGet("download-orderItems/{orderId}")]
public async Task<IActionResult> DownloadOrderItems(int orderId)
{
    var orderItems = await _db.OrderItems
                        .Include(oi => oi.Product)
                        .Where(oi => oi.OrderId == orderId)
                        .ToListAsync();

    if (orderItems == null || !orderItems.Any())
    {
        return NotFound("No order items found for this order.");
    }


    var result = orderItems.Select(oi => new
    {
        ProductName = oi.Product?.Name,
        ProductPrice = oi.Product?.Price,
        ProductBrand = oi.Product?.Brand,
        OrderQuantity = oi.Quantity
    }).ToList();

    return Ok(result);
}


[HttpGet("InvoiceByOrderID/({id})")]
public IActionResult InvoiceByOrderID(int id)
{
    if (id <= 0) { return BadRequest(); }
    var OrderInvoice = _db.OrderItems.Where(x => x.OrderId == id).ToList();
    if (OrderInvoice == null) { return NotFound(); }

    return Ok(OrderInvoice);
}

[HttpGet("GenerateInvoice")]
public IActionResult GenerateInvoice(int orderId)
{
    var orderItems = _db.OrderItems
        .Where(oi => oi.OrderId == orderId)
        .Include(oi => oi.Product)
        .ToList();

    var pdfDocument = new HtmlToPdfDocument
    {
        GlobalSettings = {
                DocumentTitle = $"Invoice for Order {orderId}",
                PaperSize = PaperKind.A4,
                Orientation = Orientation.Portrait
            },
        Objects = {
                new ObjectSettings
                {
                    HtmlContent = GenerateInvoiceHtml(orderItems),
                    WebSettings = { DefaultEncoding = "utf-8" }
                }
            }
    };

    var pdf = _converter.Convert(pdfDocument);

    return File(pdf, "application/pdf", $"invoice_{orderId}.pdf");
}

private string GenerateInvoiceHtml(List<OrderItem> orderItems)
{
    var html = @"
    <html>
    <head>
        <link href='https://fonts.googleapis.com/css2?family=Roboto:wght@400;700&display=swap' rel='stylesheet'>
        <style>
             body {
        font-family: 'Roboto', sans-serif;
        color: #333;
        background-color: #f9f9f9;
        margin: 0;
        padding: 0;
    }
    .container {
        width: 80%;
        margin: auto;
        background-color: #fff;
        padding: 20px;
        box-shadow: 0 0 10px rgba(0, 0, 0, 0.1);
        border-radius: 8px;
    }
    .header {
        text-align: center;
        margin-bottom: 20px;
        color:#F53737;
    }
    .header h1 {
        margin: 0;
        font-size: 2.5em;
        font-weight: 700;
    }
    .header h2 {
        margin: 0;
        font-size: 1.5em;
        font-weight: 400;
    }
    table {
        width: 100%;
        border-collapse: collapse;
        margin: 20px 0;
    }
    th, td {
        border: 1px solid #ddd;
        padding: 12px;
        text-align: center;
    }
    th {
        background-color: #e0e0e0; /* Changed to gray */
        color: #333;
        font-weight: 700;
        text-transform: capitalize;
    }
    td {
        padding: 15px;
    }
    .view-link {
        color: red; /* Red color for download link */
        font-weight: bold;
        text-decoration: none;
    }
    .view-link:hover {
        text-decoration: underline;
    }
    tr:nth-child(even) {
        background-color: #f9f9f9;
    }
    .footer {
        margin-top: 20px;
        text-align: center;
        font-size: 1.2em;
        color: #F53737;
    }
    .total {
        font-weight: 700;
    }
        </style>
    </head>
    <body>
        <div class='container'>
            <div class='header'>
                <h1>Dorno</h1>
                <h2>Invoice</h2>
            </div>
            <table>
                <thead>
                    <tr>
                        <th>Products Name</th>
                        <th>Price</th>
                        <th>Brand</th>
                        <th>Quantity</th>
                        <th>Total Price</th>
                    </tr>
                </thead>
                <tbody>";

    foreach (var item in orderItems)
    {
        var productName = item.Product?.Name ?? "Unknown";
        var price = item.Product?.Price ?? 0;
        var quantity = item.Quantity ?? 0;
        var total = price * quantity;
        var brand = item.Product.Brand;

        html += $@"
                    <tr>
                        <td>{productName}</td>
                        <td>${price:F2}</td>
                        <td>{brand}</td>
                        <td>{quantity}</td>
                        <td>${total:F2}</td>
                    </tr>";
    }

    var totalAmount = orderItems.Sum(oi => oi.Product.Price * oi.Quantity) ?? 0;

    html += $@"
                </tbody>
            </table>
            <div class='footer'>
                <p class='total'>Total Amount: ${totalAmount:F2}</p>
            </div>
        </div>
    </body>
    </html>";

    return html;
}


    }
}
