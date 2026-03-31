using Aspose.Email;
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

            // Ensure the input file exists; create a minimal placeholder if it does not.
            if (!File.Exists(inputPath))
            {
                try
                {
                    using (MapiMessage placeholder = new MapiMessage(
                        "Placeholder Subject",
                        "Placeholder Body",
                        "sender@example.com",
                        "recipient@example.com"))
                    {
                        placeholder.Save(inputPath);
                        Console.WriteLine($"Placeholder MSG created at '{inputPath}'.");
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create placeholder MSG: {ex.Message}");
                    return;
                }
            }

            // Remove all attachments from the MSG file.
            try
            {
                MapiAttachmentCollection removedAttachments = MapiMessage.RemoveAttachments(inputPath);
                Console.WriteLine($"Removed {removedAttachments.Count} attachment(s) from '{inputPath}'.");
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to remove attachments: {ex.Message}");
                return;
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
