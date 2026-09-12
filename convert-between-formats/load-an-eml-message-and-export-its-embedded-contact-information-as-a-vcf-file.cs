using Aspose.Email.PersonalInfo;
using System;
using System.IO;
using Aspose.Email;

class Program
{
    static void Main()
    {
        try
        {
            // Input EML file path
            const string sourcePath = "source.eml";
            // Output VCF file path (will be overwritten if exists)
            const string outputVcfPath = "contact.vcf";

            // Verify the source EML file exists
            if (!File.Exists(sourcePath))
            {
                try
                {
                    using (MailMessage placeholder = new MailMessage(
                        "sender@example.com",
                        "recipient@example.com",
                        "Placeholder Subject",
                        "Placeholder body."))
                    {
                        placeholder.Save(sourcePath, SaveOptions.DefaultEml);
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error creating placeholder message: {ex.Message}");
                    return;
                }

                Console.Error.WriteLine($"Source EML file not found: {sourcePath}");
                return;
            }

            // Load the EML message
            using (MailMessage mailMessage = MailMessage.Load(sourcePath))
            {
                bool vcfFound = false;

                // Iterate through attachments to find a VCF (vCard) file
                foreach (Attachment attachment in mailMessage.Attachments)
                {
                    // Check file extension or content type for vCard
                    if (attachment.Name != null &&
                        attachment.Name.EndsWith(".vcf", StringComparison.OrdinalIgnoreCase))
                    {
                        // Ensure the output directory exists
                        string outputDir = Path.GetDirectoryName(outputVcfPath);
                        if (!string.IsNullOrEmpty(outputDir) && !Directory.Exists(outputDir))
                        {
                            Directory.CreateDirectory(outputDir);
                        }

                        // Save the attachment content as a VCF file
                        using (FileStream fs = new FileStream(outputVcfPath, FileMode.Create, FileAccess.Write))
                        {
                            attachment.ContentStream.CopyTo(fs);
                        }

                        Console.WriteLine($"Contact exported to: {outputVcfPath}");
                        vcfFound = true;
                        break; // Assuming only one contact needed
                    }
                }

                if (!vcfFound)
                {
                    Console.Error.WriteLine("No embedded VCF attachment found in the EML message.");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
