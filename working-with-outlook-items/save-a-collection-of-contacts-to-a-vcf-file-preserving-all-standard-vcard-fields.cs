using Aspose.Email.PersonalInfo;
using System;
using System.Collections.Generic;
using System.IO;
using Aspose.Email;

class Program
{
    static void Main()
    {
        try
        {
            // Define output directory for VCF files
            string outputDirectory = Path.Combine(Directory.GetCurrentDirectory(), "VCF_Output");
            if (!Directory.Exists(outputDirectory))
            {
                Directory.CreateDirectory(outputDirectory);
            }

            // Create a collection of contacts
            List<Contact> contacts = new List<Contact>();

            // First contact
            Contact contact1 = new Contact();
            contact1.GivenName = "John";
            contact1.Surname = "Doe";
            contact1.CompanyName = "Acme Corp";
            contact1.JobTitle = "Software Engineer";
            contact1.EmailAddresses.Add(new EmailAddress("john.doe@acme.com"));
            contact1.PhoneNumbers.Add(new PhoneNumber { Number = "+1-555-0100", Category = PhoneNumberCategory.Company });
            contacts.Add(contact1);

            // Second contact
            Contact contact2 = new Contact();
            contact2.GivenName = "Jane";
            contact2.Surname = "Smith";
            contact2.CompanyName = "Beta Ltd";
            contact2.JobTitle = "Project Manager";
            contact2.EmailAddresses.Add(new EmailAddress("jane.smith@beta.com"));
            contact2.PhoneNumbers.Add(new PhoneNumber { Number = "+1-555-0200", Category = PhoneNumberCategory.Home });
            contacts.Add(contact2);

            // Save each contact to a separate VCF file
            for (int i = 0; i < contacts.Count; i++)
            {
                string vcfPath = Path.Combine(outputDirectory, $"contact_{i + 1}.vcf");
                try
                {
                    contacts[i].Save(vcfPath);
                    Console.WriteLine($"Saved contact to: {vcfPath}");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to save contact {i + 1}: {ex.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
