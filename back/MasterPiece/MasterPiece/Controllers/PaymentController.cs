using MasterPiece.DTO;
using MasterPiece.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PayPal;

namespace MasterPiece.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {

        //private readonly PayPalPaymentService _payPalService;
        //private readonly MyDbContext _dbContext;

        //public PaymentController(PayPalPaymentService payPalService, MyDbContext dbContext)
        //{
        //    _payPalService = payPalService;
        //    _dbContext = dbContext;
        //}


        //[HttpPost("create-payment")]
        //public IActionResult CreatePayment([FromBody] PaymentRequestDto paymentRequest)
        //{
        //    try
        //    {
        //        var returnUrl = Url.Action("ExecutePayment", "Payment", null, Request.Scheme); // URL for after successful payment
        //        var cancelUrl = Url.Action("CancelPayment", "Payment", null, Request.Scheme);  // URL for when user cancels

        //        // Ensure URLs are set and valid
        //        PayPal.Api.Payment payment = _payPalService.CreatePayment(paymentRequest.Amount, "USD", returnUrl, cancelUrl);

        //        // Generate the transaction ID
        //        var transationId = Guid.NewGuid();

        //        // Save payment information from front-end (UserId) and other details
        //        var newPayment = new E_Commerce_master.Models.Payment
        //        {
        //            UserId = paymentRequest.UserId,  // Get the UserId from the front-end
        //            Amount = paymentRequest.Amount,
        //            PaymentStatus = "Approved",
        //            PaymentDate = DateTime.Now,
        //            PaymentMethod = "Paypal",
        //            TransactionId = transationId.ToString()
        //        };

        //        _dbContext.Payments.Add(newPayment);
        //        _dbContext.SaveChanges();

        //        // Get approval URL
        //        var approvalUrl = payment.links.FirstOrDefault(l => l.rel.Equals("approval_url", StringComparison.OrdinalIgnoreCase))?.href;

        //        if (string.IsNullOrEmpty(approvalUrl))
        //        {
        //            return BadRequest("Could not retrieve the approval URL from PayPal.");
        //        }

        //        return Ok(new { approval_url = approvalUrl });
        //    }
        //    catch (PaymentsException ex)
        //    {
        //        return StatusCode(500, $"PayPal error: {ex.Response}");
        //    }
        //    catch (Exception ex)
        //    {
        //        return StatusCode(500, $"Internal server error: {ex.Message}");
        //    }
        //}



        //[HttpGet("execute-payment")]
        //public IActionResult ExecutePayment(string paymentId, string payerId)
        //{
        //    try
        //    {
        //        // Execute the payment
        //        PayPal.Api.Payment executedPayment = _payPalService.ExecutePayment(paymentId, payerId);

        //        // Check if the payment was approved
        //        if (executedPayment.state.ToLower() != "approved")
        //        {
        //            return BadRequest("Payment was not approved.");
        //        }

        //        // Retrieve the current payment that was created earlier with UserId
        //        var paymentRecord = _dbContext.Payments.FirstOrDefault(p => p.TransactionId == paymentId);
        //        if (paymentRecord == null)
        //        {
        //            return BadRequest("Payment record not found.");
        //        }

        //        // Save the order information with the same UserId
        //        var newOrder = new Models.Order
        //        {
        //            UserId = paymentRecord.UserId, // Use the UserId from the payment record
        //            Amount = Convert.ToDecimal(executedPayment.transactions[0].amount.total),
        //            Status = 1, // Order confirmed
        //            TransactionId = executedPayment.id
        //        };

        //        _dbContext.Orders.Add(newOrder);
        //        _dbContext.SaveChanges();

        //        return Ok("Payment successful");
        //    }
        //    catch (PaymentsException ex)
        //    {
        //        return StatusCode(500, $"PayPal error: {ex.Response}");
        //    }
        //    catch (Exception ex)
        //    {
        //        return StatusCode(500, $"Internal server error: {ex.Message}");
        //    }
        //}


        //private void SavePaymentDetails(PayPal.Api.Payment payment)
        //{
        //    var transaction = payment.transactions[0];

        //    // Assuming currentUserId is available from session, token, or other method
        //    int currentUserId = 1; // Replace with actual logic to get the current user ID

        //    // Save the payment information
        //    var newPayment = new Models.Payment
        //    {
        //        UserId = currentUserId,
        //        Amount = Convert.ToDecimal(transaction.amount.total),
        //        PaymentMethod = payment.payer.payment_method,
        //        TransactionId = payment.id,
        //        PaymentStatus = payment.state,
        //        PaymentDate = DateTime.Now
        //    };

        //    _dbContext.Payments.Add(newPayment);
        //    _dbContext.SaveChanges();

        //    // Save the order information
        //    var newOrder = new Models.Order
        //    {
        //        UserId = currentUserId,
        //        Amount = Convert.ToDecimal(transaction.amount.total),
        //        Status = 1, // Order confirmed
        //        TransactionId = payment.id
        //    };

        //    _dbContext.Orders.Add(newOrder);
        //    _dbContext.SaveChanges();
        //}
        //[HttpGet("cancel-payment")]
        //public IActionResult CancelPayment()
        //{
        //    // Handle payment cancellation here
        //    return BadRequest("Payment was cancelled by the user.");
        //}


    }
}
