using Demo.DAL.Models;
using System.Net;
using System.Net.Mail;

namespace Demo.PL.Helpers
{
	public class EmailSettings
	{
		public static void SendEmail(Email email)
		{
			var client = new SmtpClient("smtp.gmail.com", 587);
			client.EnableSsl = true;

			client.Credentials = new NetworkCredential("akramebra71m@gmail.com", "qeaaszvfgumhpqwn");

			client.Send("akramebra71m@gmail.com", email.To, email.Subject, email.Body);
		}
	}
}
