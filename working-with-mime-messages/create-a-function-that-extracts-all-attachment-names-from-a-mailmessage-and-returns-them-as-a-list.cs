using System;
using System.Collections.Generic;
using Aspose.Email;

namespace AsposeEmailAttachmentExample
{
    class Program
    {
        static void Main()
        {
            try
            {
                using (MailMessage message = new MailMessage())
                {
                    // Example: add attachments here if needed.
                    // For demonstration, we use an empty message.

                    List<string> attachmentNames = GetAttachmentNames(message);
                    Console.WriteLine("Attachment count: " + attachmentNames.Count);
                    foreach (string name in attachmentNames)
                    {
                        Console.WriteLine("Attachment: " + name);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(ex.Message);
                return;
            }
        }

        static List<string> GetAttachmentNames(MailMessage message)
        {
            List<string> names = new List<string>();
            if (message == null)
            {
                return names;
            }

            foreach (Attachment attachment in message.Attachments)
            {
                // Attachment.Name provides the file name.
                names.Add(attachment.Name);
            }

            return names;
        }
    }
}
