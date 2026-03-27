using System;
using System.IO;
using Aspose.Email.Mapi;

class Program
{
    static void Main()
    {
        try
        {
            string inputPath = "message.msg";
            if (!File.Exists(inputPath))
            {
                Console.Error.WriteLine($"Error: File not found – {inputPath}");
                return;
            }

            // Remove all attachments from the MSG file.
            // The method returns the collection of removed attachments.
            MapiAttachmentCollection removedAttachments = MapiMessage.RemoveAttachments(inputPath);
            Console.WriteLine($"Removed {removedAttachments.Count} attachment(s).");

            // Load the message to verify its content and optionally save it without attachments.
            using (MapiMessage message = MapiMessage.Load(inputPath))
            {
                Console.WriteLine($"Subject: {message.Subject}");
                Console.WriteLine($"Body: {message.Body}");

                string outputPath = "message_no_attachments.msg";
                message.Save(outputPath);
                Console.WriteLine($"Message saved without attachments to: {outputPath}");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
