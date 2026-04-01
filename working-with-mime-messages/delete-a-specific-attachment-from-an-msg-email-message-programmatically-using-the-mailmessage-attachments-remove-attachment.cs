using System;
using System.IO;
using Aspose.Email;

class Program
{
    static void Main()
    {
        try
        {
            string inputPath = "sample.msg";
            string outputPath = "sample_modified.msg";
            string attachmentNameToRemove = "remove.txt";

            if (!File.Exists(inputPath))
            {
                try
                {
                    using (MailMessage placeholder = new MailMessage(
                        "sender@example.com",
                        "recipient@example.com",
                        "Placeholder Subject",
                        "Placeholder body."))
                    {
                        placeholder.Save(inputPath, new MsgSaveOptions(MailMessageSaveType.OutlookMessageFormat));
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error creating placeholder MSG: {ex.Message}");
                    return;
                }

                Console.Error.WriteLine($"Error: File not found – {inputPath}");
                return;
            }

            using (MailMessage message = MailMessage.Load(inputPath))
            {
                // Locate the attachment to remove by name
                Attachment attachmentToRemove = null;
                foreach (Attachment att in message.Attachments)
                {
                    if (string.Equals(att.Name, attachmentNameToRemove, StringComparison.OrdinalIgnoreCase))
                    {
                        attachmentToRemove = att;
                        break;
                    }
                }

                if (attachmentToRemove != null)
                {
                    // Remove the attachment
                    message.Attachments.Remove(attachmentToRemove);

                    // Save the modified message
                    try
                    {
                        message.Save(outputPath, new MsgSaveOptions(MailMessageSaveType.OutlookMessageFormat));
                        Console.WriteLine($"Attachment '{attachmentNameToRemove}' removed and message saved to {outputPath}");
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Error saving message: {ex.Message}");
                    }
                }
                else
                {
                    Console.WriteLine($"Attachment '{attachmentNameToRemove}' not found in the message.");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
