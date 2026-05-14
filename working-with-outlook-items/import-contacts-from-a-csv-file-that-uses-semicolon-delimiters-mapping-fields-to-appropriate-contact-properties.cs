using System;
using System.IO;
using System.Collections.Generic;
using Aspose.Email;
using Aspose.Email.Clients.Exchange.Dav;
using Aspose.Email.PersonalInfo;

class Program
{
    static void Main()
    {
        try
        {
            // Paths and credentials (replace with real values)
            string csvPath = "contacts.csv";
            string mailboxUri = "https://exchange.example.com/ews/Exchange.asmx";
            string username = "user@example.com";
            string password = "password";

            // Guard against placeholder credentials/hosts
            if (mailboxUri.Contains("example.com") || username.Contains("example.com"))
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping execution.");
                return;
            }

            // Ensure CSV file exists
            if (!File.Exists(csvPath))
            {
                // Create minimal placeholder CSV
                try
                {
                    using (StreamWriter writer = new StreamWriter(csvPath))
                    {
                        writer.WriteLine("FirstName;LastName;Email;Phone");
                        writer.WriteLine("John;Doe;john.doe@example.com;1234567890");
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create placeholder CSV: {ex.Message}");
                    return;
                }
            }

            // Read and parse CSV
            List<Contact> contacts = new List<Contact>();
            try
            {
                using (StreamReader reader = new StreamReader(csvPath))
                {
                    string headerLine = reader.ReadLine(); // Skip header
                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        string[] parts = line.Split(';');
                        if (parts.Length < 4) continue; // Skip malformed lines

                        Contact contact = new Contact();
                        contact.GivenName = parts[0];
                        contact.Surname = parts[1];
                        contact.EmailAddresses.Add(new EmailAddress(parts[2]));
                        contact.PhoneNumbers.Add(new PhoneNumber { Number = parts[3], Category = PhoneNumberCategory.Company });
                        contacts.Add(contact);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error reading CSV file: {ex.Message}");
                return;
            }

            // Create contacts on Exchange server
            try
            {
                using (ExchangeClient client = new ExchangeClient(mailboxUri, username, password))
                {
                    foreach (Contact contact in contacts)
                    {
                        try
                        {
                            client.CreateContact(contact);
                            Console.WriteLine($"Created contact: {contact.GivenName} {contact.Surname}");
                        }
                        catch (Exception ex)
                        {
                            Console.Error.WriteLine($"Failed to create contact {contact.GivenName} {contact.Surname}: {ex.Message}");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Exchange client error: {ex.Message}");
                return;
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
