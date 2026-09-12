using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Storage.Mbox;
using Aspose.Email.PersonalInfo.VCard;

class Program
{
    static void Main()
    {
        try
        {
            const string mboxPath = "storage.mbox";
            const string outputFolder = "ExtractedVCards";

            // Verify input MBOX file exists
            if (!File.Exists(mboxPath))
            {
                Console.Error.WriteLine($"Input MBOX file not found: {mboxPath}");
                return;
            }

            // Ensure output directory exists
            try
            {
                if (!Directory.Exists(outputFolder))
                {
                    Directory.CreateDirectory(outputFolder);
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to create output directory: {ex.Message}");
                return;
            }

            // Create MBOX reader with load options
            MboxStorageReader mboxReader = MboxStorageReader.CreateReader(mboxPath, new MboxLoadOptions());

            // Iterate through messages using ReadNextMessage()
            while (true)
            {
                MailMessage mailMessage = mboxReader.ReadNextMessage();
                if (mailMessage == null)
                {
                    break; // No more messages
                }

                try
                {
                    // Process each attachment that is a VCF file
                    foreach (Attachment attachment in mailMessage.Attachments)
                    {
                        if (attachment.Name != null && attachment.Name.EndsWith(".vcf", StringComparison.OrdinalIgnoreCase))
                        {
                            // Load VCardContact from attachment stream
                            using (Stream vcardStream = attachment.ContentStream)
                            {
                                VCardContact vcard = VCardContact.Load(vcardStream);
                                // Determine output file path
                                string safeFileName = Path.GetFileNameWithoutExtension(attachment.Name);
                                if (string.IsNullOrWhiteSpace(safeFileName))
                                {
                                    safeFileName = Guid.NewGuid().ToString();
                                }
                                string outputPath = Path.Combine(outputFolder, safeFileName + ".vcf");

                                // Save VCardContact as VCF file
                                vcard.Save(outputPath);
                                Console.WriteLine($"Saved VCF: {outputPath}");
                            }
                        }
                    }
                }
                finally
                {
                    // Dispose the mail message
                    mailMessage.Dispose();
                }
            }

            // Dispose the MBOX reader
            mboxReader.Dispose();
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
