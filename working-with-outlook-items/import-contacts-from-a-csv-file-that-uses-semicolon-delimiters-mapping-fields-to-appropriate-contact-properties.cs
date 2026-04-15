using System;
using System.Collections.Generic;
using System.IO;
using Aspose.Email;
using Aspose.Email.PersonalInfo;
using Aspose.Email.Mapi;

class Program
{
    static void Main()
    {
        try
        {
            string csvPath = "contacts.csv";
            string outputDir = "OutputVcards";

            // Ensure CSV file exists; create a minimal placeholder if missing
            if (!File.Exists(csvPath))
            {
                try
                {
                    using (var writer = new StreamWriter(csvPath))
                    {
                        // Header: GivenName;Surname;Email;Phone
                        writer.WriteLine("GivenName;Surname;Email;Phone");
                        writer.WriteLine("John;Doe;john.doe@example.com;+1234567890");
                    }
                    Console.WriteLine($"Placeholder CSV created at '{csvPath}'.");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create placeholder CSV: {ex.Message}");
                    return;
                }
            }

            // Ensure output directory exists
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

            List<Contact> contacts = new List<Contact>();

            // Read and parse CSV
            try
            {
                using (var reader = new StreamReader(csvPath))
                {
                    string headerLine = reader.ReadLine(); // Skip header
                    if (headerLine == null)
                    {
                        Console.Error.WriteLine("CSV file is empty.");
                        return;
                    }

                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        if (string.IsNullOrWhiteSpace(line))
                            continue;

                        string[] parts = line.Split(';');
                        if (parts.Length < 4)
                            continue; // Skip malformed rows

                        string givenName = parts[0].Trim();
                        string surname = parts[1].Trim();
                        string email = parts[2].Trim();
                        string phone = parts[3].Trim();

                        Contact contact = new Contact
                        {
                            GivenName = givenName,
                            Surname = surname,
                            DisplayName = $"{givenName} {surname}"
                        };

                        // Add email address
                        if (!string.IsNullOrEmpty(email))
                        {
                            contact.EmailAddresses.Add(new EmailAddress(email));
                        }

                        // Add phone number
                        if (!string.IsNullOrEmpty(phone))
                        {
                            contact.PhoneNumbers.Add(new PhoneNumber
                            {
                                Number = phone,
                                Category = PhoneNumberCategory.Company
                            });
                        }

                        contacts.Add(contact);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error reading CSV file: {ex.Message}");
                return;
            }

            // Save each contact as a vCard file
            int index = 1;
            foreach (Contact contact in contacts)
            {
                string vcardPath = Path.Combine(outputDir, $"Contact_{index}.vcf");
                try
                {
                    // Save using the default vCard format
                    contact.Save(vcardPath, ContactSaveFormat.VCard);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to save contact '{contact.DisplayName}': {ex.Message}");
                }
                index++;
            }

            Console.WriteLine($"Imported {contacts.Count} contacts and saved to '{outputDir}'.");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
