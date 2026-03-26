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

            // Verify input file exists
            if (!File.Exists(inputPath))
            {
                Console.Error.WriteLine($"Error: File not found – {inputPath}");
                return;
            }

            // Load HTML content
            string htmlContent;
            try
            {
                htmlContent = File.ReadAllText(inputPath);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error reading file: {ex.Message}");
                return;
            }

            // Create a MailMessage with the HTML body
            using (MailMessage message = new MailMessage())
            {
                message.From = new MailAddress("sender@example.com");
                message.To.Add(new MailAddress("recipient@example.com"));
                message.Subject = "Converted HTML to MHTML";
                message.IsBodyHtml = true;
                message.HtmlBody = htmlContent;

                // Configure MHTML save options
                MhtSaveOptions saveOptions = new MhtSaveOptions();
                saveOptions.SaveAllHeaders = false; // retain all headers as per requirement

                // Save as MHTML
                try
                {
                    message.Save(outputPath, saveOptions);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error saving MHTML: {ex.Message}");
                    return;
                }
            }

            Console.WriteLine($"MHTML file saved to: {outputPath}");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}