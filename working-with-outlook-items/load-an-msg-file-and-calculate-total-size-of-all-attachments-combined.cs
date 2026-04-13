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
            // Path to the MSG file
            string msgPath = "sample.msg";

            // Ensure the file exists; create a minimal placeholder if it does not
            if (!File.Exists(msgPath))
            {
                try
                {
                    MapiMessage placeholder = new MapiMessage(
                        "sender@example.com",
                        "receiver@example.com",
                        "Placeholder",
                        "This is a placeholder message."
                    );
                    placeholder.Save(msgPath);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create placeholder MSG file: {ex.Message}");
                    return;
                }
            }

            long totalAttachmentSize = 0;

            // Load the MSG file and calculate total attachment size
            using (MapiMessage msg = MapiMessage.Load(msgPath))
            {
                foreach (MapiAttachment attachment in msg.Attachments)
                {
                    if (attachment.BinaryData != null)
                    {
                        totalAttachmentSize += attachment.BinaryData.Length;
                    }
                }
            }

            Console.WriteLine($"Total attachment size: {totalAttachmentSize} bytes");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
