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
                // Set sender and recipient
                message.From = new MailAddress("sender@example.com");
                message.To.Add(new MailAddress("recipient@example.com"));
                message.Subject = "Sample Email with HTML and Plain Text";

                // HTML body
                string htmlContent = "<html><body><h1>Hello World</h1><p>This is an <b>HTML</b> email.</p></body></html>";
                message.HtmlBody = htmlContent;
                message.IsBodyHtml = true;

                // Plain‑text body mirroring the HTML content
                string plainTextContent = "Hello World\nThis is an HTML email.";
                message.Body = plainTextContent;
                message.BodyEncoding = System.Text.Encoding.UTF8;

                // Output for demonstration
                Console.WriteLine("HTML Body:");
                Console.WriteLine(message.HtmlBody);
                Console.WriteLine("\nPlain Text Body:");
                Console.WriteLine(message.Body);
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine(ex.Message);
        }
    }
}
