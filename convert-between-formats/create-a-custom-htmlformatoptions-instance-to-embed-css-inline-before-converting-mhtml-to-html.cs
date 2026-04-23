using System;
using System.IO;
using Aspose.Email;

class Program
{
    static void Main()
    {
        try
        {
            string inputPath = "input.mht";
            string outputPath = "output.html";

            // Guard input file existence
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

                Console.Error.WriteLine($"Input file '{inputPath}' does not exist.");
                return;
            }

            // Load the MHTML message
            MailMessage message;
            try
            {
                message = MailMessage.Load(inputPath, new MhtmlLoadOptions());
            }
            catch (Exception loadEx)
            {
                Console.Error.WriteLine($"Failed to load MHTML file: {loadEx.Message}");
                return;
            }

            using (message)
            {
                // Create HtmlSaveOptions with custom CSS and embed it inline
                HtmlSaveOptions htmlOptions = new HtmlSaveOptions();
                htmlOptions.CssStyles = "body { font-family: Arial, sans-serif; margin: 0; padding: 0; }";
                // Ensure no additional format flags are set (default is None)
                htmlOptions.HtmlFormatOptions = HtmlFormatOptions.None;

                // Save as HTML
                try
                {
                    message.Save(outputPath, htmlOptions);
                    Console.WriteLine($"HTML file saved to '{outputPath}'.");
                }
                catch (Exception saveEx)
                {
                    Console.Error.WriteLine($"Failed to save HTML file: {saveEx.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
