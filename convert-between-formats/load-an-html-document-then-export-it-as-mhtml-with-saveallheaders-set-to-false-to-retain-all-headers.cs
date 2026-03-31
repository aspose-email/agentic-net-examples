using System;
using System.IO;
using Aspose.Email;

class Program
{
    static void Main()
    {
        try
        {
            string inputPath = "input.html";
            string outputPath = "output.mht";

            // Ensure the input HTML file exists; create a minimal placeholder if missing.
            if (!File.Exists(inputPath))
            {
                try
                {
                    File.WriteAllText(inputPath, "<html><body><p>Placeholder</p></body></html>");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create placeholder input file: {ex.Message}");
                    return;
                }
            }

            // Read the HTML content.
            string htmlContent;
            try
            {
                htmlContent = File.ReadAllText(inputPath);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to read input file: {ex.Message}");
                return;
            }

            // Create a MailMessage with the HTML body.
            using (MailMessage mail = new MailMessage())
            {
                mail.HtmlBody = htmlContent;

                // Configure MHTML save options with SaveAllHeaders set to false.
                MhtSaveOptions options = new MhtSaveOptions
                {
                    SaveAllHeaders = false
                };

                // Save the message as MHTML.
                try
                {
                    mail.Save(outputPath, options);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to save MHTML: {ex.Message}");
                    return;
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
