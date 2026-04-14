using System;
using System.IO;
using System.Collections.Generic;
using Aspose.Email;
using Aspose.Email.PersonalInfo;

class Program
{
    static void Main()
    {
        try
        {
            // Define output file path
            string outputPath = "contacts.txt";

            // Ensure the directory for the output file exists
            string outputDir = Path.GetDirectoryName(outputPath);
            if (!string.IsNullOrEmpty(outputDir) && !Directory.Exists(outputDir))
            {
                Directory.CreateDirectory(outputDir);
            }

            // Prepare a list of contacts
            List<Contact> contacts = new List<Contact>();

            // Create first contact
            Contact contact1 = new Contact
            {
                DisplayName = "John Doe",
                CompanyName = "Acme Corp"
            };
            // Add email address
            contact1.EmailAddresses.Add(new EmailAddress("john.doe@example.com"));
            // Add phone number using property initialization (no 2‑arg constructor)
            contact1.PhoneNumbers.Add(new PhoneNumber
            {
                Number = "1234567890",
                Category = PhoneNumberCategory.Work
            });
            contacts.Add(contact1);

            // Create second contact
            Contact contact2 = new Contact
            {
                DisplayName = "Jane Smith",
                CompanyName = "Globex Inc"
            };
            contact2.EmailAddresses.Add(new EmailAddress("jane.smith@example.com"));
            contact2.PhoneNumbers.Add(new PhoneNumber
            {
                Number = "0987654321",
                Category = PhoneNumberCategory.Mobile
            });
            contacts.Add(contact2);

            // Write contacts to a fixed‑width text file
            using (StreamWriter writer = new StreamWriter(outputPath))
            {
                // Header line
                writer.WriteLine(
                    "Name".PadRight(30) +
                    "Company".PadRight(30) +
                    "Email".PadRight(30) +
                    "Phone".PadRight(15));

                // Data lines
                foreach (Contact c in contacts)
                {
                    string name = c.DisplayName ?? string.Empty;
                    string company = c.CompanyName ?? string.Empty;
                    string email = c.EmailAddresses.Count > 0 ? c.EmailAddresses[0].Address : string.Empty;
                    string phone = c.PhoneNumbers.Count > 0 ? c.PhoneNumbers[0].Number : string.Empty;

                    writer.WriteLine(
                        name.PadRight(30) +
                        company.PadRight(30) +
                        email.PadRight(30) +
                        phone.PadRight(15));
                }
            }

            Console.WriteLine("Contacts exported successfully to " + outputPath);
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine("Error: " + ex.Message);
        }
    }
}
