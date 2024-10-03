using MasterPiece.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MasterPiece.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {

        private readonly MyDbContext _db;
        public CategoryController(MyDbContext db)
        {
            _db = db;
        }

        [HttpGet("GetAllCategory")]
        public IActionResult GetAllCategory()
        {
            var AllCategory = _db.Categories.ToList();

            if (!AllCategory.Any())
            {
                return NotFound("No categories found.");
            }

            return Ok(AllCategory);
        }
    }
}
