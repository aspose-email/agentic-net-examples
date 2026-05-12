using System;
using Aspose.Email;

class Program
{
    static void Main()
    {
        try
        {
            using (MailMessage message = new MailMessage())
            {
                message.From = "sender@example.com";
                message.To.Add("recipient@example.com");
                message.Subject = "Test bulk email";
                message.Body = "This is a test email.";

                // Add Precedence header to indicate bulk mailing
                message.Headers.Add("Precedence", "bulk");

                // Verify that the header was added
                Console.WriteLine("Precedence header set to: " + message.Headers["Precedence"]);
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine(ex.Message);
        }
    }
}
