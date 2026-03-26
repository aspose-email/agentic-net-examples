using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Mapi;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            // Input EML file path
            string emlPath = "input.eml";
            // Output VCF file path
            string vcfPath = "contact.vcf";

            // Verify input file exists
            if (!File.Exists(emlPath))
            {
                Console.Error.WriteLine($"Error: File not found – {emlPath}");
                return;
            }

            // Load the EML message with default options
            using (MailMessage mailMessage = MailMessage.Load(emlPath, new EmlLoadOptions()))
            {
                // Convert the MailMessage to a MapiMessage
                using (MapiMessage mapiMessage = MapiMessage.FromMailMessage(mailMessage))
                {
                    // Check if the MapiMessage represents a contact
                    if (mapiMessage.SupportedType == MapiItemType.Contact)
                    {
                        // Convert to MapiContact
                        MapiContact contact = (MapiContact)mapiMessage.ToMapiMessageItem();

                        // Save the contact as VCF
                        try
                        {
                            contact.Save(vcfPath);
                            Console.WriteLine($"Contact saved to {vcfPath}");
                        }
                        catch (Exception ex)
                        {
                            Console.Error.WriteLine($"Error saving VCF: {ex.Message}");
                        }
                        finally
                        {
                            if (contact != null)
                                contact.Dispose();
                        }
                    }
                    else
                    {
                        Console.Error.WriteLine("The loaded message does not contain a contact.");
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