using System;
using System.IO;
using Aspose.Email;

class Program
{
    static void Main()
    {
        try
        {
            string inputPath = "input.msg";
            string outputPath = "output.msg";

            // Ensure input MSG exists; create a minimal placeholder if missing
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
                        placeholder.Save(inputPath, new MsgSaveOptions(MailMessageSaveType.OutlookMessageFormat));
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error creating placeholder MSG: {ex.Message}");
                    return;
                }

                try
                {
                    using (MailMessage placeholder = new MailMessage())
                    {
                        placeholder.From = "sender@example.com";
                        placeholder.To.Add("recipient@example.com");
                        placeholder.Subject = "Placeholder";
                        placeholder.Body = "Original body.";
                        placeholder.Save(inputPath, new MsgSaveOptions(MailMessageSaveType.OutlookMessageFormat));
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create placeholder MSG: {ex.Message}");
                    return;
                }
            }

            // Load, modify, and save the message
            try
            {
                using (MailMessage message = MailMessage.Load(inputPath))
                {
                    message.Body = "Updated body text.";

                    string outputDirectory = Path.GetDirectoryName(outputPath);
                    if (!string.IsNullOrEmpty(outputDirectory) && !Directory.Exists(outputDirectory))
                    {
                        Directory.CreateDirectory(outputDirectory);
                    }

                    message.Save(outputPath, new MsgSaveOptions(MailMessageSaveType.OutlookMessageFormat));
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to load or save MSG: {ex.Message}");
                return;
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
