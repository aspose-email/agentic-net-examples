using System;
using System.IO;
using Aspose.Email;

class Program
{
    static void Main()
    {
        try
        {
            // Define output MSG file path
            string outputPath = Path.Combine(Directory.GetCurrentDirectory(), "output.msg");
            string outputDir = Path.GetDirectoryName(outputPath);
            if (!Directory.Exists(outputDir))
            {
                Directory.CreateDirectory(outputDir);
            }

            // Compose the email
            using (MailMessage message = new MailMessage())
            {
                message.From = new MailAddress("sender@example.com");
                message.To.Add(new MailAddress("recipient@example.com"));
                message.Subject = "Sample MSG Email";
                message.Body = "This is a sample email saved as MSG format.";

                // Save as MSG using MsgSaveOptions with the appropriate save type
                MsgSaveOptions saveOptions = new MsgSaveOptions(MailMessageSaveType.OutlookMessageFormat);
                try
                {
                    message.Save(outputPath, saveOptions);
                    Console.WriteLine($"Message saved successfully to '{outputPath}'.");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to save message: {ex.Message}");
                    return;
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
