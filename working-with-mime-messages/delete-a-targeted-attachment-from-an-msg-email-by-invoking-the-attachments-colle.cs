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
            string msgPath = @"sample.msg";
            if (!File.Exists(msgPath))
            {
                Console.Error.WriteLine($"File not found: {msgPath}");
                return;
            }

            using (MapiMessage message = MapiMessage.Load(msgPath))
            {
                string targetAttachmentName = "unwanted.txt";
                MapiAttachment attachmentToRemove = null;

                foreach (MapiAttachment att in message.Attachments)
                {
                    if (string.Equals(att.FileName, targetAttachmentName, StringComparison.OrdinalIgnoreCase))
                    {
                        attachmentToRemove = att;
                        break;
                    }
                }

                if (attachmentToRemove != null)
                {
                    message.Attachments.Remove(attachmentToRemove);
                    message.Save(msgPath);
                    Console.WriteLine($"Attachment '{targetAttachmentName}' removed.");
                }
                else
                {
                    Console.WriteLine($"Attachment '{targetAttachmentName}' not found.");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine(ex.Message);
        }
    }
}
