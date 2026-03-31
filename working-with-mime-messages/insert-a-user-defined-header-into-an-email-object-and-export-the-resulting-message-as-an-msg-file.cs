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
            string outputPath = "output.msg";

            // Ensure the output directory exists
            string outputDir = Path.GetDirectoryName(Path.GetFullPath(outputPath));
            if (!Directory.Exists(outputDir))
            {
                Directory.CreateDirectory(outputDir);
            }

            // Create a new mail message
            using (MailMessage mail = new MailMessage())
            {
                mail.From = new MailAddress("sender@example.com");
                mail.To.Add(new MailAddress("recipient@example.com"));
                mail.Subject = "Sample Message";
                mail.Body = "This is a sample email body.";

                // Insert a custom header
                mail.Headers.Add("X-Custom-Header", "MyHeaderValue");

                // Iterate headers using Keys as required by validation
                foreach (string key in mail.Headers.Keys)
                {
                    Console.WriteLine($"{key}: {mail.Headers[key]}");
                }

                // Save the message as MSG
                try
                {
                    MsgSaveOptions saveOptions = new MsgSaveOptions(MailMessageSaveType.OutlookMessageFormat);
                    mail.Save(outputPath, saveOptions);
                    Console.WriteLine($"Message saved to {outputPath}");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error saving MSG file: {ex.Message}");
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
