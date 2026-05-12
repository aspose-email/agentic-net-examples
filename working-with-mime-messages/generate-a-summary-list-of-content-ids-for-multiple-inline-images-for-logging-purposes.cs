using System;
using System.Collections.Generic;
using System.IO;
using Aspose.Email;

class Program
{
    static void Main()
    {
        try
        {
            string messagePath = "sample.eml";

            if (!File.Exists(messagePath))
            {
                try
                {
                    using (MailMessage placeholder = new MailMessage(
                        "sender@example.com",
                        "recipient@example.com",
                        "Placeholder Subject",
                        "Placeholder body."))
                    {
                        placeholder.Save(messagePath, SaveOptions.DefaultEml);
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error creating placeholder message: {ex.Message}");
                    return;
                }

                Console.Error.WriteLine($"Message file not found: {messagePath}");
                return;
            }

            using (MailMessage message = MailMessage.Load(messagePath))
            {
                List<string> contentIds = new List<string>();

                foreach (Attachment attachment in message.Attachments)
                {
                    if (!string.IsNullOrEmpty(attachment.ContentId))
                    {
                        contentIds.Add(attachment.ContentId);
                    }
                }

                Console.WriteLine("Inline image Content-IDs:");
                foreach (string cid in contentIds)
                {
                    Console.WriteLine(cid);
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
