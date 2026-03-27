using System;
using Aspose.Email;
using Aspose.Email.Clients;
using Aspose.Email.Clients.Smtp;

class Program
{
    static void Main()
    {
        try
        {
            using (MailMessage message = new MailMessage("mudassir@example.com", "test@example.com", "test email", "hello"))
            {
                using (SmtpClient client = new SmtpClient("smtp.example.com", 587, "username", "password"))
                {
                    client.SecurityOptions = SecurityOptions.Auto;
                    client.Send(message);
                    Console.WriteLine("Email sent successfully.");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine("Error: " + ex.Message);
        }
    }
}