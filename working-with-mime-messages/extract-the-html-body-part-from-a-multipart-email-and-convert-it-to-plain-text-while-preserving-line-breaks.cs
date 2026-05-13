using System;
using System.IO;
using Aspose.Email;

class Program
{
    static void Main()
    {
        try
        {
            const string emlPath = "sample.eml";

            // Ensure the input file exists; create a minimal placeholder if missing.
            if (!File.Exists(emlPath))
            {
                try
                {
                    using (MailMessage placeholder = new MailMessage(
                        "sender@example.com",
                        "recipient@example.com",
                        "Placeholder Subject",
                        "Placeholder body."))
                    {
                        placeholder.Save(emlPath, SaveOptions.DefaultEml);
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error creating placeholder message: {ex.Message}");
                    return;
                }

                using (MailMessage placeholder = new MailMessage("sender@example.com", "receiver@example.com", "Sample", string.Empty))
                {
                    placeholder.IsBodyHtml = true;
                    placeholder.HtmlBody = "<html><body>Hello<br/>World</body></html>";
                    placeholder.Save(emlPath);
                }
            }

            // Load the email and extract the HTML body as plain text.
            using (MailMessage message = MailMessage.Load(emlPath))
            {
                string plainTextBody = message.GetHtmlBodyText(true);
                Console.WriteLine("Plain text body:");
                Console.WriteLine(plainTextBody);
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine(ex.Message);
        }
    }
}
