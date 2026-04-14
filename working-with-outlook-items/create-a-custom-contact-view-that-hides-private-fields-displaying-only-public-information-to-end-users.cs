using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.PersonalInfo;

class Program
{
    static void Main()
    {
        try
        {
            // Define the output vCard file path
            string outputPath = Path.Combine(Environment.CurrentDirectory, "Contact.vcf");

            // Ensure the output directory exists
            string outputDir = Path.GetDirectoryName(outputPath);
            if (!Directory.Exists(outputDir))
            {
                Directory.CreateDirectory(outputDir);
            }

            // Create a new contact with public information only
            Contact contact = new Contact
            {
                GivenName = "John",
                Surname = "Doe",
                DisplayName = "John Doe",
                CompanyName = "Example Corp",
                JobTitle = "Software Engineer"
            };

            // Add public email address
            contact.EmailAddresses.Add(new EmailAddress("john.doe@example.com"));

            // Add public phone number using property initialization (no 2‑arg constructor)
            PhoneNumber phone = new PhoneNumber
            {
                Number = "555-1234",
                Category = PhoneNumberCategory.Company
            };
            contact.PhoneNumbers.Add(phone);

            // Save the contact as a vCard file
            try
            {
                contact.Save(outputPath);
                Console.WriteLine($"Contact saved to: {outputPath}");
            }
            catch (Exception ioEx)
            {
                Console.Error.WriteLine($"Failed to save contact: {ioEx.Message}");
                return;
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
