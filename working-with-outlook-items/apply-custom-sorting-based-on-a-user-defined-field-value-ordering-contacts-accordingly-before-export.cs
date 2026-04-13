using System;
using System.Collections.Generic;
using System.IO;
using Aspose.Email;
using Aspose.Email.PersonalInfo;

class Program
{
    static void Main()
    {
        try
        {
            // Define output folder for exported vCard files
            string outputFolder = "ExportedContacts";
            try
            {
                // Ensure the output directory exists
                if (!Directory.Exists(outputFolder))
                {
                    Directory.CreateDirectory(outputFolder);
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to prepare output folder: {ex.Message}");
                return;
            }

            // Create a sample list of contacts (placeholder data)
            List<Contact> contacts = new List<Contact>();

            Contact contact1 = new Contact();
            contact1.DisplayName = "Alice Johnson";
            contact1.EmailAddresses.Add(new EmailAddress("alice@example.com"));
            contact1.FileAs = "Johnson, Alice";
            contacts.Add(contact1);

            Contact contact2 = new Contact();
            contact2.DisplayName = "Bob Smith";
            contact2.EmailAddresses.Add(new EmailAddress("bob@example.com"));
            contact2.FileAs = "Smith, Bob";
            contacts.Add(contact2);

            Contact contact3 = new Contact();
            contact3.DisplayName = "Charlie Davis";
            contact3.EmailAddresses.Add(new EmailAddress("charlie@example.com"));
            contact3.FileAs = "Davis, Charlie";
            contacts.Add(contact3);

            // Sort contacts based on the user‑defined FileAs field
            contacts.Sort((c1, c2) => string.Compare(c1.FileAs, c2.FileAs, StringComparison.Ordinal));

            // Export each contact to a vCard file
            foreach (Contact c in contacts)
            {
                string safeFileName = $"{c.DisplayName.Replace(' ', '_')}.vcf";
                string filePath = Path.Combine(outputFolder, safeFileName);
                try
                {
                    c.Save(filePath);
                    Console.WriteLine($"Exported: {filePath}");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to export contact '{c.DisplayName}': {ex.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
