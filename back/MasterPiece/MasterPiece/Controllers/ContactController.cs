using MasterPiece.DTO;
using MasterPiece.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MasterPiece.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContactController : ControllerBase
    {

        private readonly MyDbContext _db;

        public ContactController(MyDbContext db)
        {
            _db = db;
        }

        // GET: api/Contact
        [HttpPost]
        public IActionResult Contact([FromForm] ContactRequestDTO DTO)
        {
            var contact = new Contact
            {
                Name = DTO.Name,
                Email = DTO.Email,
                Message = DTO.Message,
                Subject = DTO.Subject,
                SentDate = DateOnly.FromDateTime(DateTime.Now)
            };
            _db.Contacts.Add(contact);
            _db.SaveChanges();


            return Ok(contact);
        }
        [HttpGet("contact")]
        public IActionResult GetContact()
        {
            var contact = _db.Contacts.ToList();
            return Ok(contact);
        }


    }
}
