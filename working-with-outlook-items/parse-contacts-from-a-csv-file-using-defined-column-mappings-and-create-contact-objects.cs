using System;
using System.IO;
using System.Collections.Generic;
using Aspose.Email;
using Aspose.Email.PersonalInfo;

class Program
{
    static void Main()
    {
        try
        {
            // Define input CSV and output directory paths
            string csvPath = "contacts.csv";
            string outputDir = "OutputVcards";

            // Ensure the CSV file exists; create a minimal placeholder if missing
            if (!File.Exists(csvPath))
            {
                try
                {
                    using (var writer = new StreamWriter(csvPath))
                    {
                        writer.WriteLine("GivenName,Surname,Email");
                        writer.WriteLine("John,Doe,john.doe@example.com");
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create placeholder CSV: {ex.Message}");
                    return;
                }
            }

            // Ensure the output directory exists
            try
            {
                if (!Directory.Exists(outputDir))
                {
                    Directory.CreateDirectory(outputDir);
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to create output directory: {ex.Message}");
                return;
            }

            // Read all lines from the CSV
            string[] lines;
            try
            {
                lines = File.ReadAllLines(csvPath);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to read CSV file: {ex.Message}");
                return;
            }

            // Parse contacts (skip header)
            var contacts = new List<Contact>();
            for (int i = 1; i < lines.Length; i++)
            {
                string line = lines[i];
                if (string.IsNullOrWhiteSpace(line))
                    continue;

                string[] parts = line.Split(',');
                if (parts.Length < 3)
                    continue; // insufficient data

                string givenName = parts[0].Trim();
                string surname = parts[1].Trim();
                string email = parts[2].Trim();

                var contact = new Contact
                {
                    GivenName = givenName,
                    Surname = surname
                };
                contact.EmailAddresses.Add(new EmailAddress(email));

                contacts.Add(contact);
            }

            // Save each contact as a vCard file
            foreach (Contact contact in contacts)
            {
                string fileName = $"{contact.GivenName}_{contact.Surname}.vcf";
                string vcardPath = Path.Combine(outputDir, fileName);

                try
                {
                    contact.Save(vcardPath);
                    Console.WriteLine($"Saved vCard: {vcardPath}");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to save vCard for {contact.GivenName} {contact.Surname}: {ex.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
