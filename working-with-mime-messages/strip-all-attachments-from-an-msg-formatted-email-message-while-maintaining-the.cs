using System;
using System.IO;
using Aspose.Email.Mapi;

class Program
{
    static void Main()
    {
        try
        {
            string msgPath = "message.msg";

            if (!File.Exists(msgPath))
            {
                Console.Error.WriteLine($"Error: File not found – {msgPath}");
                return;
            }

            // Remove all attachments from the MSG file.
            MapiAttachmentCollection removedAttachments = MapiMessage.RemoveAttachments(msgPath);

            // List the names of the removed attachments (optional).
            foreach (MapiAttachment attachment in removedAttachments)
            {
                Console.WriteLine($"Removed attachment: {attachment.FileName}");
            }

            Console.WriteLine("All attachments have been stripped from the message.");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
