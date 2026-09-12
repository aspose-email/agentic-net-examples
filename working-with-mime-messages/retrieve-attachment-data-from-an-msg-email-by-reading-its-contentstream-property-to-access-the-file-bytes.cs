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

            if (!File.Exists(msgPath))
            {
                try
                {
                    using (MapiMessage placeholder = new MapiMessage(
                        "from@example.com",
                        "to@example.com",
                        "Placeholder Subject",
                        "Placeholder body."))
                    {
                        placeholder.Save(msgPath);
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error creating placeholder MSG: {ex.Message}");
                    return;
                }

                Console.Error.WriteLine($"Message file not found: {msgPath}");
                return;
            }

            using (MapiMessage msg = MapiMessage.Load(msgPath))
            {
                foreach (MapiAttachment attachment in msg.Attachments)
                {
                    Console.WriteLine($"Attachment Name: {attachment.FileName}");

                    using (MemoryStream memory = new MemoryStream())
                    {
                        // Save attachment content to the memory stream
                        attachment.Save(memory);
                        byte[] attachmentData = memory.ToArray();
                        Console.WriteLine($"Attachment Size: {attachmentData.Length} bytes");
                        // attachmentData now holds the raw bytes of the attachment
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
