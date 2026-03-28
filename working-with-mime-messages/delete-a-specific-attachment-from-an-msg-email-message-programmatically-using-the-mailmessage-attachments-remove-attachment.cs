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
            string attachmentNameToRemove = "unwanted.txt";

            // Verify the MSG file exists before attempting to load it.
            if (!File.Exists(msgPath))
            {
                Console.Error.WriteLine($"Error: File not found – {msgPath}");
                return;
            }

            // Load the MSG file into a MailMessage instance.
            using (MailMessage mailMessage = MailMessage.Load(msgPath))
            {
                // Locate the attachment with the specified name.
                Attachment attachmentToRemove = null;
                foreach (Attachment att in mailMessage.Attachments)
                {
                    if (string.Equals(att.Name, attachmentNameToRemove, StringComparison.OrdinalIgnoreCase))
                    {
                        attachmentToRemove = att;
                        break;
                    }
                }

                // If the attachment was found, remove it.
                if (attachmentToRemove != null)
                {
                    mailMessage.Attachments.Remove(attachmentToRemove);
                    Console.WriteLine($"Attachment \"{attachmentNameToRemove}\" removed.");
                }
                else
                {
                    Console.WriteLine($"Attachment \"{attachmentNameToRemove}\" not found.");
                }

                // Save the modified message back to the same file.
                try
                {
                    mailMessage.Save(msgPath);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error saving file: {ex.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
