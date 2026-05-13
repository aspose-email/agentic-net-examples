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
            // Create a sample MailMessage with a few attachments
            using (MailMessage message = new MailMessage())
            {
                // Attachment 1: text file
                using (MemoryStream stream1 = new MemoryStream())
                {
                    Attachment attachment1 = new Attachment(stream1, "document.txt");
                    message.Attachments.Add(attachment1);
                }

                // Attachment 2: PDF file
                using (MemoryStream stream2 = new MemoryStream())
                {
                    Attachment attachment2 = new Attachment(stream2, "report.pdf");
                    message.Attachments.Add(attachment2);
                }

                // Attachment 3: another PDF (duplicate extension)
                using (MemoryStream stream3 = new MemoryStream())
                {
                    Attachment attachment3 = new Attachment(stream3, "summary.PDF");
                    message.Attachments.Add(attachment3);
                }

                // Extract distinct extensions
                List<string> extensions = GetDistinctAttachmentExtensions(message);

                Console.WriteLine("Distinct attachment extensions:");
                foreach (string ext in extensions)
                {
                    Console.WriteLine(ext);
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
            return;
        }
    }

    // Returns a list of distinct file extensions (including the dot) from the attachments of the given MailMessage.
    static List<string> GetDistinctAttachmentExtensions(MailMessage message)
    {
        HashSet<string> extensionSet = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        foreach (Attachment attachment in message.Attachments)
        {
            string extension = Path.GetExtension(attachment.Name);
            if (!string.IsNullOrEmpty(extension))
            {
                extensionSet.Add(extension);
            }
        }
        return new List<string>(extensionSet);
    }
}
