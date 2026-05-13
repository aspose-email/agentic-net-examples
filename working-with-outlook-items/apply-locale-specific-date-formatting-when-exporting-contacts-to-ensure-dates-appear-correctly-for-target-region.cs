using Aspose.Email.PersonalInfo;
using System;
using System.Globalization;
using System.IO;
using Aspose.Email;

class Program
{
    static void Main()
    {
        try
        {
            // Define output directory and file
            string outputDirectory = Path.Combine(Environment.CurrentDirectory, "Output");
            string outputPath = Path.Combine(outputDirectory, "contact.vcf");

            // Ensure the output directory exists
            if (!Directory.Exists(outputDirectory))
            {
                Directory.CreateDirectory(outputDirectory);
            }

            // Create a contact and set some properties
            Contact contact = new Contact();
            contact.GivenName = "John";
            contact.Surname = "Doe";
            contact.EmailAddresses.Add(new EmailAddress("john.doe@example.com"));
            contact.CompanyName = "Example Corp";

            // Example date: birthday
            DateTime birthday = new DateTime(1990, 5, 23);

            // Apply locale‑specific formatting (e.g., French format)
            CultureInfo targetCulture = new CultureInfo("fr-FR");
            string formattedBirthday = birthday.ToString("d", targetCulture); // short date pattern

            // Store the formatted date in the Notes field
            contact.Notes = $"Birthday: {formattedBirthday}";

            // Save the contact to a vCard file
            try
            {
                contact.Save(outputPath);
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
