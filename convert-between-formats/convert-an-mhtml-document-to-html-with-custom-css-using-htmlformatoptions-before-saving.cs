using System;
using System.IO;
using Aspose.Email;

class Program
{
    static void Main(string[] args)
    {
        // Author note: Simple console app converting MHTML to HTML with custom CSS.
        try
        {
            string inputPath = "input.mhtml";
            string outputPath = "output.html";

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
                HtmlSaveOptions saveOptions = new HtmlSaveOptions();
                saveOptions.CssStyles = "body { font-family: Arial, sans-serif; color: #333; }";

                message.Save(outputPath, saveOptions);
            }

            Console.WriteLine($"Conversion completed. HTML saved to '{outputPath}'.");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
