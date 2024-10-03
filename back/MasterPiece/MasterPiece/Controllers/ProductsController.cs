using MasterPiece.DTO;
using MasterPiece.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MasterPiece.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {

        private readonly MyDbContext _db;
        public ProductsController(MyDbContext db)
        {
            _db = db;

        }


        [HttpGet("GetAllProducts")]
        public IActionResult GetAllProducts()
        {
            var products = _db.Products.ToList();
            if (!products.Any()) { return NotFound("No product found."); }
            return Ok(products);
        }

        /// /////////////////////////////////////////////////////////////////////
       

        [HttpGet("GetProductByID/{id}")]
        public IActionResult GetProductByID(int id)
        {
            if (id <= 0) { return BadRequest(); }
            var product = _db.Products.Find(id);
            if (product == null) { return NotFound("No product found."); }




            return Ok(product);
        }
        /// /////////////////////////////////////////////////////////////////////

        [HttpGet("GetProductByIDStars/{id}")]
        public IActionResult GetProductByIDStars(int id)
        {
            if (id <= 0) { return BadRequest(); }
            var product = _db.Products.Find(id);
            if (product == null) { return NotFound("No product found."); }

            var checkComment = _db.Comments.Where(p => p.ProductId == id);

            var stars = 0;
            if (checkComment != null && checkComment.Any())
            {
                stars = checkComment.Sum(p => p.Rating ?? 0) / checkComment.Count();
            }

            return Ok(new { product, stars });
        }
        /// /////////////////////////////////////////////////////////////////////

        [HttpGet("GetProductByCategoryID{id:int}")]
        public IActionResult GetProductByCategoryID(int id)
        {
            if (id <= 0) { return BadRequest(); }
            var products = _db.Products.Where(p => p.CategoryId == id).ToList();
            if (!products.Any()) { return NotFound(); }
            return Ok(products);
        }
        /// /////////////////////////////////////////////////////////////////////

        [HttpGet("GetBrandCount")]
        public ActionResult<IEnumerable<BrandCountDto>> GetBrandCount()
        {
            var brandCounts = _db.Products
                .Where(p => p.Brand != null)
                .GroupBy(p => p.Brand)
                .Select(group => new BrandCountDto
                {
                    BrandName = group.Key,
                    ProductCount = group.Count()
                })
                .ToList();

            return Ok(brandCounts);
        }
        /// /////////////////////////////////////////////////////////////////////

        [HttpGet("GetProductByBrand/{name}")]
        public IActionResult GetProductByBrand(string name)
        {
            if (name == null) { return BadRequest("no product under this Brand"); }
            var product = _db.Products.Where(p => p.Brand == name).ToList();
            if (!product.Any())
            {
                return NotFound();
            }
            return Ok(product);

        }
        /// /////////////////////////////////////////////////////////////////////

        [HttpGet("FilterByPrice")]
        public  IActionResult FilterByPrice(decimal minPrice, decimal maxPrice)
        {
            if (minPrice < 0 || maxPrice < 0 || minPrice > maxPrice)
            {
                return BadRequest("Invalid price range.");
            }
            var filteredProducts = _db.Products
                .Where(p => p.Price >= minPrice && p.Price <= maxPrice)
                .ToList();


            if (filteredProducts == null || filteredProducts.Count == 0)
            {
                return NotFound("No products found within the specified price range.");
            }

            return Ok(filteredProducts);
        }
        /// /////////////////////////////////////////////////////////////////////


        [HttpGet("FilterByPriceHighToLow")]
        public async Task<IActionResult> FilterByPriceHighToLow()
        {
            var order = _db.Products.OrderByDescending(p => p.Price);
            if (order == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(order);
            }
        }
        /// /////////////////////////////////////////////////////////////////////


        [HttpGet("FilterByPriceLowToHigh")]
        public async Task<IActionResult> FilterByPriceLowToHigh()
        {
            var order = _db.Products.OrderBy(p => p.Price);
            if (order == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(order);
            }
        }

        /// /////////////////////////////////////////////////////////////////////

        [HttpGet("FilterByName")]
        public async Task<IActionResult> FilterByName()
        {
            var order = _db.Products.OrderBy(p => p.Name);
            if (order == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(order);
            }
        }

    }
}
