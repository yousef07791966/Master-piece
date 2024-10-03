using MasterPiece.DTO;
using MasterPiece.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MasterPiece.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommentsController : ControllerBase
    {

        private readonly MyDbContext _db;
        public CommentsController(MyDbContext db)
        {
            _db = db;
        }

        [HttpGet("GetComments/{productId:int}")]
        public async Task<IActionResult> GetComments(int productId)
        {
            var comments = await _db.Comments
                                           .Where(c => c.ProductId == productId && c.Status == "approved")
                                           .Include(c => c.User)
                                           .ToListAsync();

            var commentDTOs = comments.Select(c => new CommentDTO
            {
                CommentId = c.CommentId,
                Comment1 = c.Comment1,
                Rating = c.Rating ?? 0,
                Date = c.Date ?? DateOnly.FromDateTime(DateTime.Now),
                UserName = c.User?.Name ?? "Anonymous"
            }).ToList();

            return Ok(commentDTOs);
              
        }

        [HttpPost("AddComment")]
        public async Task<IActionResult> AddComment([FromBody] Comment comment)
        {
            comment.Status = "pending";
            comment.Date = DateOnly.FromDateTime(DateTime.Now);

            _db.Comments.Add(comment);
            await _db.SaveChangesAsync();

            return Ok("Comment submitted successfully. It will be visible once approved.");
        }

    }
}
