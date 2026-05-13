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
            string outputDir = "Output";
            if (!Directory.Exists(outputDir))
            {
                Directory.CreateDirectory(outputDir);
            }

            string contactPath = Path.Combine(outputDir, "contact.vcf");

            using (MapiContact contact = new MapiContact())
            {
                // Set first name and last name
                contact.NameInfo.GivenName = "John";
                contact.NameInfo.Surname = "Doe";

                // Custom display name format: FirstName - LastName
                contact.NameInfo.DisplayName = $"{contact.NameInfo.GivenName} - {contact.NameInfo.Surname}";

                try
                {
                    contact.Save(contactPath);
                    Console.WriteLine($"Contact saved to {contactPath}");
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
