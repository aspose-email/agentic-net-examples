using System;
using System.IO;
using Aspose.Email;

class Program
{
    static void Main()
    {
        try
        {
            string msgPath = @"c:\temp\sample.msg";
            string attachmentNameToRemove = "unwanted.pdf";

            if (!File.Exists(msgPath))
            {
                Console.Error.WriteLine($"Message file not found: {msgPath}");
                return;
            }

            using (MailMessage message = MailMessage.Load(msgPath))
            {
                Attachment targetAttachment = null;
                foreach (Attachment att in message.Attachments)
                {
                    if (string.Equals(att.Name, attachmentNameToRemove, StringComparison.OrdinalIgnoreCase))
                    {
                        targetAttachment = att;
                        break;
                    }
                }

                if (targetAttachment != null)
                {
                    message.Attachments.Remove(targetAttachment);
                    // Save the modified message back to the same file
                    message.Save(msgPath);
                    Console.WriteLine($"Attachment '{attachmentNameToRemove}' removed successfully.");
                }
                else
                {
                    Console.WriteLine($"Attachment '{attachmentNameToRemove}' not found.");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
