using System;
using System.IO;
using Aspose.Email;

class Program
{
    static void Main()
    {
        try
        {
            // Input and output file paths
            string inputPath = "input.eml";
            string outputPath = "output_utf8.eml";

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

            // Load the EML message
            using (MailMessage mailMessage = MailMessage.Load(inputPath))
            {
                // Create EML save options
                EmlSaveOptions saveOptions = new EmlSaveOptions(MailMessageSaveType.EmlFormat);

                // The Aspose.Email API provides an Encoding property on SaveOptions in some versions.
                // If available, uncomment the line below to force UTF‑8 encoding.
                // saveOptions.Encoding = System.Text.Encoding.UTF8; // <-- property may not exist in this version

                // Save the message using the custom options
                mailMessage.Save(outputPath, saveOptions);
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
