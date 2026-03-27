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
            string msgPath = "message.msg";

            // Guard file existence
            if (!File.Exists(msgPath))
            {
                Console.Error.WriteLine($"Error: File not found – {msgPath}");
                return;
            }

            // Load the MSG file
            using (MapiMessage msg = MapiMessage.Load(msgPath))
            {
                // Iterate through attachments
                foreach (MapiAttachment attachment in msg.Attachments)
                {
                    // Access the property stream
                    MapiPropertyStream propStream = attachment.PropertyStream;
                    object contentObj = propStream.Content;

                    // Ensure the content is a Stream
                    if (contentObj is Stream contentStream)
                    {
                        // Read the attachment data
                        using (contentStream)
                        using (MemoryStream ms = new MemoryStream())
                        {
                            contentStream.CopyTo(ms);
                            byte[] data = ms.ToArray();
                            Console.WriteLine($"Attachment: {attachment.FileName}, Size: {data.Length} bytes");
                        }
                    }
                    else
                    {
                        Console.Error.WriteLine($"Attachment {attachment.FileName} does not contain a stream.");
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
