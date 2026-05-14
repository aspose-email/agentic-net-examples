using System;
using System.IO;
using System.Collections.Generic;
using Aspose.Email;
using Aspose.Email.Mapi;

class Program
{
    static void Main()
    {
        try
        {
            // Define output directory for exported contacts
            string outputDirectory = "ExportedContacts";

            // Ensure the output directory exists
            if (!Directory.Exists(outputDirectory))
            {
                Directory.CreateDirectory(outputDirectory);
            }

            // Prepare a list of contacts with custom user‑defined fields
            List<MapiContact> contacts = new List<MapiContact>();

            // First contact
            MapiContact contact1 = new MapiContact();
            contact1.NameInfo.DisplayName = "John Doe";
            contact1.ElectronicAddresses.Email1.EmailAddress = "john.doe@example.com";

            // Set custom user‑defined fields via OtherFields property
            MapiContactOtherPropertySet otherFields1 = new MapiContactOtherPropertySet();
            otherFields1.UserField1 = "CustomValue1";
            otherFields1.UserField2 = "CustomValue2";
            contact1.OtherFields = otherFields1;

            contacts.Add(contact1);

            // Second contact
            MapiContact contact2 = new MapiContact();
            contact2.NameInfo.DisplayName = "Jane Smith";
            contact2.ElectronicAddresses.Email1.EmailAddress = "jane.smith@example.com";

            MapiContactOtherPropertySet otherFields2 = new MapiContactOtherPropertySet();
            otherFields2.UserField1 = "AnotherCustom1";
            otherFields2.UserField3 = "AnotherCustom3";
            contact2.OtherFields = otherFields2;

            contacts.Add(contact2);

            // Export each contact to a VCard file, ensuring custom fields are included
            int index = 1;
            foreach (MapiContact mapiContact in contacts)
            {
                string filePath = Path.Combine(outputDirectory, $"Contact_{index}.vcf");

                // Guard file write operation
                try
                {
                    using (FileStream fileStream = new FileStream(filePath, FileMode.Create, FileAccess.Write))
                    {
                        // Save the contact using VCard format; custom fields are preserved
                        mapiContact.Save(fileStream);
                    }

                    Console.WriteLine($"Exported contact {index} to '{filePath}'.");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to export contact {index}: {ex.Message}");
                }

                index++;
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
