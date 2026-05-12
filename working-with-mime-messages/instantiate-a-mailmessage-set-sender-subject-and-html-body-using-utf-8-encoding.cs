using System;
using System.Text;
using Aspose.Email;

class Program
{
    static void Main()
    {
        try
        {
            // Create a new MailMessage instance
            using (MailMessage message = new MailMessage())
            {
                // Set the sender address
                message.From = new MailAddress("sender@example.com");

                // Set the recipient address
                message.To.Add(new MailAddress("recipient@example.com"));

                // Set the subject
                message.Subject = "Sample Subject";

                // Set the HTML body and indicate that the body is HTML
                message.HtmlBody = "<html><body><h1>Hello, World!</h1></body></html>";
                message.IsBodyHtml = true;

                // Set the body encoding to UTF-8
                message.BodyEncoding = Encoding.UTF8;
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine(ex.Message);
        }
    }
}
