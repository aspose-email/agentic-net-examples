using System;
using System.IO;
using Aspose.Email;

class Program
{
    static void Main()
    {
        try
        {
            // Define output file path
            string outputPath = "output.msg";

            // Ensure the output directory exists
            string outputDir = Path.GetDirectoryName(Path.GetFullPath(outputPath));
            if (!Directory.Exists(outputDir))
            {
                Directory.CreateDirectory(outputDir);
            }

            // Create a simple mail message
            using (MailMessage message = new MailMessage())
            {
                message.From = new MailAddress("sender@example.com");
                message.To.Add(new MailAddress("recipient@example.com"));
                message.Subject = "Sample MSG";
                message.Body = "This is a sample message saved as MSG format.";

                // Save as MSG with preserved original dates
                MsgSaveOptions saveOptions = new MsgSaveOptions(MailMessageSaveType.OutlookMessageFormatUnicode)
                {
                    PreserveOriginalDates = true
                };

                try
                {
                    message.Save(outputPath, saveOptions);
                    Console.WriteLine($"Message saved successfully to '{outputPath}'.");
                }
                catch (Exception ioEx)
                {
                    Console.Error.WriteLine($"Failed to save message: {ioEx.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
