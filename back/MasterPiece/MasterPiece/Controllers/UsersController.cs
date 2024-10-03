using MasterPiece.DTO;
using MasterPiece.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using MimeKit;






namespace MasterPiece.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {

        private MyDbContext _db;
        private readonly ILogger<UsersController> _logger;
        private readonly TokenGeneratorDTO _tokenGenerator;
        public IWebHostEnvironment _webHostEnvironment;


        public UsersController(MyDbContext db, ILogger<UsersController> logger, TokenGeneratorDTO tokenGenerator, IWebHostEnvironment webHostEnvironment)
        {

            _db = db;
            _logger = logger;
            _tokenGenerator = tokenGenerator;
            _webHostEnvironment = webHostEnvironment;

        }


        ///////////////////////////////////////////////////////////

        [HttpPost]
        [Route("register")]
        // [ProducesResponseType(200, Type = typeof())]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(500)]
        [ProducesResponseType(404)]

        public IActionResult Register([FromForm] UserHashDTO model)
        {

            //if (_db.Users.Find(model.Email) != null)
            //{
            //    return BadRequest("The email you want to register with is alredy exest in uor database");
            //}


            if (model.ConfirmPassword != null)
                if (model.Password != model.ConfirmPassword)
                {
                    return BadRequest();
                }
          
            byte[] passwordHash, passwordSalt;
            PasswordHashDTO.CreatePasswordHash(model.Password, out passwordHash, out passwordSalt);
            User user = new User
            {
                Name = model.Name,
                Email = model.Email,
                Password = model.Password,
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt
            };

            _db.Users.Add(user);
            _db.SaveChanges();
            var token = GenerateJwtToken(user);
            return Ok(new { token });
            //For Demo Purpose we are returning the PasswordHash and PasswordSalt
            //  return Ok(user);
        }

        ////////////////////////////////////////////////////////////////////////////////


        [HttpPost]
        [Route("login")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(500)]
        [ProducesResponseType(404)]
        public IActionResult Login([FromForm] LoginDTO model)
        {
            var user = _db.Users.FirstOrDefault(x => x.Email == model.Email);

            if (user == null || !PasswordHashDTO.VerifyPasswordHash(model.Password, user.PasswordHash, user.PasswordSalt))
            {
                return Unauthorized("Invalid username or password.");
            }

            var token = GenerateJwtToken(user);
            return Ok(new { token });
        }

        private string GenerateJwtToken(User user)
        {
            var claims = new[]
            {
        new Claim(JwtRegisteredClaimNames.Sub, user.Email),
        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
        new Claim(ClaimTypes.Name, user.Name)
    };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("YourSuperSecureLongKeyForJWT12345"));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: "yourapp.com",
                audience: "yourapp.com",
                claims: claims,
                expires: DateTime.Now.AddMinutes(30),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        ///
        /// ///////////////////////////////////////////////


        [HttpGet("showUserInfoByID/{id}")]
        public IActionResult ShowUserProfile(int id)
        {
            if (id <= 0) { return BadRequest(); }

            var user = _db.Users.Find(id);


            if (user == null) { return NotFound("no user found"); }

            return Ok(user);
        }



        [HttpPost("ForgotPassword")]
        public async Task<IActionResult> ForgotPassword([FromBody] string email)
        {
            var user = _db.Users.FirstOrDefault(u => u.Email == email);
            if (user != null)
            {
                // Store user ID and OTP in session
                //HttpContext.Session.SetInt32("UserID", user.UserId);

                // Generate a random OTP
                Random rand = new Random();
                string otp = rand.Next(100000, 1000000).ToString();
                //HttpContext.Session.SetString("otp", otp);

                // Prepare email content
                string fromEmail = "techlearnhub.contact@gmail.com";
                string fromName = "Support Team";
                string subjectText = "Your OTP Code";
                string messageText = $@"
<html>
<body dir='rtl'>
    <h2>Hello {user.Name}</h2>
    <p><strong>Your OTP code is {otp}. This code is valid for a short period of time.</strong></p>
    <p>If you have any questions or need additional assistance, please feel free to contact our support team.</p>
    <p>Best wishes,<br>Support Team</p>
</body>
</html>";

                try
                {
                    // Send email using MailKit
                    var message = new MimeMessage();
                    message.From.Add(new MailboxAddress(fromName, fromEmail));
                    message.To.Add(new MailboxAddress("", user.Email));
                    message.Subject = subjectText;
                    message.Body = new TextPart("html") { Text = messageText };

                    using (var client = new MailKit.Net.Smtp.SmtpClient())
                    {
                        await client.ConnectAsync("smtp.gmail.com", 465, true);
                        await client.AuthenticateAsync("techlearnhub.contact@gmail.com", "lyrlogeztsxclank");
                        await client.SendAsync(message);
                        await client.DisconnectAsync(true);
                    }

                    return Ok(new { otp, user.UserId });
                }
                catch (Exception ex)
                {
                    return StatusCode(500, new { Message = "Failed to send email. Please try again later.", Error = ex.Message });
                }
            }
            else
            {
                return NotFound(new { Message = "Email not found." });
            }

        }
        [HttpPost("SetNewPassword")]
        public IActionResult SetNewPassword([FromBody] SetNewPasswordDto model)
        {


            var user = _db.Users.Find(model.UserId);
            byte[] passwordHash, passwordSalt;
            PasswordHashDTO.CreatePasswordHash(model.NewPassword, out passwordHash, out passwordSalt);
            user.Password = model.NewPassword;
            user.PasswordSalt = passwordSalt;
            user.PasswordHash = passwordHash;
            _db.Entry(user).State = EntityState.Modified;
            _db.SaveChanges();
            return Ok(new { Message = "Password changed successfully." });


        }

    }
}
