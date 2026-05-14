using Aspose.Email;
using System;
using System.Collections.Generic;
using System.IO;
using Aspose.Email.Mapi;

class Program
{
    static void Main()
    {
        try
        {
            // Define output directory for MSG files
            string outputDirectory = "ContactsOutput";

            // Ensure the output directory exists
            if (!Directory.Exists(outputDirectory))
                Directory.CreateDirectory(outputDirectory);

            // Prepare a list of contacts to be saved
            List<MapiContact> contacts = new List<MapiContact>();

            // First contact
            var contact1 = new MapiContact();
            var nameInfo1 = new MapiContactNamePropertySet
            {
                DisplayName = "John Doe"
            };
            contact1.NameInfo = nameInfo1;
            contact1.ElectronicAddresses.Email1 = new MapiContactElectronicAddress
            {
                EmailAddress = "john.doe@example.com"
            };
            contacts.Add(contact1);

            // Second contact
            var contact2 = new MapiContact();
            var nameInfo2 = new MapiContactNamePropertySet
            {
                DisplayName = "Jane Smith"
            };
            contact2.NameInfo = nameInfo2;
            contact2.ElectronicAddresses.Email1 = new MapiContactElectronicAddress
            {
                EmailAddress = "jane.smith@example.com"
            };
            contacts.Add(contact2);

            // Save each contact as an MSG file using default Outlook formatting
            foreach (var contact in contacts)
            {
                try
                {
                    using (contact)
                    {
                        string fileName = $"{contact.NameInfo.DisplayName}.msg";
                        string filePath = Path.Combine(outputDirectory, fileName);
                        contact.Save(filePath);
                        Console.WriteLine($"Saved contact '{contact.NameInfo.DisplayName}' to '{filePath}'.");
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error saving contact '{contact.NameInfo?.DisplayName}': {ex.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
