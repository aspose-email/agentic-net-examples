using Aspose.Email.Clients.Exchange.WebService;
using Aspose.Email.PersonalInfo;
using System;
using System.IO;
using Aspose.Email;
class Program
{
    static void Main(string[] args)
    {
        try
        {
            // Path to the CSV file containing contacts
            string csvPath = "contacts.csv";

            // Verify that the CSV file exists
            if (!File.Exists(csvPath))
            {
                Console.Error.WriteLine($"CSV file not found: {csvPath}");
                return;
            }

            // Read all lines from the CSV file
            string[] lines = File.ReadAllLines(csvPath);
            if (lines.Length == 0)
            {
                Console.Error.WriteLine("CSV file is empty.");
                return;
            }

            // Create and configure the Exchange client
            // Replace the URI, username, and password with actual values
            string mailboxUri = "https://exchange.example.com/EWS/Exchange.asmx";
            string username = "username";
            string password = "password";


            // Skip external calls when placeholder credentials are used
            if (mailboxUri.Contains("example.com") || username == "username" || password == "password")
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping external calls.");
                return;
            }

            using (IEWSClient client = EWSClient.GetEWSClient(mailboxUri, username, password))
            {
                // Iterate over CSV rows (skip header if present)
                for (int i = 1; i < lines.Length; i++)
                {
                    string line = lines[i];
                    if (string.IsNullOrWhiteSpace(line))
                        continue;

                    // Expected CSV format: DisplayName,Email,Phone
                    string[] fields = line.Split(',');
                    if (fields.Length < 3)
                        continue;

                    string displayName = fields[0].Trim();
                    string email = fields[1].Trim();
                    string phone = fields[2].Trim();

                    // Build a Contact object
                    Contact contact = new Contact
                    {
                        DisplayName = displayName
                    };
                    contact.EmailAddresses.Add(new EmailAddress(email));
                    contact.PhoneNumbers.Add(new PhoneNumber
                    {
                        Number = phone,
                        Category = PhoneNumberCategory.Company
                    });

                    // Create the contact in the Exchange address book
                    client.CreateContact(contact);
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
