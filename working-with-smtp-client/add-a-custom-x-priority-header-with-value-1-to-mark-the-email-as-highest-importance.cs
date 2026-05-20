using System;
using Aspose.Email;

class Program
{
    static void Main()
    {
        try
        {
            // Create a new mail message
            using (MailMessage message = new MailMessage())
            {
                // Set basic properties
                message.From = "sender@example.com";
                message.To.Add("recipient@example.com");
                message.Subject = "Test Email with X-Priority Header";
                message.Body = "This email includes a custom X-Priority header set to 1.";

                // Add custom X-Priority header (value 1 = highest importance)
                message.Headers.Add("X-Priority", "1");

                // For demonstration, output the header value to console
                Console.WriteLine("Added header: X-Priority = " + message.Headers["X-Priority"]);
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine("Error: " + ex.Message);
        }
    }
}
