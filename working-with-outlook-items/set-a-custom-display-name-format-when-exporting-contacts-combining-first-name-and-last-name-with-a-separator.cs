using Aspose.Email.PersonalInfo;
using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Mapi;

class Program
{
    static void Main()
    {
        try
        {
            // Define output directory and file
            string outputDir = Path.Combine(Environment.CurrentDirectory, "Output");
            string outputPath = Path.Combine(outputDir, "custom_contact.vcf");

            // Ensure the output directory exists
            if (!Directory.Exists(outputDir))
            {
                Directory.CreateDirectory(outputDir);
            }

            // Create a new contact and set its properties
            Contact contact = new Contact();
            contact.GivenName = "John";
            contact.Surname = "Doe";

            // Custom display name format: FirstName | LastName
            string separator = " | ";
            contact.DisplayName = $"{contact.GivenName}{separator}{contact.Surname}";

            // Save the contact as VCard using the explicit ContactSaveFormat enum
            try
            {
                contact.Save(outputPath, ContactSaveFormat.VCard);
                Console.WriteLine($"Contact saved to: {outputPath}");
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to save contact: {ex.Message}");
                return;
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
