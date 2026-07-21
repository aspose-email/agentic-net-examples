using System;
using Aspose.Email;

class Program
{
    static void Main()
    {
        try
        {
            // Author note: This sample loads an EML file and saves it as MSG preserving MIME structure and metadata.
            string inputPath = "input.eml";
            string outputPath = "output.msg";

            // Verify input file exists
            if (!System.IO.File.Exists(inputPath))
            {
                Console.Error.WriteLine($"Input file not found: {inputPath}");
                return;
            }

            // Ensure output directory exists
            string outputDir = System.IO.Path.GetDirectoryName(outputPath);
            if (!string.IsNullOrEmpty(outputDir) && !System.IO.Directory.Exists(outputDir))
            {
                System.IO.Directory.CreateDirectory(outputDir);
            }

            // Load the EML message with options to preserve TNEF attachments and embedded message format
            EmlLoadOptions emlLoadOptions = new EmlLoadOptions()
            {
                PreserveTnefAttachments = true,
                PreserveEmbeddedMessageFormat = true
            };

            using (MailMessage message = MailMessage.Load(inputPath, emlLoadOptions))
            {
                // Prepare MSG save options to preserve original dates
                MsgSaveOptions msgSaveOptions = new MsgSaveOptions(MailMessageSaveType.OutlookMessageFormatUnicode)
                {
                    PreserveOriginalDates = true
                };

                // Save the message as MSG
                message.Save(outputPath, msgSaveOptions);
            }

            Console.WriteLine($"Message successfully saved to: {outputPath}");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
