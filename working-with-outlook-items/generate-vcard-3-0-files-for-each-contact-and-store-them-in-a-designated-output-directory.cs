using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Mapi;
using Aspose.Email.PersonalInfo.VCard;

class Program
{
    static void Main()
    {
        try
        {
            // Define output directory for vCard files
            string outputDir = Path.Combine(Directory.GetCurrentDirectory(), "OutputVcards");
            if (!Directory.Exists(outputDir))
            {
                Directory.CreateDirectory(outputDir);
            }

            // Sample contacts to be saved as vCard 3.0
            MapiContact[] contacts = new MapiContact[2];

            // First contact
            MapiContact contact1 = new MapiContact();
            contact1.NameInfo.DisplayName = "John Doe";
            contact1.ElectronicAddresses.Email1.EmailAddress = "john.doe@example.com";
            contact1.Telephones.BusinessTelephoneNumber = "+1-555-0100";
            contacts[0] = contact1;

            // Second contact
            MapiContact contact2 = new MapiContact();
            contact2.NameInfo.DisplayName = "Jane Smith";
            contact2.ElectronicAddresses.Email1.EmailAddress = "jane.smith@example.com";
            contact2.Telephones.HomeTelephoneNumber = "+1-555-0200";
            contacts[1] = contact2;

            // Prepare vCard save options (default version is 3.0)
            VCardSaveOptions saveOptions = new VCardSaveOptions();

            // Save each contact as a vCard file
            foreach (MapiContact contact in contacts)
            {
                using (contact)
                {
                    string safeFileName = string.Concat(contact.NameInfo.DisplayName.Split(Path.GetInvalidFileNameChars()));
                    string vcardPath = Path.Combine(outputDir, safeFileName + ".vcf");

                    if (File.Exists(vcardPath))
                    {
                        File.Delete(vcardPath);
                    }

                    contact.Save(vcardPath, saveOptions);
                    Console.WriteLine($"Saved vCard: {vcardPath}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
