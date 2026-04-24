using Aspose.Email.PersonalInfo;
using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Clients.Exchange.Dav;

class Program
{
    static void Main()
    {
        try
        {
            // Placeholder connection details
            string mailboxUri = "https://exchange.example.com/ews/Exchange.asmx";
            string username = "username";
            string password = "password";

            // Skip actual network call when placeholders are used
            if (mailboxUri.Contains("example.com"))
            {
                Console.Error.WriteLine("Placeholder Exchange server detected. Skipping execution.");
                return;
            }

            // CSV file path
            string csvPath = "contacts.csv";

            // Ensure CSV file exists; create minimal placeholder if missing
            if (!File.Exists(csvPath))
            {
                try
                {
                    using (var writer = new StreamWriter(csvPath))
                    {
                        writer.WriteLine("Name,Email");
                        writer.WriteLine("John Doe,john.doe@example.com");
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create placeholder CSV: {ex.Message}");
                    return;
                }
            }

            // Create Exchange client
            using (ExchangeClient client = new ExchangeClient(mailboxUri, username, password))
            {
                try
                {
                    // Read CSV and import contacts
                    using (var reader = new StreamReader(csvPath))
                    {
                        string headerLine = reader.ReadLine(); // Skip header
                        string line;
                        while ((line = reader.ReadLine()) != null)
                        {
                            if (string.IsNullOrWhiteSpace(line))
                                continue;

                            string[] parts = line.Split(',');
                            if (parts.Length < 2)
                                continue;

                            string name = parts[0].Trim();
                            string email = parts[1].Trim();

                            // Build Contact object
                            Contact contact = new Contact();
                            contact.DisplayName = name;
                            contact.EmailAddresses.Add(new EmailAddress(email));

                            // Create contact on the server
                            try
                            {
                                client.CreateContact(contact);
                            }
                            catch (Exception ex)
                            {
                                Console.Error.WriteLine($"Failed to create contact '{name}': {ex.Message}");
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error during contact import: {ex.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
