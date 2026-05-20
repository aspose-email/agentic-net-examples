using System;
using Aspose.Email;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            // Create a new email message
            MailMessage message = new MailMessage();
            message.From = new MailAddress("sender@example.com");
            message.To.Add(new MailAddress("recipient@example.com"));
            message.Subject = "Newsletter Subscription";
            message.Body = "Hello, this is our monthly newsletter.";

            // Add the List-Unsubscribe header
            string unsubscribeHeader = "<mailto:unsubscribe@example.com>, <http://example.com/unsubscribe>";
            message.Headers.Add("List-Unsubscribe", unsubscribeHeader);

            // Output the header to verify
            Console.WriteLine("List-Unsubscribe header added:");
            Console.WriteLine(message.Headers["List-Unsubscribe"]);
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine(ex.Message);
        }
    }
}
