using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Tools.Search;

class Program
{
    static void Main()
    {
        try
        {
            // Define temporary directory for test files
            string tempDir = Path.Combine(Path.GetTempPath(), "AsposeEmailLargeAttachmentTest");
            if (!Directory.Exists(tempDir))
            {
                Directory.CreateDirectory(tempDir);
            }

            // Paths for the intermediate EML and final MSG files
            string emlPath = Path.Combine(tempDir, "LargeAttachment.eml");
            string msgPath = Path.Combine(tempDir, "LargeAttachment.msg");

            // Simulate a large Base64 attachment (e.g., ~5 MB)
            int sizeInBytes = 5 * 1024 * 1024; // 5 MB
            byte[] randomData = new byte[sizeInBytes];
            new Random().NextBytes(randomData);
            string base64Content = Convert.ToBase64String(randomData);

            // Create a MailMessage with a large Base64 attachment
            MailMessage message = new MailMessage();
            message.From = "sender@example.com";
            message.To = "receiver@example.com";
            message.Subject = "Test message with large Base64 attachment";
            message.Body = "This email contains a large Base64 encoded attachment.";

            // Create attachment from the Base64 string
            // Content type "application/octet-stream" is generic binary
            Attachment largeAttachment = Attachment.CreateAttachmentFromString(base64Content, "application/octet-stream");
            largeAttachment.Name = "large.bin";
            message.Attachments.Add(largeAttachment);

            // Save the message as EML
            try
            {
                message.Save(emlPath);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to save EML file: {ex.Message}");
                return;
            }

            // Ensure the EML file exists before loading
            if (!File.Exists(emlPath))
            {
                try
                {
                    using (MailMessage placeholder = new MailMessage(
                        "sender@example.com",
                        "recipient@example.com",
                        "Placeholder Subject",
                        "Placeholder body."))
                    {
                        placeholder.Save(emlPath, SaveOptions.DefaultEml);
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error creating placeholder message: {ex.Message}");
                    return;
                }

                Console.Error.WriteLine("EML file was not created.");
                return;
            }

            // Load the EML with options to preserve attachments
            EmlLoadOptions emlLoadOptions = new EmlLoadOptions()
            {
                PreserveTnefAttachments = true,
                PreserveEmbeddedMessageFormat = true
            };

            using (MailMessage loadedMessage = MailMessage.Load(emlPath, emlLoadOptions))
            {
                // Convert and save as MSG
                try
                {
                    loadedMessage.Save(msgPath, SaveOptions.DefaultMsg);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to save MSG file: {ex.Message}");
                    return;
                }
            }

            // Verify MSG file creation
            if (File.Exists(msgPath))
            {
                Console.WriteLine("Conversion succeeded. MSG file created at:");
                Console.WriteLine(msgPath);
            }
            else
            {
                Console.Error.WriteLine("MSG file was not created.");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
