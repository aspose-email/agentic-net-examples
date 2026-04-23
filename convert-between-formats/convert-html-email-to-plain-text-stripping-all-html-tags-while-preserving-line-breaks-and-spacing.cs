using System;
using System.IO;
using Aspose.Email;

class Program
{
    static void Main()
    {
        try
        {
            string inputPath = "HtmlEmail.eml";

            if (!File.Exists(inputPath))
            {
                try
                {
                    using (MailMessage placeholder = new MailMessage(
                        "sender@example.com",
                        "recipient@example.com",
                        "Placeholder Subject",
                        "Placeholder body."))
                    {
                        placeholder.Save(inputPath, SaveOptions.DefaultEml);
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error creating placeholder message: {ex.Message}");
                    return;
                }

                Console.Error.WriteLine($"Input file '{inputPath}' not found.");
                return;
            }

            using (MailMessage message = MailMessage.Load(inputPath))
            {
                // Convert the HTML body to plain text, omitting URLs.
                string plainText = message.GetHtmlBodyText(false);
                Console.WriteLine("Plain text body:");
                Console.WriteLine(plainText);
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
