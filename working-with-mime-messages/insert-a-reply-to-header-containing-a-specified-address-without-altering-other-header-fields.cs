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
                // Set basic fields
                message.From = new MailAddress("sender@example.com");
                message.To.Add(new MailAddress("recipient@example.com"));
                message.Subject = "Sample Email";
                message.Body = "This email demonstrates adding a Reply-To header.";

                // Insert Reply-To header without affecting other headers
                message.Headers.Add(HeaderType.ReplyTo, "replyto@example.com");

                // Verify the header was added
                Console.WriteLine("Reply-To header set to: " + message.Headers[HeaderType.ReplyTo]);
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine(ex.Message);
        }
    }
}
