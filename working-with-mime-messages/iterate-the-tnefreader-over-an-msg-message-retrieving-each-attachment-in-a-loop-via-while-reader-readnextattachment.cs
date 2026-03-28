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
            string msgPath = "sample.msg";

            // Ensure the MSG file exists before attempting to read it
            if (!File.Exists(msgPath))
            {
                Console.Error.WriteLine($"Input file not found: {msgPath}");
                return;
            }

            // Read the MSG file using MapiMessageReader
            using (MapiMessageReader reader = new MapiMessageReader(msgPath))
            {
                using (MapiMessage message = reader.ReadMessage())
                {
                    // Iterate over each attachment in the message
                    foreach (MapiAttachment attachment in message.Attachments)
                    {
                        Console.WriteLine($"Attachment: {attachment.FileName}");
                        // Save the attachment to the current directory
                        attachment.Save(attachment.FileName);
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
