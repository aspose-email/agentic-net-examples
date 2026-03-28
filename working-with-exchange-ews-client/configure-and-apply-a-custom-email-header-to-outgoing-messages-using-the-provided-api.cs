using System;
using Aspose.Email;
using Aspose.Email.Clients.Smtp;
using Aspose.Email.Clients;

class Program
{
    static void Main()
    {
        try
        {
            // Create a mail message
            using (MailMessage message = new MailMessage("sender@example.com", "recipient@example.com", "Test Subject", "This is the body."))
            {
                // Add a custom header
                message.Headers.Add("X-Custom-Header", "MyValue");

                // Initialize SMTP client
                using (SmtpClient client = new SmtpClient("smtp.example.com", 587, "username", "password"))
                {
                    client.SecurityOptions = SecurityOptions.Auto;
                    // Send the message
                    client.Send(message);
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine(ex.Message);
        }
    }
}
