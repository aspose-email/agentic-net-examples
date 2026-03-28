using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Mime;

class Program
{
    static void Main()
    {
        try
        {
            string emlPath = "sample.eml";
            string outputDirectory = "output";

            // Verify input EML file exists
            if (!File.Exists(emlPath))
            {
                Console.Error.WriteLine($"Input file not found: {emlPath}");
                return;
            }

            // Ensure output directory exists
            if (!Directory.Exists(outputDirectory))
            {
                try
                {
                    Directory.CreateDirectory(outputDirectory);
                }
                catch (Exception dirEx)
                {
                    Console.Error.WriteLine($"Failed to create output directory: {dirEx.Message}");
                    return;
                }
            }

            // Load the EML message
            using (MailMessage message = MailMessage.Load(emlPath))
            {
                bool vcardFound = false;

                // Iterate through attachments to find vCard files
                foreach (Attachment attachment in message.Attachments)
                {
                    string attachmentName = attachment.Name ?? string.Empty;
                    string mediaType = attachment.ContentType.MediaType ?? string.Empty;

                    bool isVCard = mediaType.Equals("text/vcard", StringComparison.OrdinalIgnoreCase) ||
                                   attachmentName.EndsWith(".vcf", StringComparison.OrdinalIgnoreCase);

                    if (isVCard)
                    {
                        vcardFound = true;
                        string vcfPath = Path.Combine(outputDirectory, string.IsNullOrEmpty(attachmentName) ? "contact.vcf" : attachmentName);

                        try
                        {
                            using (FileStream fileStream = new FileStream(vcfPath, FileMode.Create, FileAccess.Write))
                            {
                                // Ensure the attachment stream is at the beginning
                                if (attachment.ContentStream.CanSeek)
                                {
                                    attachment.ContentStream.Position = 0;
                                }
                                attachment.ContentStream.CopyTo(fileStream);
                            }

                            Console.WriteLine($"Exported vCard to: {vcfPath}");
                        }
                        catch (Exception ioEx)
                        {
                            Console.Error.WriteLine($"Failed to save vCard '{attachmentName}': {ioEx.Message}");
                        }
                    }
                }

                if (!vcardFound)
                {
                    Console.WriteLine("No vCard attachments were found in the EML message.");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"An error occurred: {ex.Message}");
        }
    }
}
