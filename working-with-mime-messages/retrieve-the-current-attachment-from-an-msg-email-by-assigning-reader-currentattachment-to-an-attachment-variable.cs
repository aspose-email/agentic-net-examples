using System;
using System.IO;
using Aspose.Email.Mapi;

namespace Example
{
    class Program
    {
        static void Main()
        {
            try
            {
                // Path to the MSG file
                string msgPath = "sample.msg";

                // Verify that the file exists before attempting to read it
                if (!File.Exists(msgPath))
{
    try
    {
        MailMessage placeholderMsg = new MailMessage("sender@example.com", "recipient@example.com", "Placeholder", "This is a placeholder MSG.");
        placeholderMsg.Save(msgPath, SaveOptions.DefaultMsgUnicode);
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
                    // Extract all attachments from the message
                    MapiAttachmentCollection attachments = reader.ReadAttachments();

                    if (attachments == null || attachments.Count == 0)
                    {
                        Console.WriteLine("No attachments found in the message.");
                        return;
                    }

                    // Retrieve the first attachment (as an example)
                    MapiAttachment attachment = attachments[0];

                    // Save the attachment to disk
                    string outputPath = attachment.FileName;
                    attachment.Save(outputPath);
                    Console.WriteLine($"Attachment saved to: {outputPath}");
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error: {ex.Message}");
            }
        }
    }
}
