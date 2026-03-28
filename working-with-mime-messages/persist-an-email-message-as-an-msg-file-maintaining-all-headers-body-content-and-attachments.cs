using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Tools.Verifications;

class Program
{
    static void Main()
    {
        try
        {
            string inputPath = "sample.eml";
            string outputPath = "sample.msg";

            // Ensure the output directory exists
            string outputDir = Path.GetDirectoryName(outputPath);
            if (!string.IsNullOrEmpty(outputDir) && !Directory.Exists(outputDir))
            {
                Directory.CreateDirectory(outputDir);
            }

            // Guarded file I/O for the input file
            if (!File.Exists(inputPath))
            {
                // Create a minimal placeholder email if the source file is missing
                using (MailMessage placeholder = new MailMessage("sender@example.com", "receiver@example.com", "Placeholder Subject", "Placeholder body."))
                {
                    placeholder.Save(inputPath);
                }
            }

            // Load the email message and save it as MSG preserving all data
            using (MailMessage message = MailMessage.Load(inputPath))
            {
                MsgSaveOptions saveOptions = new MsgSaveOptions(MailMessageSaveType.OutlookMessageFormat);
                // Preserve original dates (default is true, set explicitly for clarity)
                saveOptions.PreserveOriginalDates = true;

                message.Save(outputPath, saveOptions);
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
