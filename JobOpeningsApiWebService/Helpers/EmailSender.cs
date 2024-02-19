using System.Net.Mail;
using System.Net;

namespace JobOpeningsApiWebService.Helpers
{
	public class EmailSender
	{
		private readonly IConfiguration _config;

		public EmailSender(IConfiguration configuration)
		{
			_config = configuration;
		}
		public void SendEmail(string recipientEmail, string subject, string body)
		{
			SmtpClient smtpClient = new(_config.GetSection("EmailSettings")["emailAccountClient"]?.ToString())
			{
				Port = 587,
				EnableSsl = true,
				Credentials = new NetworkCredential(_config.GetSection("EmailSettings")["emailAccountID"]?.ToString(), _config.GetSection("EmailSettings")["emailAccountPassword"]?.ToString())
			};

			MailMessage mailMessage = new()
			{
				From = new MailAddress(_config.GetSection("EmailSettings")["emailAccountID"]?.ToString())
			};
			mailMessage.To.Add(recipientEmail);
			mailMessage.Subject = subject;
			mailMessage.Body = body;

			try
			{
				smtpClient.Send(mailMessage);
			}
			catch (Exception ex)
			{
				Console.WriteLine("Failed to send email: " + ex.Message);
			}
		}
	}
}
