using System;
using System.IO;
using Aspose.Email;

class Program
{
    static void Main()
    {
        try
        {
            string mhtmlPath = "input.mht";
            string outputVcfPath = "output.vcf";

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

                Console.Error.WriteLine($"Input file '{mhtmlPath}' does not exist.");
                return;
            }

            string outputDirectory = Path.GetDirectoryName(outputVcfPath);
            if (!string.IsNullOrEmpty(outputDirectory) && !Directory.Exists(outputDirectory))
            {
                Directory.CreateDirectory(outputDirectory);
            }

            using (MailMessage message = MailMessage.Load(mhtmlPath))
            {
                foreach (Attachment attachment in message.Attachments)
                {
                    string attachmentName = attachment.Name;
                    string mediaType = attachment.ContentType.MediaType;

                    if (string.Equals(mediaType, "text/vcard", StringComparison.OrdinalIgnoreCase) ||
                        (attachmentName != null && attachmentName.EndsWith(".vcf", StringComparison.OrdinalIgnoreCase)))
                    {
                        using (Stream attachmentStream = attachment.ContentStream)
                        {
                            using (FileStream fileStream = new FileStream(outputVcfPath, FileMode.Create, FileAccess.Write))
                            {
                                attachmentStream.CopyTo(fileStream);
                            }
                        }

                        Console.WriteLine($"vCard extracted to '{outputVcfPath}'.");
                        return;
                    }
                }

                Console.Error.WriteLine("No vCard attachment found in the MHTML file.");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
