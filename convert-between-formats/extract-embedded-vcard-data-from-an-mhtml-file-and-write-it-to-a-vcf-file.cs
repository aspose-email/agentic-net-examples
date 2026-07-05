using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Mime;

namespace ExtractVCardFromMhtml
{
    class Program
    {
        static void Main(string[] args)
        {
            // Author: Aspose.Email example - extract vCard attachments from an MHTML file.
            string mhtmlPath = "input.mhtml";
            string outputFolder = "ExtractedVcards";

            // Guard file I/O
            if (!File.Exists(mhtmlPath))
            {
                try
                {
                    using (MailMessage placeholder = new MailMessage(
                        "sender@example.com",
                        "recipient@example.com",
                        "Placeholder Subject",
                        "Placeholder body."))
                    {
                        placeholder.Save(mhtmlPath, SaveOptions.DefaultEml);
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error creating placeholder message: {ex.Message}");
                    return;
                }

                Console.Error.WriteLine($"Input file not found: {mhtmlPath}");
                return;
            }

            try
            {
                // Ensure output directory exists
                if (!Directory.Exists(outputFolder))
                {
                    Directory.CreateDirectory(outputFolder);
                }

                // Load the MHTML message
                using (MailMessage message = MailMessage.Load(mhtmlPath))
                {
                    foreach (Attachment attachment in message.Attachments)
                    {
                        // Identify vCard attachments by MIME type or file extension
                        bool isVCard = string.Equals(attachment.ContentType.MediaType, "text/vcard", StringComparison.OrdinalIgnoreCase) ||
                                       (attachment.Name != null && attachment.Name.EndsWith(".vcf", StringComparison.OrdinalIgnoreCase));

                        if (!isVCard)
                            continue;

                        string targetPath = Path.Combine(outputFolder, attachment.Name ?? "contact.vcf");

                        // Write the attachment content to a .vcf file
                        using (MemoryStream ms = new MemoryStream())
                        {
                            attachment.ContentStream.CopyTo(ms);
                            File.WriteAllBytes(targetPath, ms.ToArray());
                        }

                        Console.WriteLine($"Extracted vCard to: {targetPath}");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"An error occurred: {ex.Message}");
            }
        }
    }
}
