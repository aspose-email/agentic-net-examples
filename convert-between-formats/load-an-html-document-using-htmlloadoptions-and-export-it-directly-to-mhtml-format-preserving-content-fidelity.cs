using System;
using System.IO;
using Aspose.Email;

class Program
{
    static void Main()
    {
        try
        {
            // Define input and output paths
            string inputPath = "sample.html";
            string outputPath = "sample.mhtml";

            // Ensure the input HTML file exists; create a minimal placeholder if missing
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

                try
                {
                    File.WriteAllText(inputPath, "<html><body><p>Placeholder content.</p></body></html>");
                    Console.WriteLine($"Created placeholder HTML file at '{inputPath}'.");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create placeholder HTML file: {ex.Message}");
                    return;
                }
            }

            // Load the HTML document with load options
            HtmlLoadOptions htmlLoadOptions = new HtmlLoadOptions();
            MailMessage mailMessage;
            try
            {
                mailMessage = MailMessage.Load(inputPath, htmlLoadOptions);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to load HTML file: {ex.Message}");
                return;
            }

            // Ensure the output directory exists
            try
            {
                string outputDir = Path.GetDirectoryName(Path.GetFullPath(outputPath));
                if (!Directory.Exists(outputDir))
                {
                    Directory.CreateDirectory(outputDir);
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to prepare output directory: {ex.Message}");
                return;
            }

            // Save the message as MHTML using default options
            try
            {
                mailMessage.Save(outputPath, SaveOptions.DefaultMhtml);
                Console.WriteLine($"HTML successfully converted to MHTML at '{outputPath}'.");
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to save MHTML file: {ex.Message}");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
