using Aspose.Email.PersonalInfo;
using Aspose.Email;
using System;
using System.IO;
using Aspose.Email.Mapi;

class Program
{
    static void Main()
    {
        try
        {
            // Output directory for exported contacts
            string outputDir = "ExportedContacts";
            if (!Directory.Exists(outputDir))
            {
                Directory.CreateDirectory(outputDir);
            }

            // Create a MAPI contact and set basic properties
            using (MapiContact contact = new MapiContact())
            {
                // Set display name via NameInfo
                contact.NameInfo = new MapiContactNamePropertySet
                {
                    DisplayName = "John Doe"
                };

                // Set primary email address
                contact.ElectronicAddresses.Email1 = new MapiContactElectronicAddress("john.doe@example.com");

                // Build a timestamped file name
                string timestamp = DateTime.Now.ToString("yyyyMMddHHmmssfff");
                string filePath = Path.Combine(outputDir, $"Contact_{timestamp}.vcf");

                // Save the contact to a VCF file
                try
                {
                    contact.Save(filePath);
                    Console.WriteLine($"Contact saved to {filePath}");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to save contact: {ex.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
