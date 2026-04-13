using System;
using System.Collections.Generic;
using System.IO;
using Aspose.Email;
using Aspose.Email.PersonalInfo;
using Aspose.Email.PersonalInfo.VCard;

class Program
{
    static void Main()
    {
        try
        {
            // Define output directory
            string outputDir = Path.Combine(Environment.CurrentDirectory, "VCardOutput");
            if (!Directory.Exists(outputDir))
            {
                Directory.CreateDirectory(outputDir);
            }

            // Prepare a list of contacts
            List<Contact> contacts = new List<Contact>();

            Contact contact1 = new Contact();
            contact1.GivenName = "John";
            contact1.Surname = "Doe";
            contact1.EmailAddresses.Add(new EmailAddress("john.doe@example.com"));
            contact1.PhoneNumbers.Add(new PhoneNumber { Number = "+1234567890", Category = PhoneNumberCategory.Company });
            contacts.Add(contact1);

            Contact contact2 = new Contact();
            contact2.GivenName = "Jane";
            contact2.Surname = "Smith";
            contact2.EmailAddresses.Add(new EmailAddress("jane.smith@example.com"));
            contact2.PhoneNumbers.Add(new PhoneNumber { Number = "+1987654321", Category = PhoneNumberCategory.Home });
            contacts.Add(contact2);

            // Save each contact as a VCard 3.0 file
            foreach (Contact c in contacts)
            {
                string fileName = $"{c.GivenName}_{c.Surname}.vcf";
                string filePath = Path.Combine(outputDir, fileName);

                try
                {
                    VCardSaveOptions saveOptions = new VCardSaveOptions(VCardVersion.V30);
                    c.Save(filePath, saveOptions);
                    Console.WriteLine($"Saved VCard: {filePath}");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to save VCard for {c.GivenName} {c.Surname}: {ex.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
