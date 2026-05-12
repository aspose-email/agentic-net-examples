using System;
using Aspose.Email;

class Program
{
    static void Main()
    {
        try
        {
            // Create a new mail message
            using (MailMessage message = new MailMessage(
                "sender@example.com",
                "recipient@example.com",
                "Test Subject",
                "This is the body."))
            {
                // Insert a custom X-Priority header to indicate high importance
                message.Headers.Add("X-Priority", "1 (Highest)");

                // Verify that the header was added
                Console.WriteLine("Added X-Priority header: " + message.Headers["X-Priority"]);
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine("Error: " + ex.Message);
        }
    }
}
