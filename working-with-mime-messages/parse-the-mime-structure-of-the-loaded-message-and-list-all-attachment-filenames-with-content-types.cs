using System;
using System.IO;
using Aspose.Email;

class Program
{
    static void Main()
    {
        try
        {
            string messagePath = "message.eml";

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

                Console.Error.WriteLine($"Input file not found: {messagePath}");
                return;
            }

            using (MailMessage mailMessage = MailMessage.Load(messagePath))
            {
                Console.WriteLine($"Subject: {mailMessage.Subject}");
                Console.WriteLine("Attachments:");

                foreach (Attachment attachment in mailMessage.Attachments)
                {
                    string fileName = attachment.Name ?? "Unnamed";
                    string contentType = attachment.ContentType?.MediaType ?? "unknown";
                    Console.WriteLine($"{fileName} - {contentType}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
