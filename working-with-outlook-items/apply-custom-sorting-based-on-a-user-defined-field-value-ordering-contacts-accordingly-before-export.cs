using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Aspose.Email;
using Aspose.Email.Mapi;

class Program
{
    static void Main()
    {
        try
        {
            // Define a custom MAPI property tag (example: 0x8000)
            const long CustomTag = 0x8000;

            // Prepare a list of contacts with a custom field value
            List<MapiContact> contacts = new List<MapiContact>();

            // Helper to create a contact with a custom field
            MapiContact CreateContact(string displayName, string email, string customValue)
            {
                MapiContact contact = new MapiContact(displayName, email);
                // Set the custom property (Unicode string)
                byte[] customBytes = Encoding.Unicode.GetBytes(customValue);
                contact.SetProperty(new MapiProperty(CustomTag, customBytes));
                return contact;
            }

            contacts.Add(CreateContact("Alice Johnson", "alice@example.com", "High"));
            contacts.Add(CreateContact("Bob Smith", "bob@example.com", "Medium"));
            contacts.Add(CreateContact("Charlie Davis", "charlie@example.com", "Low"));

            // Sort contacts by the custom field value (ascending)
            List<MapiContact> sortedContacts = contacts
                .OrderBy(c => c.GetPropertyString(CustomTag))
                .ToList();

            // Output directory for exported vCard files
            string outputDir = Path.Combine(Environment.CurrentDirectory, "ExportedContacts");
            if (!Directory.Exists(outputDir))
            {
                Directory.CreateDirectory(outputDir);
            }

            // Export each sorted contact to a vCard file
            foreach (MapiContact contact in sortedContacts)
            {
                string safeFileName = $"{Guid.NewGuid()}.vcf";
                string filePath = Path.Combine(outputDir, safeFileName);

                try
                {
                    // Save the contact as vCard
                    contact.Save(filePath);
                    Console.WriteLine($"Exported: {contact.NameInfo.DisplayName} -> {filePath}");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to export contact '{contact.NameInfo.DisplayName}': {ex.Message}");
                }
                finally
                {
                    // Dispose the contact instance
                    contact.Dispose();
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
