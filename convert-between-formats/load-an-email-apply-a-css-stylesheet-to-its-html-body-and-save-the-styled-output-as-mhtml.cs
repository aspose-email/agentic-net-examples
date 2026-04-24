using System;
using System.IO;
using Aspose.Email;

class Program
{
    static void Main()
    {
        try
        {
            string inputPath = "input.eml";
            string outputPath = "styled_output.mht";

            // Verify input file exists
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

                Console.Error.WriteLine($"Input file not found: {inputPath}");
                return;
            }

            // Load the email message
            MailMessage mailMessage;
            try
            {
                mailMessage = MailMessage.Load(inputPath);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to load email: {ex.Message}");
                return;
            }

            // Ensure the message is disposed after use
            using (mailMessage)
            {
                // Prepare MHTML save options with CSS styling
                MhtSaveOptions saveOptions = new MhtSaveOptions();
                saveOptions.CssStyles = "body { font-family: Arial, sans-serif; color: #333333; }";

                // Save the styled message as MHTML
                try
                {
                    mailMessage.Save(outputPath, saveOptions);
                    Console.WriteLine($"Message saved with CSS to: {outputPath}");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to save MHTML: {ex.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
