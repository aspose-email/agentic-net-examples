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

                Console.Error.WriteLine($"Message file '{msgPath}' does not exist.");
                return;
            }

            string attachmentsDir = "Attachments";
            if (!Directory.Exists(attachmentsDir))
            {
                Directory.CreateDirectory(attachmentsDir);
            }

            using (MapiMessage msg = MapiMessage.Load(msgPath))
            {
                // Metadata
                Console.WriteLine($"Subject: {msg.Subject}");
                Console.WriteLine($"From: {msg.SenderEmailAddress}");
                Console.WriteLine($"Sent: {msg.DeliveryTime}");
                Console.WriteLine($"Body: {msg.Body}");
                Console.WriteLine($"HTML Body Length: {(msg.BodyHtml != null ? msg.BodyHtml.Length : 0)}");
                Console.WriteLine($"Attachments Count: {(msg.Attachments != null ? msg.Attachments.Count : 0)}");

                // Attachments
                if (msg.Attachments != null)
                {
                    foreach (MapiAttachment attachment in msg.Attachments)
                    {
                        string attachmentPath = Path.Combine(attachmentsDir, attachment.FileName);
                        try
                        {
                            attachment.Save(attachmentPath);
                            Console.WriteLine($"Saved attachment: {attachment.FileName}");
                        }
                        catch (Exception ex)
                        {
                            Console.Error.WriteLine($"Failed to save attachment '{attachment.FileName}': {ex.Message}");
                        }
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
