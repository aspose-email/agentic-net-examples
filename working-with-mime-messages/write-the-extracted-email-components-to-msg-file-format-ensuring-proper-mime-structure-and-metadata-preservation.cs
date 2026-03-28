using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Mapi;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            // Define input EML file and output MSG file paths
            string inputPath = "Message.eml";
            string outputPath = "Message.msg";

            // Verify that the input file exists
            if (!File.Exists(inputPath))
            {
                Console.Error.WriteLine($"Input file not found: {inputPath}");
                return;
            }

            // Ensure the output directory exists
            string outputDirectory = Path.GetDirectoryName(outputPath);
            if (!string.IsNullOrEmpty(outputDirectory) && !Directory.Exists(outputDirectory))
            {
                Directory.CreateDirectory(outputDirectory);
            }

            // Load the EML message
            using (MailMessage mailMessage = MailMessage.Load(inputPath))
            {
                // Convert to MAPI message to preserve MIME structure and metadata
                using (MapiMessage mapiMessage = MapiMessage.FromMailMessage(mailMessage))
                {
                    // Configure MSG save options (Unicode format with original dates preserved)
                    MsgSaveOptions saveOptions = new MsgSaveOptions(MailMessageSaveType.OutlookMessageFormatUnicode)
                    {
                        PreserveOriginalDates = true
                    };

                    // Save the message as MSG
                    mapiMessage.Save(outputPath, saveOptions);
                }
            }

            Console.WriteLine($"Message successfully saved to: {outputPath}");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
