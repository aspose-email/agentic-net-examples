using Aspose.Email;
using System;
using System.IO;
using System.Text.RegularExpressions;
using Aspose.Email.Mapi;

class Program
{
    static void Main()
    {
        try
        {
            // Simple email validation regex
            var emailRegex = new Regex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", RegexOptions.Compiled);

            // Create sample contacts (replace with real PST loading if needed)
            var contacts = new[]
            {
                new MapiContact("John", "Doe")
                {
                    ElectronicAddresses =
                    {
                        Email1 = new MapiContactElectronicAddress("john.doe@example.com"),
                        Email2 = new MapiContactElectronicAddress("invalid-email@@example..com")
                    }
                },
                new MapiContact("Jane", "Smith")
                {
                    ElectronicAddresses =
                    {
                        Email1 = new MapiContactElectronicAddress("jane.smith@domain.com")
                    }
                }
            };

            // Validate each contact's email addresses
            foreach (var contact in contacts)
            {
                ValidateElectronicAddress(contact.ElectronicAddresses.Email1, "Email1", emailRegex);
                ValidateElectronicAddress(contact.ElectronicAddresses.Email2, "Email2", emailRegex);
                ValidateElectronicAddress(contact.ElectronicAddresses.Email3, "Email3", emailRegex);
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }

    private static void ValidateElectronicAddress(MapiContactElectronicAddress address, string label, Regex regex)
    {
        if (address != null && !address.IsEmpty)
        {
            string email = address.EmailAddress;
            if (!string.IsNullOrEmpty(email) && !regex.IsMatch(email))
            {
                Console.WriteLine($"Invalid {label}: {email}");
            }
        }
    }
}
