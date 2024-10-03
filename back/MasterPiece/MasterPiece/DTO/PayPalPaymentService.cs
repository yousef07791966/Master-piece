using MasterPiece.Models;
using System;

namespace MasterPiece.DTO
{
    public class PayPalPaymentService
    {
        private readonly string _clientId;
        private readonly string _clientSecret;
        private readonly MyDbContext _db;


    //    public PayPalPaymentService(IConfiguration config, MyDbContext db)
    //    {
    //        _clientId = config["PayPal:ClientId"];
    //        _clientSecret = config["PayPal:ClientSecret"];
    //        _db = db;

    //    }

    //    // Method to create a payment
    //    public PayPal.Api.Payment CreatePayment(decimal amount, string currency, string returnUrl, string cancelUrl)
    //    {
    //        var apiContext = GetAPIContext();

    //        var payer = new Payer { payment_method = "paypal" };

    //        // Make sure cancelUrl and returnUrl are not null
    //        if (string.IsNullOrEmpty(returnUrl) || string.IsNullOrEmpty(cancelUrl))
    //        {
    //            throw new ArgumentException("Return URL and Cancel URL are required.");
    //        }

    //        var redirUrls = new RedirectUrls
    //        {
    //            cancel_url = cancelUrl,  // This is required when payment method is PayPal
    //            return_url = returnUrl   // Redirect the user back after approval
    //        };

    //        var details = new Details { tax = "0", shipping = "0", subtotal = amount.ToString() };

    //        var amountObj = new Amount
    //        {
    //            currency = currency,
    //            total = amount.ToString(), // Total must match details
    //            details = details
    //        };

    //        var transactionList = new List<Transaction>
    //{
    //    new Transaction
    //    {
    //        description = "Payment for your order",
    //        amount = amountObj,

    //    }
    //};

    //        var payment = new PayPal.Api.Payment
    //        {
    //            intent = "sale",
    //            payer = payer,
    //            transactions = transactionList,
    //            redirect_urls = redirUrls  // Make sure redirect URLs are set
    //        };

    //        //var transationId = Guid.NewGuid();
    //        //var payment1 = new E_Commerce_Clothes.Models.Payment
    //        //{
    //        //    UserId = 1,
    //        //    Amount = amount,
    //        //    PaymentStatus =  "Approved",
    //        //    PaymentDate = DateTime.Now ,
    //        //    PaymentMethod = "Paypal",
    //        //    TransactionId = Convert.ToString(transationId)



    //        //};
    //        //_db.Payments.Add(payment1);
    //        //_db.SaveChanges();

    //        return payment.Create(apiContext);  // Create payment
    //    }


    //    // Method to execute a payment
    //    public PayPal.Api.Payment ExecutePayment(string paymentId, string payerId)
    //    {
    //        var apiContext = GetAPIContext();

    //        var paymentExecution = new PaymentExecution { payer_id = payerId };
    //        var payment = new PayPal.Api.Payment { id = paymentId };

    //        // Execute the payment
    //        return payment.Execute(apiContext, paymentExecution);
    //    }

    //    // Helper method to get the PayPal API context
    //    private APIContext GetAPIContext()
    //    {
    //        var accessToken = new OAuthTokenCredential(_clientId, _clientSecret).GetAccessToken();
    //        return new APIContext(accessToken);
    //    }
    }
}
