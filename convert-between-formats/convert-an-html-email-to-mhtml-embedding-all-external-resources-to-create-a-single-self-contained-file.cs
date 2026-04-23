using System;
using System.IO;
using Aspose.Email;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            // Define input and output file paths
            string inputPath = "input.eml";
            string outputPath = "output.mht";

            // Ensure the input file exists; if not, create a minimal placeholder EML
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
                    // Create a simple MailMessage with HTML body
                    MailMessage placeholder = new MailMessage();
                    placeholder.From = "sender@example.com";
                    placeholder.To = "recipient@example.com";
                    placeholder.Subject = "Placeholder Email";
                    placeholder.HtmlBody = "<html><body><h1>Placeholder</h1><p>This is a placeholder email.</p></body></html>";

                    // Save the placeholder as EML
                    placeholder.Save(inputPath);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create placeholder EML: {ex.Message}");
                    return;
                }
            }

            // Load the email message
            MailMessage emailMessage;
            try
            {
                emailMessage = MailMessage.Load(inputPath);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to load email from '{inputPath}': {ex.Message}");
                return;
            }

            // Prepare MHT save options to embed all resources
            MhtSaveOptions mhtOptions = new MhtSaveOptions
            {
                MailMessageSaveType = MailMessageSaveType.MHtmlFormat,
                // Ensure resources are embedded (default behavior)
                ExtractHTMLBodyResourcesAsAttachments = false
            };

            // Ensure the output directory exists
            try
            {
                string outputDir = Path.GetDirectoryName(Path.GetFullPath(outputPath));
                if (!string.IsNullOrEmpty(outputDir) && !Directory.Exists(outputDir))
                {
                    Directory.CreateDirectory(outputDir);
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to prepare output directory: {ex.Message}");
                return;
            }

            // Save the message as MHTML with embedded resources
            try
            {
                emailMessage.Save(outputPath, mhtOptions);
                Console.WriteLine($"Email successfully converted to MHTML: {outputPath}");
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to save MHTML file: {ex.Message}");
            }
            finally
            {
                // Dispose the loaded message
                if (emailMessage != null)
                {
                    emailMessage.Dispose();
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
