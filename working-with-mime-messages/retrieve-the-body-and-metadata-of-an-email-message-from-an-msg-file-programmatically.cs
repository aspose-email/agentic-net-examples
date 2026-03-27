using System;
using System.IO;
using Aspose.Email.Mapi;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            string msgFilePath = "message.msg";

            if (!File.Exists(msgFilePath))
            {
                Console.Error.WriteLine("File not found: " + msgFilePath);
                return;
            }

            using (MapiMessage message = MapiMessage.Load(msgFilePath))
            {
                string subject = message.Subject ?? string.Empty;
                string sender = message.SenderName ?? message.SenderEmailAddress ?? string.Empty;
                string body = message.Body ?? string.Empty;

                Console.WriteLine("Subject: " + subject);
                Console.WriteLine("From: " + sender);
                Console.WriteLine("Body: " + body);

                foreach (MapiAttachment attachment in message.Attachments)
                {
                    Console.WriteLine("Attachment: " + attachment.FileName);
                    // Example: save attachment to current directory
                    string attachmentPath = Path.Combine(Directory.GetCurrentDirectory(), attachment.FileName);
                    attachment.Save(attachmentPath);
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine("Error: " + ex.Message);
        }
    }
}
