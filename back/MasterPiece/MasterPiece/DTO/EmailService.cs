using MimeKit;
using System.Net.Mail;
using MasterPiece;
using SmtpClient = MailKit.Net.Smtp.SmtpClient;





namespace MasterPiece.DTO
{
    public class EmailService : IEmailService
    {
        public void SendEmail(string to, string subject, string body)
        {
            // إعداد رسالة البريد الإلكتروني
            var emailMessage = new MimeMessage();
            emailMessage.From.Add(new MailboxAddress("Your Name", "teamorange077@gmail.com"));
            emailMessage.To.Add(new MailboxAddress("", to));
            emailMessage.Subject = subject;

            // إعداد محتوى الرسالة
            var bodyBuilder = new BodyBuilder { HtmlBody = body };
            emailMessage.Body = bodyBuilder.ToMessageBody();

            // استخدام SMTP لإرسال البريد
            using (var client = new SmtpClient())
            {
                try
                {
                    // الاتصال بخادم SMTP (مثل Gmail)
                    client.Connect("smtp.gmail.com", 587, MailKit.Security.SecureSocketOptions.StartTls);

                    // المصادقة باستخدام اسم المستخدم وكلمة المرور (يجب أن يكون لديك حساب Gmail)
                    client.Authenticate("teamorange077@gmail.com", "thxc lkmk weir bxbq");

                    // إرسال البريد
                    client.Send(emailMessage);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error sending email: {ex.Message}");
                }
                finally
                {
                    // التأكد من قطع الاتصال بالخادم
                    client.Disconnect(true);
                }
            }
        }


    }
}
