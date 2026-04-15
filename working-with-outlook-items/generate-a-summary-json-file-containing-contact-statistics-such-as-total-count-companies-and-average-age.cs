using Aspose.Email.PersonalInfo;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using Aspose.Email;

class Program
{
    static void Main()
    {
        try
        {
            // Prepare a mock list of contacts using Aspose.Email.Contact
            List<Contact> contacts = new List<Contact>();
            List<int> ages = new List<int>();

            // Contact 1
            Contact contact1 = new Contact();
            contact1.GivenName = "John";
            contact1.Surname = "Doe";
            contact1.CompanyName = "Acme Corp";
            contacts.Add(contact1);
            ages.Add(30);

            // Contact 2
            Contact contact2 = new Contact();
            contact2.GivenName = "Jane";
            contact2.Surname = "Smith";
            contact2.CompanyName = "Beta Ltd";
            contacts.Add(contact2);
            ages.Add(25);

            // Contact 3
            Contact contact3 = new Contact();
            contact3.GivenName = "Bob";
            contact3.Surname = "Brown";
            contact3.CompanyName = "Acme Corp";
            contacts.Add(contact3);
            ages.Add(40);

            // Compute statistics
            int totalCount = contacts.Count;
            var distinctCompanies = contacts
                .Select(c => c.CompanyName)
                .Where(name => !string.IsNullOrEmpty(name))
                .Distinct()
                .ToList();
            double averageAge = ages.Count > 0 ? ages.Average() : 0.0;

            // Build summary object
            var summary = new
            {
                TotalCount = totalCount,
                Companies = distinctCompanies,
                AverageAge = averageAge
            };

            // Define output directory and file
            string outputDir = "output";
            string outputPath = Path.Combine(outputDir, "contact_summary.json");

            // Ensure output directory exists
            if (!Directory.Exists(outputDir))
            {
                Directory.CreateDirectory(outputDir);
            }

            // Serialize to JSON and write to file with error handling
            try
            {
                string json = JsonSerializer.Serialize(summary, new JsonSerializerOptions { WriteIndented = true });
                File.WriteAllText(outputPath, json);
                Console.WriteLine($"Contact summary written to: {outputPath}");
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to write summary file: {ex.Message}");
                return;
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
            return;
        }
    }
}
