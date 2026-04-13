using System;
using System.IO;
using Aspose.Email;

class Program
{
    static void Main()
    {
        try
        {
            string inputPath = "sample.msg";
            string outputPath = "output.eml";

            // Ensure the input MSG file exists; create a minimal placeholder if it does not.
            if (!File.Exists(inputPath))
            {
                using (MailMessage placeholder = new MailMessage("from@example.com", "to@example.com", "Placeholder", "This is a placeholder message."))
                {
                    MsgSaveOptions placeholderSaveOptions = new MsgSaveOptions(MailMessageSaveType.OutlookMessageFormat);
                    placeholder.Save(inputPath, placeholderSaveOptions);
                }
                Console.WriteLine($"Placeholder MSG created at '{inputPath}'.");
            }

            // Ensure the output directory exists.
            string outputDir = Path.GetDirectoryName(outputPath);
            if (!string.IsNullOrEmpty(outputDir) && !Directory.Exists(outputDir))
            {
                Directory.CreateDirectory(outputDir);
            }

            // Load the MSG file with PreserveEmbeddedMessageFormat enabled.
            MsgLoadOptions msgLoadOptions = new MsgLoadOptions
            {
                PreserveEmbeddedMessageFormat = true
            };

            using (MailMessage message = MailMessage.Load(inputPath, msgLoadOptions))
            {
                // Save the message as EML with PreserveEmbeddedMessageFormat enabled.
                EmlSaveOptions emlSaveOptions = new EmlSaveOptions(MailMessageSaveType.EmlFormat)
                {
                    PreserveEmbeddedMessageFormat = true
                };
                message.Save(outputPath, emlSaveOptions);
            }

            Console.WriteLine($"MSG converted to EML at '{outputPath}'.");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
