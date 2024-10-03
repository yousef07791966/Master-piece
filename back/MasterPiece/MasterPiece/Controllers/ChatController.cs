using MasterPiece.DTO;
using MasterPiece.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MasterPiece.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChatController : ControllerBase
    {
        private readonly MyDbContext _db;

        public ChatController(MyDbContext db)
        {
            _db = db;
        }

        [HttpGet("AllUsers")]
        public IActionResult AllUsers()
        {
            var users = _db.Chats.ToList();

            return Ok(users);
        }

        [HttpGet("showMessage/{userId}")]
        public IActionResult ShowMessage(int userId)
        {
            // البحث عن المحادثة المرتبطة بهذا المستخدم
            var userChat = _db.Chats.FirstOrDefault(c => c.UserId == userId);

            if (userChat == null)
            {
                return NotFound("No chat found for this user.");
            }

            // استرداد الرسائل الخاصة بالمحادثة
            var messages = _db.ChatMessages.Where(m => m.Id == userChat.ChatId).ToList();

            return Ok(messages);
        }

        [HttpPost("replayMessage/{userId}")]
        public IActionResult ReplayMessage([FromForm] ChatResponseDTO chat, int userId)
        {
            // جلب المحادثة الخاصة بالمستخدم
            var userChat = _db.Chats.FirstOrDefault(c => c.UserId == userId);

            // إذا لم تكن هناك محادثة، قم بإنشاء محادثة جديدة
            if (userChat == null)
            {
                userChat = new Chat
                {
                    UserId = userId,
                    // أي خصائص أخرى تحتاجها عند إنشاء المحادثة
                };

                _db.Chats.Add(userChat);
                _db.SaveChanges(); // حفظ المحادثة الجديدة للحصول على ChatId
            }

            // الآن أضف الرسالة الجديدة
            var newMessage = new ChatMessage
            {
                Id = userChat.ChatId, // الحصول على ChatId الخاص بالمحادثة
                MessageContent= chat.Cmessages,
            };

            _db.ChatMessages.Add(newMessage);
            _db.SaveChanges();

            return Ok(newMessage);
        }
    }
}
