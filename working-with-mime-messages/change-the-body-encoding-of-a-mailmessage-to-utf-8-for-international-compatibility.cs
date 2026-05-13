using System;
using System.Text;
using Aspose.Email;

class Program
{
    static void Main()
    {
        try
        {
            // Create a new mail message with sample content
            using (MailMessage message = new MailMessage(
                "sender@example.com",
                "recipient@example.com",
                "Sample Subject",
                "Sample body with international characters: Привет мир"))
            {
                // Change the body encoding to UTF-8 for international compatibility
                message.BodyEncoding = Encoding.UTF8;
                // Also set the preferred text encoding to UTF-8
                message.PreferredTextEncoding = Encoding.UTF8;

                // Output the current encoding settings
                Console.WriteLine("BodyEncoding: " + message.BodyEncoding.WebName);
                Console.WriteLine("PreferredTextEncoding: " + message.PreferredTextEncoding.WebName);
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine(ex.Message);
        }
    }
}
