using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Mapi;

class Program
{
    static void Main()
    {
        try
        {
            string msgPath = "sample.msg";

            // Verify the MSG file exists
            if (!File.Exists(msgPath))
            {
                Console.Error.WriteLine($"Message file not found: {msgPath}");
                // Create a minimal placeholder MSG file
                using (MapiMessage placeholder = new MapiMessage(
                    "placeholder@domain.com",
                    "recipient@domain.com",
                    "Placeholder",
                    "Body"))
                {
                    placeholder.Save(msgPath);
                    Console.WriteLine($"Created placeholder MSG at {msgPath}");
                }
                return;
            }

            // Load the MSG file
            using (MapiMessage message = MapiMessage.Load(msgPath))
            {
                if (message.Attachments == null || message.Attachments.Count == 0)
                {
                    Console.WriteLine("No attachments found.");
                    return;
                }

                // Local folder representing the cloud storage bucket
                string bucketPath = "BucketArchive";

                // Ensure the bucket folder exists
                if (!Directory.Exists(bucketPath))
                {
                    Directory.CreateDirectory(bucketPath);
                }

                // Upload each attachment by saving it to the bucket folder
                foreach (MapiAttachment attachment in message.Attachments)
                {
                    try
                    {
                        string destFile = Path.Combine(bucketPath, attachment.FileName);
                        attachment.Save(destFile);
                        Console.WriteLine($"Uploaded attachment: {attachment.FileName}");
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Failed to upload attachment {attachment.FileName}: {ex.Message}");
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
