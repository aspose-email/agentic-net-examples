using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Mime;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            string msgPath = "sample.msg";

            // Verify that the MSG file exists
            if (!File.Exists(msgPath))
            {
                Console.Error.WriteLine($"Error: File not found – {msgPath}");
                return;
            }

            // Load the MSG file into a MailMessage instance
            using (MailMessage message = MailMessage.Load(msgPath))
            {
                // Identify the attachment to remove (by name)
                Attachment attachmentToRemove = null;
                foreach (Attachment att in message.Attachments)
                {
                    if (att.Name == "remove.txt")
                    {
                        attachmentToRemove = att;
                        break;
                    }
                }

                if (attachmentToRemove != null)
                {
                    // Remove the attachment
                    message.Attachments.Remove(attachmentToRemove);

                    // Save the modified message back to the same file
                    message.Save(msgPath, SaveOptions.DefaultMsg);
                    Console.WriteLine("Attachment removed and message saved.");
                }
                else
                {
                    Console.WriteLine("Specified attachment not found.");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
