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
            const string msgPath = "sample.msg";

            // Ensure the MSG file exists; create a minimal placeholder if it does not.
            if (!File.Exists(msgPath))
            {
                try
                {
                    using (MapiMessage placeholder = new MapiMessage(
                        "sender@example.com",
                        "receiver@example.com",
                        "Placeholder",
                        "This is a placeholder message."))
                    {
                        placeholder.Save(msgPath);
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create placeholder MSG file: {ex.Message}");
                    return;
                }
            }

            // Read the MSG file into a byte array.
            byte[] msgBytes;
            try
            {
                msgBytes = File.ReadAllBytes(msgPath);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to read MSG file: {ex.Message}");
                return;
            }

            // Load the message from the byte array and list attachment filenames.
            using (MemoryStream msgStream = new MemoryStream(msgBytes))
            {
                using (MapiMessage message = MapiMessage.Load(msgStream))
                {
                    foreach (MapiAttachment attachment in message.Attachments)
                    {
                        Console.WriteLine($"Attachment: {attachment.FileName}");
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
