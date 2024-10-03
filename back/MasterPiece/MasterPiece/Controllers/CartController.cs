using MasterPiece.DTO;
using MasterPiece.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MasterPiece.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private readonly MyDbContext _db;

        public CartController(MyDbContext db)
        {
            _db = db;

        }

        [HttpPost("AddCartItem/{UserId}")]
        public IActionResult AddCartItem([FromBody] addCartItemRequestDTO newItem, int UserId)
        {
            // Check if the user has a cart
            var user = _db.Carts.FirstOrDefault(x => x.UserId == UserId);

            if (user == null)
            {
                return NotFound("Cart not found for this user.");
            }

            // Check if the product is already in the user's cart
            var checkSelectedProduct = _db.CartItems.FirstOrDefault(x => x.ProductId == newItem.ProductId && x.CartItemId == user.CartId);

            if (checkSelectedProduct == null)
            {
                // Add new product to cart
                var data = new CartItem
                {
                    
                    CartItemId= user.CartId,
                    ProductId = newItem.ProductId,
                    Quantity = newItem.Quantity,
                };

                _db.CartItems.Add(data);
                _db.SaveChanges();
                return Ok("Product added to cart");
            }
            else
            {
                // Update the quantity of the existing product in the cart
                checkSelectedProduct.Quantity += newItem.Quantity;

                _db.CartItems.Update(checkSelectedProduct);
                _db.SaveChanges();
                return Ok("Quantity of product increased");
            }
        }


        [HttpGet("getUserCartItems/{UserId}")]
        public IActionResult getUserCartItems(int UserId)
        {

            var user = _db.Carts.FirstOrDefault(x => x.UserId == UserId);

            var cartItem = _db.CartItems.Where(c => c.CartItemId == user.CartId).Select(
             x => new cartItemResponseDTO
             {
                 CartItemId = x.CartItemId,
                 CartId = x.CartItemId,
                 Product = new productDTO
                 {
                     ProductId = x.Product.ProductId,
                     Name = x.Product.Name,
                     Price = x.Product.Price,
                     Image = x.Product.Image,
                     PriceWithDiscount = x.Product.PriceWithDiscount,
                 },
                 Quantity = x.Quantity,
             });



            return Ok(cartItem);
        }

        [HttpDelete("deleteItemById/{cartItemId}")]
        public IActionResult deleteItemById(int cartItemId)
        {
            var delItem = _db.CartItems.Find(cartItemId);
            _db.CartItems.RemoveRange(delItem);
            _db.SaveChanges();

            return Ok($"Category '{cartItemId}' deleted successfully.");
        }


        [HttpPost("changeQuantity")]
        public IActionResult changeQuantity([FromBody] changeQuantityDTO update)
        {
            var item = _db.CartItems.Find(update.CartItemId);

            if (update.Quantity == 0)
            {
                _db.Remove(item);
                _db.SaveChanges(true);
                return Ok("item was deleted");
            }

            item.Quantity = update.Quantity;
            _db.SaveChanges();
            return Ok();
        }



        [HttpGet("ApplyVoucher/{code}")]
        public IActionResult ApplyVoucher(string code)
        {

            var voucher = _db.Vouchers.FirstOrDefault(v => v.Code == code);



            if (voucher != null && voucher.IsUsed == false && voucher.ExpiryDate.Date >= DateTime.Now.Date)
            {
                voucher.IsUsed = true;
                _db.Vouchers.Update(voucher);
                _db.SaveChanges();


                return Ok(new { Discount = voucher.DiscountAmount });

            }
            else
            {
                return NotFound(new { message = "Invalid voucher code." });

            }

        }



    }
}
