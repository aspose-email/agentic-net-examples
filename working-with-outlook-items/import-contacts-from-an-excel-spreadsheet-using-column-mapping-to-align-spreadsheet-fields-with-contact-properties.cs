using System;
using System.IO;
using System.Collections.Generic;
using System.Net;
using Aspose.Email;
using Aspose.Email.PersonalInfo;
using Aspose.Email.Clients.Exchange.Dav;

class Program
{
    static void Main()
    {
        try
        {
            // Paths
            string csvPath = "contacts.csv";

            // Ensure CSV file exists; create a minimal placeholder if missing
            if (!File.Exists(csvPath))
            {
                try
                {
                    using (var writer = new StreamWriter(csvPath))
                    {
                        writer.WriteLine("FirstName,LastName,Email,Phone");
                        writer.WriteLine("John,Doe,john.doe@example.com,1234567890");
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create placeholder CSV: {ex.Message}");
                    return;
                }
            }

            // Read CSV lines
            List<string[]> rows = new List<string[]>();
            try
            {
                using (var reader = new StreamReader(csvPath))
                {
                    string headerLine = reader.ReadLine(); // skip header
                    while (!reader.EndOfStream)
                    {
                        string line = reader.ReadLine();
                        if (string.IsNullOrWhiteSpace(line)) continue;
                        string[] columns = line.Split(',');
                        if (columns.Length >= 4)
                            rows.Add(columns);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error reading CSV file: {ex.Message}");
                return;
            }

            // Placeholder Exchange connection details
            string mailboxUri = "https://exchange.example.com/ews/Exchange.asmx";
            string username = "user@example.com";
            string password = "password";

            // Guard against placeholder credentials to avoid real network calls
            if (mailboxUri.Contains("example.com") || username.Contains("example.com"))
            {
                Console.WriteLine("Placeholder Exchange credentials detected. Skipping actual contact upload.");
                return;
            }

            // Create Exchange client and upload contacts
            try
            {
                using (ExchangeClient client = new ExchangeClient(mailboxUri, new NetworkCredential(username, password)))
                {
                    foreach (var columns in rows)
                    {
                        string firstName = columns[0].Trim();
                        string lastName = columns[1].Trim();
                        string email = columns[2].Trim();
                        string phone = columns[3].Trim();

                        // Build Contact object
                        Contact contact = new Contact
                        {
                            GivenName = firstName,
                            Surname = lastName
                        };
                        contact.EmailAddresses.Add(new EmailAddress(email));
                        contact.PhoneNumbers.Add(new PhoneNumber
                        {
                            Number = phone,
                            Category = PhoneNumberCategory.Company
                        });

                        // Create contact on Exchange server
                        try
                        {
                            string contactUri = client.CreateContact(contact);
                            Console.WriteLine($"Created contact: {firstName} {lastName} -> {contactUri}");
                        }
                        catch (Exception ex)
                        {
                            Console.Error.WriteLine($"Failed to create contact {firstName} {lastName}: {ex.Message}");
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
