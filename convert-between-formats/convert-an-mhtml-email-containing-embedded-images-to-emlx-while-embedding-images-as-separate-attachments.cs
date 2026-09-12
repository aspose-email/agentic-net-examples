using System;
using System.IO;
using Aspose.Email;

class Program
{
    static void Main()
    {
        try
        {
            // Author note: This example converts an MHTML (.mht) email to EMLX format,
            // ensuring that embedded images are saved as separate attachments.

            const string inputFile = "message.mht";

            // Verify input file exists
            if (!File.Exists(inputFile))
            {
                try
                {
                    using (MailMessage placeholder = new MailMessage(
                        "sender@example.com",
                        "recipient@example.com",
                        "Placeholder Subject",
                        "Placeholder body."))
                    {
                        placeholder.Save(inputFile, SaveOptions.DefaultEml);
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error creating placeholder message: {ex.Message}");
                    return;
                }

                Console.Error.WriteLine($"Input file '{inputFile}' not found.");
                return;
            }

            // Load the MHTML message
            using (MailMessage mailMessage = MailMessage.Load(inputFile))
            {
                // Prepare EMLX save options
                var emlSaveOptions = new EmlSaveOptions(MailMessageSaveType.EmlxFormat)
                {
                    // Preserve the original format of any embedded messages (if present)
                    PreserveEmbeddedMessageFormat = true
                };

                // Determine output path
                string outputFile = Path.ChangeExtension(inputFile, ".emlx");

                // Ensure the output directory exists
                string outputDir = Path.GetDirectoryName(outputFile);
                if (!string.IsNullOrEmpty(outputDir) && !Directory.Exists(outputDir))
                {
                    Directory.CreateDirectory(outputDir);
                }

                // Save as EMLX; embedded images will be stored as separate attachments
                mailMessage.Save(outputFile, emlSaveOptions);

                Console.WriteLine($"Conversion successful. EMLX saved to: {outputFile}");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
