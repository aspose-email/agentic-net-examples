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
            // Author note: Example reads attachments from an MSG file via a stream.
            string msgPath = "outlookmessage.msg";

            // Guard file existence
            if (!File.Exists(msgPath))
            {
                Console.Error.WriteLine($"Input file not found: {msgPath}");
                return;
            }

            // Open the MSG file as a stream
            using (FileStream fileStream = new FileStream(msgPath, FileMode.Open, FileAccess.Read))
            {
                // Initialize the MapiMessageReader with the stream
                using (MapiMessageReader reader = new MapiMessageReader(fileStream))
                {
                    // Parse the message
                    MapiMessage message = reader.ReadMessage();

                    // Iterate through attachments
                    foreach (MapiAttachment attachment in message.Attachments)
                    {
                        // Save attachment content to a memory stream
                        using (MemoryStream memory = new MemoryStream())
                        {
                            attachment.Save(memory);
                            memory.Position = 0; // Reset for reading if needed

                            // Example: output attachment name and size
                            Console.WriteLine($"Attachment: {attachment.FileName}, Size: {memory.Length} bytes");
                        }
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
