using Aspose.Email.PersonalInfo;
using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Mapi;

namespace AsposeEmailExamples
{
    // Custom data model for contact information
    public class ContactInfo
    {
        public string DisplayName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
    }

    public class Program
    {
        public static void Main()
        {
            try
            {
                // Path to the VCF file containing the Outlook contact
                string vcfPath = "contact.vcf";

                // Ensure the VCF file exists; create a minimal placeholder if it does not
                if (!File.Exists(vcfPath))
                {
                    try
                    {
                        using (StreamWriter writer = new StreamWriter(vcfPath, false))
                        {
                            writer.WriteLine("BEGIN:VCARD");
                            writer.WriteLine("VERSION:3.0");
                            writer.WriteLine("FN:John Doe");
                            writer.WriteLine("EMAIL:john.doe@example.com");
                            writer.WriteLine("TEL;TYPE=HOME:1234567890");
                            writer.WriteLine("END:VCARD");
                        }
                        Console.WriteLine($"Placeholder VCF file created at '{vcfPath}'.");
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Failed to create placeholder VCF file: {ex.Message}");
                        return;
                    }
                }

                // Load the contact from the VCF file
                MapiContact mapiContact;
                try
                {
                    mapiContact = MapiContact.FromVCard(vcfPath);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to load VCF file: {ex.Message}");
                    return;
                }

                // Use a using block to ensure the MapiContact is disposed
                using (mapiContact)
                {
                    // Map the MapiContact to the custom ContactInfo model
                    ContactInfo contactInfo = new ContactInfo
                    {
                        DisplayName = mapiContact.NameInfo?.DisplayName,
                        Email = mapiContact.ElectronicAddresses?.Email1?.EmailAddress,
                        Phone = mapiContact.Telephones?.HomeTelephoneNumber
                    };

                    // Output the mapped contact information
                    Console.WriteLine("Contact Information:");
                    Console.WriteLine($"Display Name: {contactInfo.DisplayName}");
                    Console.WriteLine($"Email: {contactInfo.Email}");
                    Console.WriteLine($"Phone: {contactInfo.Phone}");
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Unexpected error: {ex.Message}");
            }
        }
    }
}
