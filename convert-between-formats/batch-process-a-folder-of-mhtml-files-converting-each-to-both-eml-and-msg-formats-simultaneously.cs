using System;
using System.IO;
using Aspose.Email;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            // Define input and output directories
            string inputFolder = "MhtmlFolder";
            string outputFolder = "OutputFolder";

            // Verify input folder exists
            if (!Directory.Exists(inputFolder))
            {
                Console.Error.WriteLine($"Input folder '{inputFolder}' does not exist.");
                return;
            }

            // Ensure output folder exists
            if (!Directory.Exists(outputFolder))
            {
                Directory.CreateDirectory(outputFolder);
            }

            // Gather all .mht and .mhtml files
            string[] allFiles = Directory.GetFiles(inputFolder);
            foreach (string filePath in allFiles)
            {
                string extension = Path.GetExtension(filePath).ToLowerInvariant();
                if (extension != ".mht" && extension != ".mhtml")
                {
                    continue; // Skip non‑MHTML files
                }

                try
                {
                    // Load the MHTML file into a MailMessage
                    using (MailMessage message = MailMessage.Load(filePath))
                    {
                        // Prepare save options for EML
                        EmlSaveOptions emlSaveOptions = new EmlSaveOptions(MailMessageSaveType.EmlFormat)
                        {
                            PreserveEmbeddedMessageFormat = true
                        };

                        // Prepare save options for MSG
                        MsgSaveOptions msgSaveOptions = new MsgSaveOptions(MailMessageSaveType.OutlookMessageFormatUnicode)
                        {
                            PreserveOriginalDates = true
                        };

                        // Build output file names
                        string baseName = Path.GetFileNameWithoutExtension(filePath);
                        string emlPath = Path.Combine(outputFolder, baseName + ".eml");
                        string msgPath = Path.Combine(outputFolder, baseName + ".msg");

                        // Save as EML
                        message.Save(emlPath, emlSaveOptions);

                        // Save as MSG
                        message.Save(msgPath, msgSaveOptions);
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error processing '{filePath}': {ex.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
