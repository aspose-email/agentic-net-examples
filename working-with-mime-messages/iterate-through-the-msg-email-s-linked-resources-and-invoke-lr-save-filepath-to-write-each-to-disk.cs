using System;
using System.IO;
using Aspose.Email;

class Program
{
    static void Main()
    {
        try
        {
            // Input MSG file path
            string msgPath = "sample.msg";

            // Verify the input file exists
            if (!File.Exists(msgPath))
            {
                try
                {
                    using (MailMessage placeholder = new MailMessage(
                        "sender@example.com",
                        "recipient@example.com",
                        "Placeholder Subject",
                        "Placeholder body."))
                    {
                        placeholder.Save(msgPath, new MsgSaveOptions(MailMessageSaveType.OutlookMessageFormat));
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error creating placeholder MSG: {ex.Message}");
                    return;
                }

                Console.Error.WriteLine($"Input file not found: {msgPath}");
                return;
            }

            // Load the message (MailMessage implements IDisposable)
            using (MailMessage message = MailMessage.Load(msgPath))
            {
                // Prepare output directory for linked resources
                string outputDir = "LinkedResources";
                Directory.CreateDirectory(outputDir);

                int resourceIndex = 0;
                foreach (LinkedResource linkedResource in message.LinkedResources)
                {
                    // Build a simple file name for each resource
                    string extension = ".bin";
                    if (linkedResource.ContentType != null && !string.IsNullOrEmpty(linkedResource.ContentType.MediaType))
                    {
                        // Attempt to derive a file extension from the media type (e.g., "image/png" -> ".png")
                        string[] parts = linkedResource.ContentType.MediaType.Split('/');
                        if (parts.Length == 2)
                        {
                            extension = "." + parts[1];
                        }
                    }

                    string fileName = $"resource_{resourceIndex}{extension}";
                    string outputPath = Path.Combine(outputDir, fileName);
                    resourceIndex++;

                    try
                    {
                        // Save the linked resource to disk
                        linkedResource.Save(outputPath);
                        Console.WriteLine($"Saved linked resource to {outputPath}");
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Failed to save linked resource '{fileName}': {ex.Message}");
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
