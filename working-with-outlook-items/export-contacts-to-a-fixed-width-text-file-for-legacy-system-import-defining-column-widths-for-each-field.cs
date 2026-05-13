using Aspose.Email.PersonalInfo;
using Aspose.Email;
using System;
using System.IO;
using System.Collections.Generic;
using Aspose.Email.Mapi;

namespace ExportContactsFixedWidth
{
    class Program
    {
        static void Main()
        {
            try
            {
                // Define output file path
                string outputPath = "contacts_fixed_width.txt";

                // Ensure the directory exists
                string outputDir = Path.GetDirectoryName(Path.GetFullPath(outputPath));
                if (!Directory.Exists(outputDir))
                {
                    Directory.CreateDirectory(outputDir);
                }

                // Prepare a list of contacts (sample data)
                List<MapiContact> contacts = new List<MapiContact>();

                // Contact 1
                MapiContact contact1 = new MapiContact();
                contact1.NameInfo = new MapiContactNamePropertySet
                {
                    DisplayName = "John Doe",
                    GivenName = "John",
                    Surname = "Doe"
                };
                contact1.ElectronicAddresses = new MapiContactElectronicAddressPropertySet
                {
                    Email1 = new MapiContactElectronicAddress { EmailAddress = "john.doe@example.com" }
                };
                contact1.Telephones = new MapiContactTelephonePropertySet
                {
                    BusinessTelephoneNumber = "555-1234"
                };
                contacts.Add(contact1);

                // Contact 2
                MapiContact contact2 = new MapiContact();
                contact2.NameInfo = new MapiContactNamePropertySet
                {
                    DisplayName = "Jane Smith",
                    GivenName = "Jane",
                    Surname = "Smith"
                };
                contact2.ElectronicAddresses = new MapiContactElectronicAddressPropertySet
                {
                    Email1 = new MapiContactElectronicAddress { EmailAddress = "jane.smith@example.org" }
                };
                contact2.Telephones = new MapiContactTelephonePropertySet
                {
                    BusinessTelephoneNumber = "555-9876"
                };
                contacts.Add(contact2);

                // Define column widths
                int nameWidth = 30;
                int emailWidth = 30;
                int phoneWidth = 15;

                // Write contacts to fixed‑width file
                using (StreamWriter writer = new StreamWriter(outputPath, false))
                {
                    // Header line
                    string header = PadRight("Name", nameWidth) +
                                    PadRight("Email", emailWidth) +
                                    PadRight("Phone", phoneWidth);
                    writer.WriteLine(header);
                    writer.WriteLine(new string('-', nameWidth + emailWidth + phoneWidth));

                    foreach (MapiContact c in contacts)
                    {
                        string name = c.NameInfo?.DisplayName ?? string.Empty;
                        string email = c.ElectronicAddresses?.Email1?.EmailAddress ?? string.Empty;
                        string phone = c.Telephones?.BusinessTelephoneNumber ?? string.Empty;

                        string line = PadRight(name, nameWidth) +
                                      PadRight(email, emailWidth) +
                                      PadRight(phone, phoneWidth);
                        writer.WriteLine(line);
                    }
                }

                Console.WriteLine($"Contacts exported successfully to '{outputPath}'.");
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error: {ex.Message}");
                return;
            }
        }

        // Helper method for fixed‑width padding
        private static string PadRight(string text, int totalWidth)
        {
            if (text == null) text = string.Empty;
            if (text.Length > totalWidth)
                return text.Substring(0, totalWidth);
            return text.PadRight(totalWidth);
        }
    }
}
