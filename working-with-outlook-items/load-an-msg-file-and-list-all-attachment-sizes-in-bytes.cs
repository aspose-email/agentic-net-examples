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
            string msgPath = "sample.msg";

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

                Console.Error.WriteLine($"File not found: {msgPath}");
                return;
            }

            using (MapiMessage message = MapiMessage.Load(msgPath))
            {
                foreach (MapiAttachment attachment in message.Attachments)
                {
                    int size = attachment.BinaryData != null ? attachment.BinaryData.Length : 0;
                    Console.WriteLine($"Attachment: {attachment.FileName}, Size: {size} bytes");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
