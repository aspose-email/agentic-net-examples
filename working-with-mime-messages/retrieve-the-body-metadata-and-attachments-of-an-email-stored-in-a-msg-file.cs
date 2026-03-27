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
            string msgPath = "sample.msg";

            if (!File.Exists(msgPath))
            {
                Console.Error.WriteLine($"File not found: {msgPath}");
                return;
            }

            using (MapiMessage message = MapiMessage.Load(msgPath))
            {
                // Metadata
                Console.WriteLine("Subject: " + (message.Subject ?? string.Empty));
                Console.WriteLine("From: " + (message.SenderName ?? string.Empty));
                Console.WriteLine("To: " + (message.DisplayTo ?? string.Empty));
                Console.WriteLine("CC: " + (message.DisplayCc ?? string.Empty));
                Console.WriteLine("BCC: " + (message.DisplayBcc ?? string.Empty));
                Console.WriteLine("Sent Time: " + message.ClientSubmitTime);

                // Body
                Console.WriteLine("Body:");
                Console.WriteLine(message.Body ?? string.Empty);

                // Attachments
                foreach (MapiAttachment attachment in message.Attachments)
                {
                    Console.WriteLine("Attachment: " + attachment.FileName);
                    string attachmentPath = Path.Combine(Path.GetDirectoryName(msgPath) ?? string.Empty, attachment.FileName);
                    attachment.Save(attachmentPath);
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine(ex.Message);
        }
    }
}
