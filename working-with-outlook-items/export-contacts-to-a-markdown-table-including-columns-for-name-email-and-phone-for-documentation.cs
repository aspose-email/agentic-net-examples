using System;
using System.Collections.Generic;
using System.IO;
using Aspose.Email;
using Aspose.Email.Mapi;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            // Define output markdown file path
            string outputPath = "contacts.md";

            // Ensure the directory for the output file exists
            string outputDir = Path.GetDirectoryName(outputPath);
            if (!string.IsNullOrEmpty(outputDir) && !Directory.Exists(outputDir))
            {
                try
                {
                    Directory.CreateDirectory(outputDir);
                }
                catch (Exception dirEx)
                {
                    Console.Error.WriteLine($"Failed to create directory '{outputDir}': {dirEx.Message}");
                    return;
                }
            }

            // Prepare a list of contacts
            List<MapiContact> contacts = new List<MapiContact>();

            // First contact
            using (MapiContact contact1 = new MapiContact())
            {
                contact1.NameInfo.DisplayName = "John Doe";
                contact1.ElectronicAddresses.Email1 = new MapiContactElectronicAddress("john.doe@example.com");
                contact1.Telephones.BusinessTelephoneNumber = "123-456-7890";
                contacts.Add(contact1);
            }

            // Second contact
            using (MapiContact contact2 = new MapiContact())
            {
                contact2.NameInfo.DisplayName = "Jane Smith";
                contact2.ElectronicAddresses.Email1 = new MapiContactElectronicAddress("jane.smith@example.com");
                contact2.Telephones.MobileTelephoneNumber = "555-123-4567";
                contacts.Add(contact2);
            }

            // Write contacts to markdown file
            try
            {
                using (StreamWriter writer = new StreamWriter(outputPath, false))
                {
                    // Write table header
                    writer.WriteLine("| Name | Email | Phone |");
                    writer.WriteLine("|------|-------|-------|");

                    // Write each contact as a table row
                    foreach (MapiContact contact in contacts)
                    {
                        string name = contact.NameInfo?.DisplayName ?? string.Empty;
                        string email = contact.ElectronicAddresses?.Email1?.EmailAddress ?? string.Empty;
                        string phone = contact.Telephones?.BusinessTelephoneNumber ?? contact.Telephones?.MobileTelephoneNumber ?? string.Empty;

                        writer.WriteLine($"| {name} | {email} | {phone} |");
                    }
                }
            }
            catch (Exception ioEx)
            {
                Console.Error.WriteLine($"Error writing markdown file: {ioEx.Message}");
                return;
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
