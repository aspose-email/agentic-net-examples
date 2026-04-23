using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Mapi;
using Aspose.Email.Storage;

class Program
{
    static void Main()
    {
        try
        {
            string emlPath = "sample.eml";

            // Ensure the input EML file exists
            if (!File.Exists(emlPath))
            {
                try
                {
                    using (StreamWriter writer = new StreamWriter(emlPath, false))
                    {
                        writer.WriteLine("From: sender@example.com");
                        writer.WriteLine("To: receiver@example.com");
                        writer.WriteLine("Subject: Test Message");
                        writer.WriteLine();
                        writer.WriteLine("This is a test email body.");
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create placeholder EML file: {ex.Message}");
                    return;
                }
            }

            // Run unit‑style tests
            TestAttachmentCompression(emlPath, true);
            TestAttachmentCompression(emlPath, false);
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }

    static void TestAttachmentCompression(string emlFilePath, bool enableCompression)
    {
        Console.WriteLine($"Testing conversion with attachment compression set to {(enableCompression ? "enabled" : "disabled")}...");

        try
        {
            using (MailMessage message = MailMessage.Load(emlFilePath))
            {
                // Add a simple attachment to ensure there is something to compress
                using (MemoryStream attachmentStream = new MemoryStream())
                {
                    using (StreamWriter attWriter = new StreamWriter(attachmentStream, leaveOpen: true))
                    {
                        attWriter.Write("Attachment content");
                        attWriter.Flush();
                        attachmentStream.Position = 0;
                    }

                    Attachment attachment = new Attachment(attachmentStream, "text/plain")
                    {
                        Name = "sample.txt"
                    };
                    message.Attachments.Add(attachment);

                    // Configure conversion options
                    MapiConversionOptions options = new MapiConversionOptions
                    {
                        UseBodyCompression = enableCompression
                    };

                    // Perform conversion
                    MapiMessage mapiMessage = MapiMessage.FromMailMessage(message, options);

                    // Simple verification: ensure the MapiMessage was created
                    if (mapiMessage == null)
                    {
                        Console.Error.WriteLine("Conversion returned null MapiMessage.");
                    }
                    else
                    {
                        Console.WriteLine("Conversion succeeded.");
                    }

                    // Cleanup
                    mapiMessage.Dispose();
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Test failed: {ex.Message}");
        }
    }
}
