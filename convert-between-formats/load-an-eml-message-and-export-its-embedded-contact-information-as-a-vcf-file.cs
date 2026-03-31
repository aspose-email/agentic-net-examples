using System;
using System.IO;
using System.Text;
using Aspose.Email;
using Aspose.Email.Mapi;

class Program
{
    static void Main()
    {
        try
        {
            string emlPath = "input.eml";
            string vcfPath = "output.vcf";

            // Ensure input EML exists; create a minimal placeholder if missing
            if (!File.Exists(emlPath))
            {
                try
                {
                    string placeholderEml = "From: sender@example.com\r\nTo: recipient@example.com\r\nSubject: Sample\r\n\r\nThis is a placeholder email.";
                    File.WriteAllText(emlPath, placeholderEml, Encoding.UTF8);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create placeholder EML: {ex.Message}");
                    return;
                }
            }

            // Ensure output directory exists
            string outputDir = Path.GetDirectoryName(vcfPath);
            if (!string.IsNullOrEmpty(outputDir) && !Directory.Exists(outputDir))
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

            // Load the EML message
            MailMessage mailMessage;
            try
            {
                mailMessage = MailMessage.Load(emlPath, new EmlLoadOptions());
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to load EML file: {ex.Message}");
                return;
            }

            using (mailMessage)
            {
                // Convert to MAPI message
                MapiMessage mapiMessage;
                try
                {
                    mapiMessage = MapiMessage.FromMailMessage(mailMessage);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to convert to MAPI message: {ex.Message}");
                    return;
                }

                using (mapiMessage)
                {
                    // Check if the message contains a contact
                    if (mapiMessage.SupportedType == MapiItemType.Contact)
                    {
                        // Extract the contact
                        MapiContact mapiContact;
                        try
                        {
                            mapiContact = (MapiContact)mapiMessage.ToMapiMessageItem();
                        }
                        catch (Exception ex)
                        {
                            Console.Error.WriteLine($"Failed to extract contact: {ex.Message}");
                            return;
                        }

                        using (mapiContact)
                        {
                            try
                            {
                                // Save contact as VCF
                                mapiContact.Save(vcfPath);
                                Console.WriteLine($"Contact saved to VCF: {vcfPath}");
                            }
                            catch (Exception ex)
                            {
                                Console.Error.WriteLine($"Failed to save VCF file: {ex.Message}");
                            }
                        }
                    }
                    else
                    {
                        // Not a contact – create a minimal placeholder VCF
                        try
                        {
                            string placeholderVcf = "BEGIN:VCARD\r\nVERSION:2.1\r\nEND:VCARD\r\n";
                            File.WriteAllText(vcfPath, placeholderVcf, Encoding.UTF8);
                            Console.WriteLine($"Placeholder VCF created at: {vcfPath}");
                        }
                        catch (Exception ex)
                        {
                            Console.Error.WriteLine($"Failed to create placeholder VCF: {ex.Message}");
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
