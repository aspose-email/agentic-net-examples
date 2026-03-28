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

            // Guard file existence
            if (!File.Exists(msgPath))
{
    try
    {
        MailMessage placeholderMsg = new MailMessage("sender@example.com", "recipient@example.com", "Placeholder", "This is a placeholder MSG.");
        placeholderMsg.Save(msgPath, SaveOptions.DefaultMsgUnicode);
    }
    catch (Exception ex)
    {
        Console.Error.WriteLine($"Failed to create placeholder MSG: {ex.Message}");
        return;
    }
}


            // Load the MSG file
            using (MapiMessage msg = MapiMessage.Load(msgPath))
            {
                // Ensure there is at least one attachment
                if (msg.Attachments == null || msg.Attachments.Count == 0)
                {
                    Console.WriteLine("No attachments found.");
                    return;
                }

                // Iterate through attachments and read their data via a stream
                foreach (MapiAttachment attachment in msg.Attachments)
                {
                    // BinaryData holds the raw bytes of the attachment
                    byte[] data = attachment.BinaryData;

                    // Create a memory stream to mimic ContentStream usage
                    using (MemoryStream contentStream = new MemoryStream(data))
                    {
                        // Example: read all bytes from the stream
                        byte[] buffer = new byte[contentStream.Length];
                        int read = contentStream.Read(buffer, 0, buffer.Length);

                        Console.WriteLine($"Attachment: {attachment.FileName}");
                        Console.WriteLine($"Bytes read from ContentStream: {read}");
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
