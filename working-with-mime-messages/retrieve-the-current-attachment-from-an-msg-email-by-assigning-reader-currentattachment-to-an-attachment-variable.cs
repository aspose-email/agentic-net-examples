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
            // Path to the MSG file
            string msgPath = "sample.msg";

            // Ensure the file exists; create a minimal placeholder if missing
            if (!File.Exists(msgPath))
            {
                try
                {
                    // Create an empty MAPI message and save it as a placeholder
                    using (MapiMessage placeholder = new MapiMessage())
                    {
                        placeholder.Save(msgPath);
                    }
                    Console.WriteLine($"Placeholder MSG created at '{msgPath}'.");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create placeholder MSG: {ex.Message}");
                    return;
                }
            }

            // Open the MSG file with MapiMessageReader
            using (MapiMessageReader reader = new MapiMessageReader(msgPath))
            {
                // Read all attachments from the message
                MapiAttachmentCollection attachments = reader.ReadAttachments();

                if (attachments != null && attachments.Count > 0)
                {
                    // Retrieve the first attachment (as the "current" one)
                    MapiAttachment attachment = attachments[0];

                    // Example: display attachment name and size
                    Console.WriteLine($"Attachment Name: {attachment.FileName}");
                    Console.WriteLine($"Attachment Size: {attachment.BinaryData?.Length ?? 0} bytes");

                    // Optionally, save the attachment to disk
                    string outputDir = "Attachments";
                    try
                    {
                        if (!Directory.Exists(outputDir))
                        {
                            Directory.CreateDirectory(outputDir);
                        }

                        string outputPath = Path.Combine(outputDir, attachment.FileName);
                        File.WriteAllBytes(outputPath, attachment.BinaryData ?? Array.Empty<byte>());
                        Console.WriteLine($"Attachment saved to '{outputPath}'.");
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Failed to save attachment: {ex.Message}");
                    }
                }
                else
                {
                    Console.WriteLine("No attachments found in the MSG file.");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
