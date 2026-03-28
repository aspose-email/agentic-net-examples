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
            string msgFilePath = "outlookmessage.msg";

            if (!File.Exists(msgFilePath))
            {
                Console.Error.WriteLine($"File not found: {msgFilePath}");
                return;
            }

            try
            {
                using (MapiMessage message = MapiMessage.Load(msgFilePath))
                {
                    Console.WriteLine("Subject: " + message.Subject);
                    Console.WriteLine("From: " + message.SenderEmailAddress);
                    Console.WriteLine("Body: " + message.Body);

                    foreach (MapiAttachment attachment in message.Attachments)
                    {
                        Console.WriteLine("Attachment: " + attachment.FileName);
                        try
                        {
                            attachment.Save(attachment.FileName);
                            Console.WriteLine($"Saved attachment to {attachment.FileName}");
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
                Console.Error.WriteLine($"Error loading MSG file: {ex.Message}");
                return;
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
