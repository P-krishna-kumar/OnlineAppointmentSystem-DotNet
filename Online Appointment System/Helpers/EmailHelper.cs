using System.Net;
using System.Net.Mail;
using Microsoft.Extensions.Configuration;

namespace OnlineAppointmentSystem.Helpers
{
    public class EmailHelper
    {
        private readonly IConfiguration _config;

        public EmailHelper(IConfiguration config)
        {
            _config = config;
        }

        public bool SendEmail(string toEmail, string subject, string body)
        {
            try
            {
                var host = _config["EmailSettings:Host"];
                var port = Convert.ToInt32(_config["EmailSettings:Port"]);
                var user = _config["EmailSettings:User"];
                var pass = _config["EmailSettings:Password"];

                MailMessage mm = new MailMessage();
                mm.From = new MailAddress(user);
                mm.To.Add(toEmail);
                mm.Subject = subject;
                mm.Body = body;
                mm.IsBodyHtml = true;

                SmtpClient smtp = new SmtpClient(host, port);
                smtp.EnableSsl = true;
                smtp.Credentials = new NetworkCredential(user, pass);
                smtp.Send(mm);

                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
