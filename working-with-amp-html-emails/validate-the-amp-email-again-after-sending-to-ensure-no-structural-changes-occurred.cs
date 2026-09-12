using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Amp;
using Aspose.Email.Tools.Verifications;

class Program
{
    static void Main()
    {
        try
        {
            string outputPath = "amp_message.eml";

            // Ensure the directory exists
            try
            {
                string directory = Path.GetDirectoryName(Path.GetFullPath(outputPath));
                if (!Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to prepare output directory: {ex.Message}");
                return;
            }

            // Create and configure the AMP message
            try
            {
                using (AmpMessage ampMessage = new AmpMessage())
                {
                    ampMessage.From = "sender@example.com";
                    ampMessage.To.Add("recipient@example.com");
                    ampMessage.Subject = "Test AMP Email";
                    ampMessage.AmpHtmlBody = "<!doctype html><html amp4email><head><meta charset=\"utf-8\"><script async src=\"https://cdn.ampproject.org/v0.js\"></script></head><body><h1>Hello AMP</h1></body></html>";

                    // Save the message to a file
                    ampMessage.Save(outputPath);
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error creating or saving AMP message: {ex.Message}");
                return;
            }

            // Validate the saved AMP message
            try
            {
                MessageValidationResult validationResult = MessageValidator.Validate(outputPath);
                Console.WriteLine($"Validation completed. Errors count: {validationResult.Errors.Count}");
                foreach (var error in validationResult.Errors)
                {
                    Console.WriteLine($"- {error}");
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Validation failed: {ex.Message}");
                return;
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
