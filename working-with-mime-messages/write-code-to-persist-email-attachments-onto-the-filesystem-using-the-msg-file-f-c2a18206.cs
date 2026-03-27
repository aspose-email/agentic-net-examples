using System;
using System.IO;
using Aspose.Email.Mapi;

class Program
{
    static void Main()
    {
        try
        {
            // Path to the source MSG file
            string msgFilePath = "sample.msg";
            // Directory where attachments will be saved
            string attachmentsFolder = "Attachments";

            // Verify that the MSG file exists
            if (!File.Exists(msgFilePath))
            {
                Console.Error.WriteLine($"Error: File not found – {msgFilePath}");
                return;
            }

            // Ensure the output directory exists; create if necessary
            if (!Directory.Exists(attachmentsFolder))
            {
                try
                {
                    Directory.CreateDirectory(attachmentsFolder);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error: Unable to create directory – {attachmentsFolder}. {ex.Message}");
                    return;
                }
            }

            // Load the MSG file and extract attachments
            using (MapiMessage msg = MapiMessage.Load(msgFilePath))
            {
                foreach (MapiAttachment attachment in msg.Attachments)
                {
                    // Build full path for the attachment file
                    string attachmentPath = Path.Combine(attachmentsFolder, attachment.FileName);

                    try
                    {
                        // Save the attachment to the file system
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
