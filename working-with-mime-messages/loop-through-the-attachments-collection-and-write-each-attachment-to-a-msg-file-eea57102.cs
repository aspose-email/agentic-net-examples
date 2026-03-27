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
            // Path to the source MSG file
            string msgPath = "input.msg";

            // Verify the source file exists
            if (!File.Exists(msgPath))
            {
                Console.Error.WriteLine($"Error: File not found – {msgPath}");
                return;
            }

            // Directory where attachments will be saved
            string outputDir = "Attachments";

            // Ensure the output directory exists
            if (!Directory.Exists(outputDir))
            {
                Directory.CreateDirectory(outputDir);
            }

            // Load the Outlook message
            using (MapiMessage message = MapiMessage.Load(msgPath))
            {
                // Loop through each attachment in the message
                foreach (MapiAttachment attachment in message.Attachments)
                {
                    // Build the full path for the attachment file
                    string attachmentPath = Path.Combine(outputDir, attachment.FileName);

                    try
                    {
                        // Save the attachment to disk
                        attachment.Save(attachmentPath);
                        Console.WriteLine($"Saved attachment: {attachmentPath}");
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Error saving attachment '{attachment.FileName}': {ex.Message}");
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
