using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Storage.Mbox;

class Program
{
    static void Main()
    {
        try
        {
            // Input MBOX file path
            string mboxPath = "input.mbox";
            // Output directory for VCF files
            string outputDir = "vcf_output";

            // Guard input file existence
            if (!File.Exists(mboxPath))
            {
                Console.Error.WriteLine($"Input MBOX file not found: {mboxPath}");
                return;
            }

            // Ensure output directory exists
            if (!Directory.Exists(outputDir))
            {
                try
                {
                    Directory.CreateDirectory(outputDir);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create output directory: {ex.Message}");
                    return;
                }
            }

            // Open the MBOX reader
            using (MboxStorageReader mboxReader = MboxStorageReader.CreateReader(mboxPath, new MboxLoadOptions()))
            {
                while (true)
                {
                    // Read next message; returns null when no more messages
                    MailMessage message = mboxReader.ReadNextMessage();
                    if (message == null)
                        break;

                    // Process attachments that are VCF files
                    foreach (Attachment attachment in message.Attachments)
                    {
                        if (attachment.Name != null && attachment.Name.EndsWith(".vcf", StringComparison.OrdinalIgnoreCase))
                        {
                            string vcfPath = Path.Combine(outputDir, attachment.Name);
                            try
                            {
                                // Save the attachment directly to file
                                attachment.Save(vcfPath);
                                Console.WriteLine($"Saved VCF: {vcfPath}");
                            }
                            catch (Exception ex)
                            {
                                Console.Error.WriteLine($"Failed to save VCF '{vcfPath}': {ex.Message}");
                            }
                        }
                    }

                    // Optionally, handle inline vCard content in the body (if present)
                    if (!string.IsNullOrEmpty(message.Body) && message.Body.TrimStart().StartsWith("BEGIN:VCARD", StringComparison.OrdinalIgnoreCase))
                    {
                        string vcfFileName = $"message_{Guid.NewGuid()}.vcf";
                        string vcfPath = Path.Combine(outputDir, vcfFileName);
                        try
                        {
                            File.WriteAllText(vcfPath, message.Body);
                            Console.WriteLine($"Saved inline VCF: {vcfPath}");
                        }
                        catch (Exception ex)
                        {
                            Console.Error.WriteLine($"Failed to write inline VCF '{vcfPath}': {ex.Message}");
                        }
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
