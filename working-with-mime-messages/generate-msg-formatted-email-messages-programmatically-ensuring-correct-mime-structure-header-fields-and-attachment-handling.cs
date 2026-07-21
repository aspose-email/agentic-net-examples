using System;
using System.IO;
using Aspose.Email;

class Program
{
    static void Main()
    {
        try
        {
            // Output directory and file path
            string outputDirectory = "Output";
            string outputFilePath = Path.Combine(outputDirectory, "SampleMessage.msg");

            // Ensure the output directory exists
            if (!Directory.Exists(outputDirectory))
            {
                Directory.CreateDirectory(outputDirectory);
            }

            // Create the email message
            using (MailMessage message = new MailMessage())
            {
                message.From = "sender@example.com";
                message.To = "receiver@example.com";
                message.Subject = "Sample MSG with Attachments";
                message.Body = "This is the body of the email.";

                // Attachment file names
                string[] attachmentFiles = { "attachment1.txt", "attachment2.jpg" };

                foreach (string fileName in attachmentFiles)
                {
                    // Ensure the attachment file exists; create a minimal placeholder if missing
                    if (!File.Exists(fileName))
                    {
                        try
                        {
                            File.WriteAllText(fileName, $"Placeholder content for {fileName}");
                        }
                        catch (Exception ex)
                        {
                            Console.Error.WriteLine($"Failed to create placeholder '{fileName}': {ex.Message}");
                            continue;
                        }
                    }

                    // Add the attachment to the message
                    try
                    {
                        Attachment attachment = new Attachment(fileName);
                        message.Attachments.Add(attachment);
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Failed to add attachment '{fileName}': {ex.Message}");
                    }
                }

                // Save the message as MSG with original dates preserved
                MsgSaveOptions saveOptions = new MsgSaveOptions(MailMessageSaveType.OutlookMessageFormatUnicode)
                {
                    PreserveOriginalDates = true
                };

                try
                {
                    message.Save(outputFilePath, saveOptions);
                    Console.WriteLine($"Message saved to '{outputFilePath}'.");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to save MSG file: {ex.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
