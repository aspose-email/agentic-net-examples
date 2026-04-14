using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Mapi;

class Program
{
    static void Main(string[] args)
    {
        // Top‑level exception guard
        try
        {
            // Determine the MSG file path (argument or default)
            string msgPath;
            if (args.Length > 0)
            {
                msgPath = args[0];
            }
            else
            {
                msgPath = "calendar.msg";
            }

            // Guard against missing file
            if (!File.Exists(msgPath))
            {
                Console.Error.WriteLine($"File not found: {msgPath}");
                return;
            }

            // Remove all attachments from the MSG file
            try
            {
                MapiAttachmentCollection removedAttachments = MapiMessage.RemoveAttachments(msgPath);
                Console.WriteLine($"Removed {removedAttachments.Count} attachment(s) from \"{msgPath}\".");
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error while removing attachments: {ex.Message}");
                return;
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
