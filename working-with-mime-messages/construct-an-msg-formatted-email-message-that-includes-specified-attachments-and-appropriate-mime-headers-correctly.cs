using System;
using System.IO;
using Aspose.Email;

class Program
{
    static void Main()
    {
        try
        {
            // Prepare attachment file
            string attachmentPath = "sample.txt";
            if (!File.Exists(attachmentPath))
            {
                try
                {
                    File.WriteAllText(attachmentPath, "Sample attachment content");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create attachment file: {ex.Message}");
                    return;
                }
            }

            // Create the email message
            using (MailMessage message = new MailMessage())
            {
                message.From = "sender@example.com";
                message.To = "receiver@example.com";
                message.Subject = "Test MSG with attachment";
                message.Body = "This is the body of the message.";

                // Add attachment
                using (Attachment attachment = new Attachment(attachmentPath))
                {
                    message.Attachments.Add(attachment);

                    // Prepare save options for MSG format
                    MsgSaveOptions saveOptions = new MsgSaveOptions(MailMessageSaveType.OutlookMessageFormat);

                    // Define output path and ensure directory exists
                    string outputPath = "output.msg";
                    string outputDir = Path.GetDirectoryName(outputPath);
                    if (!string.IsNullOrEmpty(outputDir) && !Directory.Exists(outputDir))
                    {
                        try
                        {
                            Directory.CreateDirectory(outputDir);
                        }
                        catch (Exception ex)
                        {
                            Console.Error.WriteLine($"Failed to create output directory: {ex.Message}");
                            return;
                        }
                    }

                    // Save the message as MSG
                    try
                    {
                        message.Save(outputPath, saveOptions);
                        Console.WriteLine($"Message saved successfully to {outputPath}");
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Failed to save message: {ex.Message}");
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
