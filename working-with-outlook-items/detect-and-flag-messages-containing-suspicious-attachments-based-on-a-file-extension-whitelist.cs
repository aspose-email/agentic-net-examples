using System;
using System.IO;
using System.Collections.Generic;
using Aspose.Email;

class Program
{
    static void Main()
    {
        try
        {
            // Path to the email file (EML or MSG)
            string messagePath = "sample.eml";

            // Verify that the input file exists before attempting to load it
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

            // Define a whitelist of safe attachment extensions (lower‑case)
            List<string> allowedExtensions = new List<string>
            {
                ".pdf",
                ".docx",
                ".xlsx",
                ".txt"
            };

            try
            {
                // Load the email message; MailMessage implements IDisposable
                using (MailMessage message = MailMessage.Load(messagePath))
                {
                    // Iterate through all attachments in the message
                    foreach (Attachment attachment in message.Attachments)
                    {
                        // Attachment.Name provides the file name of the attachment
                        string attachmentName = attachment.Name;
                        string extension = Path.GetExtension(attachmentName).ToLowerInvariant();

                        // Flag any attachment whose extension is not in the whitelist
                        if (!allowedExtensions.Contains(extension))
                        {
                            Console.WriteLine($"Suspicious attachment detected: {attachmentName}");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to process the email message: {ex.Message}");
                return;
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
