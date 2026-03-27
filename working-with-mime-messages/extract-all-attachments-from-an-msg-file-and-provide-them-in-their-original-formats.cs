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
            string msgPath = "sample.msg";

            // Verify the MSG file exists
            if (!File.Exists(msgPath))
            {
                Console.Error.WriteLine($"Error: File not found – {msgPath}");
                return;
            }

            // Ensure the output directory exists
            string outputDir = Path.GetDirectoryName(msgPath) ?? Directory.GetCurrentDirectory();
            if (!Directory.Exists(outputDir))
            {
                try
                {
                    Directory.CreateDirectory(outputDir);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create directory '{outputDir}': {ex.Message}");
                    return;
                }
            }

            // Load the MSG file and extract attachments
            using (MapiMessage message = MapiMessage.Load(msgPath))
            {
                MapiAttachmentCollection attachments = message.Attachments;
                foreach (MapiAttachment attachment in attachments)
                {
                    Console.WriteLine($"Attachment Name: {attachment.FileName}");
                    string attachmentPath = Path.Combine(outputDir, attachment.FileName);
                    try
                    {
                        attachment.Save(attachmentPath);
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Failed to save attachment '{attachment.FileName}': {ex.Message}");
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
