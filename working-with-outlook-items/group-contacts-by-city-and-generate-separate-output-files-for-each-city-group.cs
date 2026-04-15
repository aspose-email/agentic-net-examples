using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Aspose.Email;
using Aspose.Email.PersonalInfo;

class Program
{
    static void Main()
    {
        try
        {
            // Prepare a sample list of contacts.
            List<Contact> contacts = new List<Contact>
            {
                new Contact { GivenName = "John", Surname = "Doe", Location = "NewYork", EmailAddresses = { new EmailAddress("john.doe@example.com") } },
                new Contact { GivenName = "Jane", Surname = "Smith", Location = "London", EmailAddresses = { new EmailAddress("jane.smith@example.co.uk") } },
                new Contact { GivenName = "Alice", Surname = "Brown", Location = "NewYork", EmailAddresses = { new EmailAddress("alice.brown@example.com") } },
                new Contact { GivenName = "Bob", Surname = "White", Location = "Paris", EmailAddresses = { new EmailAddress("bob.white@example.fr") } }
            };

            // Group contacts by city (Location property).
            var groups = contacts.GroupBy(c => string.IsNullOrWhiteSpace(c.Location) ? "Unknown" : c.Location);

            // Base output directory.
            string outputRoot = "OutputContacts";

            // Ensure the base directory exists.
            try
            {
                if (!Directory.Exists(outputRoot))
                {
                    Directory.CreateDirectory(outputRoot);
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to create base output directory '{outputRoot}': {ex.Message}");
                return;
            }

            // Process each city group.
            foreach (var cityGroup in groups)
            {
                string cityFolder = Path.Combine(outputRoot, cityGroup.Key);
                try
                {
                    if (!Directory.Exists(cityFolder))
                    {
                        Directory.CreateDirectory(cityFolder);
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create directory for city '{cityGroup.Key}': {ex.Message}");
                    continue;
                }

                foreach (var contact in cityGroup)
                {
                    // Build a safe file name.
                    string fileName = $"{contact.GivenName}_{contact.Surname}.vcf".Replace(Path.GetInvalidFileNameChars(), '_');
                    string filePath = Path.Combine(cityFolder, fileName);

                    try
                    {
                        // Save the contact as a vCard file.
                        contact.Save(filePath);
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Failed to save contact '{contact.GivenName} {contact.Surname}' to '{filePath}': {ex.Message}");
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}

// Extension method to replace invalid filename characters.
static class StringExtensions
{
    public static string Replace(this string str, char[] chars, char replacement)
    {
        foreach (char c in chars)
        {
            str = str.Replace(c, replacement);
        }
        return str;
    }
}
