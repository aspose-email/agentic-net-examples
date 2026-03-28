using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Mapi;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            // Path to the MSG file
            string msgFilePath = "sample.msg";

            // Verify the MSG file exists
            if (!File.Exists(msgFilePath))
{
    try
    {
        MailMessage placeholderMsg = new MailMessage("sender@example.com", "recipient@example.com", "Placeholder", "This is a placeholder MSG.");
        placeholderMsg.Save(msgFilePath, SaveOptions.DefaultMsgUnicode);
    }
    catch (Exception ex)
    {
        Console.Error.WriteLine($"Failed to create placeholder MSG: {ex.Message}");
        return;
    }
}


            // Directory where attachments will be saved
            string outputDirectory = "ExtractedAttachments";

            // Ensure the output directory exists
            if (!Directory.Exists(outputDirectory))
            {
                Directory.CreateDirectory(outputDirectory);
            }

            // Load the MSG file
            using (MapiMessage message = MapiMessage.Load(msgFilePath))
            {
                Console.WriteLine($"Subject: {message.Subject}");

                // Iterate through all attachments (including embedded objects)
                foreach (MapiAttachment attachment in message.Attachments)
                {
                    // Build a full path for the attachment file
                    string attachmentPath = Path.Combine(outputDirectory, attachment.FileName);

                    // Save the attachment to disk
                    attachment.Save(attachmentPath);

                    Console.WriteLine($"Saved attachment: {attachment.FileName}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
