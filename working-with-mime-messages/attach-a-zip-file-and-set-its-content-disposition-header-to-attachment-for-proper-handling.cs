using System;
using System.IO;
using System.IO.Compression;
using Aspose.Email;
using Aspose.Email.Mime;

class Program
{
    static void Main()
    {
        try
        {
            const string zipPath = "sample.zip";

            // Ensure the ZIP file exists; create a minimal placeholder if missing
            if (!File.Exists(zipPath))
            {
                try
                {
                    using (FileStream fs = new FileStream(zipPath, FileMode.Create))
                    using (ZipArchive archive = new ZipArchive(fs, ZipArchiveMode.Create))
                    {
                        archive.CreateEntry("placeholder.txt");
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create placeholder ZIP: {ex.Message}");
                    return;
                }
            }

            using (MailMessage message = new MailMessage())
            {
                message.From = "sender@example.com";
                message.To.Add("receiver@example.com");
                message.Subject = "Message with ZIP attachment";

                using (Attachment attachment = new Attachment(zipPath))
                {
                    // Set Content‑Disposition header to attachment
                    ContentDisposition disposition = attachment.ContentDisposition;
                    if (disposition != null)
                    {
                        disposition.DispositionType = DispositionTypeNames.Attachment; // "attachment"
                    }

                    message.Attachments.Add(attachment);

                    // Save the message to an EML file (optional demonstration)
                    const string emlPath = "message.eml";
                    try
                    {
                        message.Save(emlPath, SaveOptions.DefaultEml);
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Failed to save message: {ex.Message}");
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
