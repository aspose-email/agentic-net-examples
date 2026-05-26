using System;
using System.Text;
using Aspose.Email;

class Program
{
    static void Main()
    {
        try
        {
            // Create a new email message
            MailMessage message = new MailMessage();
            message.From = "sender@example.com";
            message.To.Add("recipient@example.com");
            message.Subject = "International Greeting";

            // Set the body text with international characters
            message.Body = "こんにちは、世界！"; // Japanese for "Hello, World!"

            // Set body encoding to UTF‑8 to support international characters
            message.BodyEncoding = Encoding.UTF8;
            // Also set the preferred text encoding for all text properties
            message.PreferredTextEncoding = Encoding.UTF8;

            // Display the message details
            Console.WriteLine("Subject: " + message.Subject);
            Console.WriteLine("Body Encoding: " + message.BodyEncoding.WebName);
            Console.WriteLine("Body: " + message.Body);
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine("Error: " + ex.Message);
        }
    }
}
