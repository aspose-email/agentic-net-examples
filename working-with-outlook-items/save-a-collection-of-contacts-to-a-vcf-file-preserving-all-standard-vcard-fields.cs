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
            // Prepare output directory
            string outputDir = "ContactsVcf";
            try
            {
                if (!Directory.Exists(outputDir))
                {
                    Directory.CreateDirectory(outputDir);
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to prepare output directory: {ex.Message}");
                return;
            }

            // Create a collection of contacts
            List<Contact> contacts = new List<Contact>();

            // First contact
            Contact contact1 = new Contact
            {
                DisplayName = "John Doe",
                GivenName = "John",
                Surname = "Doe",
                CompanyName = "Acme Corp",
                JobTitle = "Software Engineer"
            };
            contact1.EmailAddresses.Add(new EmailAddress("john.doe@example.com"));
            contact1.PhoneNumbers.Add(new PhoneNumber { Number = "+1-555-0100", Category = PhoneNumberCategory.Company });
            contacts.Add(contact1);

            // Second contact
            Contact contact2 = new Contact
            {
                DisplayName = "Jane Smith",
                GivenName = "Jane",
                Surname = "Smith",
                CompanyName = "Beta Ltd",
                JobTitle = "Project Manager"
            };
            contact2.EmailAddresses.Add(new EmailAddress("jane.smith@beta.com"));
            contact2.PhoneNumbers.Add(new PhoneNumber { Number = "+1-555-0200", Category = PhoneNumberCategory.Company });
            contacts.Add(contact2);

            // Save each contact to a separate VCF file
            int index = 1;
            foreach (Contact c in contacts)
            {
                string filePath = Path.Combine(outputDir, $"Contact{index}.vcf");
                try
                {
                    c.Save(filePath);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to save contact #{index}: {ex.Message}");
                    // Continue with next contact
                }
                index++;
            }

            Console.WriteLine("Contacts have been saved successfully.");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
