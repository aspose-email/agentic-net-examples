using Aspose.Email.PersonalInfo;
using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Mapi;

class Program
{
    static void Main()
    {
        try
        {
            string vcardPath = "contact.vcf";
            string msgPath = "contact.msg";

            // Ensure the input VCF file exists; create a minimal placeholder if missing.
            if (!File.Exists(vcardPath))
            {
                try
                {
                    string placeholderVCard = "BEGIN:VCARD\r\nVERSION:2.1\r\nFN:John Doe\r\nEND:VCARD";
                    File.WriteAllText(vcardPath, placeholderVCard);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create placeholder VCF file: {ex.Message}");
                    return;
                }
            }

            // Load the contact from VCF, preserving categories and other properties.
            MapiContact mapiContact;
            try
            {
                mapiContact = MapiContact.FromVCard(vcardPath);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to load VCF contact: {ex.Message}");
                return;
            }

            // Ensure the loaded contact is disposed properly.
            using (mapiContact)
            {
                // Save the contact to MSG format, categories are retained automatically.
                try
                {
                    mapiContact.Save(msgPath);
                    Console.WriteLine($"Contact converted and saved to '{msgPath}'.");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to save MSG contact: {ex.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
