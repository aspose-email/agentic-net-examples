using System;
using System.Collections.Generic;
using System.Net;
using Aspose.Email;
using Aspose.Email.Clients.Exchange.WebService;
using Aspose.Email.PersonalInfo;

namespace BulkContactImportSample
{
    // Delegate for progress reporting
    public delegate void ProgressChangedHandler(int processed, int total);

    // Helper class to import contacts in bulk and raise progress events
    public class BulkContactImporter
    {
        private readonly IEWSClient _client;
        private readonly IList<Contact> _contacts;

        public event ProgressChangedHandler ProgressChanged;

        public BulkContactImporter(IEWSClient client, IList<Contact> contacts)
        {
            _client = client;
            _contacts = contacts;
        }

        public void Import()
        {
            int total = _contacts.Count;
            int processed = 0;

            foreach (Contact contact in _contacts)
            {
                try
                {
                    _client.CreateContact(contact);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to import contact '{contact.DisplayName}': {ex.Message}");
                }

                processed++;
                ProgressChanged?.Invoke(processed, total);
            }
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // Placeholder connection settings
                string exchangeUrl = "https://exchange.example.com/EWS/Exchange.asmx";
                string username = "user@example.com";
                string password = "password";

                // Guard against placeholder credentials to avoid real network calls
                if (exchangeUrl.Contains("example.com") || username.Contains("example.com"))
                {
                    Console.WriteLine("Placeholder credentials detected. Skipping actual import.");
                    return;
                }

                // Create the Exchange client
                using (IEWSClient client = EWSClient.GetEWSClient(exchangeUrl, new NetworkCredential(username, password)))
                {
                    // Prepare a list of contacts to import
                    List<Contact> contactsToImport = new List<Contact>
                    {
                        new Contact
                        {
                            DisplayName = "John Doe",
                            EmailAddresses = { new EmailAddress("john.doe@example.com", "John Doe") },
                            PhoneNumbers = { new PhoneNumber { Number = "+1234567890", Category = PhoneNumberCategory.Company } }
                        },
                        new Contact
                        {
                            DisplayName = "Jane Smith",
                            EmailAddresses = { new EmailAddress("jane.smith@example.com", "Jane Smith") },
                            PhoneNumbers = { new PhoneNumber { Number = "+1987654321", Category = PhoneNumberCategory.Company } }
                        }
                        // Add more contacts as needed
                    };

                    // Initialize the bulk importer and subscribe to progress events
                    BulkContactImporter importer = new BulkContactImporter(client, contactsToImport);
                    importer.ProgressChanged += (processed, total) =>
                    {
                        Console.WriteLine($"Imported {processed} of {total} contacts.");
                    };

                    // Start the import process
                    importer.Import();
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error: {ex.Message}");
            }
        }
    }
}
