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
            // Output file path for the MSG message
            string outputPath = "ReferenceAttachment.msg";

            // Ensure the output directory exists
            string directory = Path.GetDirectoryName(outputPath);
            if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            // Create a simple MAPI message
            using (MapiMessage message = new MapiMessage(
                "sender@example.com",
                "receiver@example.com",
                "Reference Attachment Example",
                "This message contains a reference attachment."))
            {
                // Configure reference attachment options
                ReferenceAttachmentOptions options = new ReferenceAttachmentOptions(
                    "https://example.com/file.pdf",
                    "https://example.com",
                    "ExampleProvider")
                {
                    PermissionType = AttachmentPermissionType.AnyoneCanEdit,
                    OriginalPermissionType = 0,
                    IsFolder = false
                };

                // Add the reference attachment to the message
                message.Attachments.Add("Document.pdf", options);

                // Save the message to a file
                try
                {
                    message.Save(outputPath);
                    Console.WriteLine($"Message saved to {outputPath}");
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
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
