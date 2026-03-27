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

            if (!File.Exists(msgPath))
            {
                Console.Error.WriteLine($"File not found: {msgPath}");
                return;
            }

            using (MapiMessage message = MapiMessage.Load(msgPath))
            {
                Console.WriteLine($"Subject: {message.Subject}");
                Console.WriteLine($"From: {message.SenderName}");
                Console.WriteLine($"Body: {message.Body}");

                foreach (MapiAttachment attachment in message.Attachments)
                {
                    string attachmentPath = attachment.FileName;
                    try
                    {
                        attachment.Save(attachmentPath);
                        Console.WriteLine($"Saved attachment: {attachmentPath}");
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Failed to save attachment {attachment.FileName}: {ex.Message}");
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
