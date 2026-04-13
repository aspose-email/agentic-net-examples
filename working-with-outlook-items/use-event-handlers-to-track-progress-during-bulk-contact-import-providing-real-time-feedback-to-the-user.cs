using System;
using System.Collections.Generic;
using System.Net;
using Aspose.Email;
using Aspose.Email.PersonalInfo;
using Aspose.Email.Clients.Exchange.Dav;

namespace BulkContactImportSample
{
    // Event arguments for progress reporting
    public class ProgressEventArgs : EventArgs
    {
        public int Imported { get; }
        public int Total { get; }

        public ProgressEventArgs(int imported, int total)
        {
            Imported = imported;
            Total = total;
        }
    }

    // Helper class that performs bulk import and raises progress events
    public class BulkContactImporter
    {
        private readonly ExchangeClient _client;

        public BulkContactImporter(ExchangeClient client)
        {
            _client = client;
        }

        // Progress event
        public event EventHandler<ProgressEventArgs> ProgressChanged;

        // Imports a collection of contacts
        public void ImportContacts(List<Contact> contacts)
        {
            if (contacts == null) throw new ArgumentNullException(nameof(contacts));

            int count = 0;
            foreach (Contact contact in contacts)
            {
                // Create contact on the Exchange server
                _client.CreateContact(contact);
                count++;

                // Raise progress event
                ProgressChanged?.Invoke(this, new ProgressEventArgs(count, contacts.Count));
            }
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // Placeholder connection details
                string mailboxUri = "https://exchange.example.com/ews/Exchange.asmx";
                string username = "user@example.com";
                string password = "password";

                // Skip real network call when placeholders are detected
                if (mailboxUri.Contains("example.com"))
                {
                    Console.WriteLine("Placeholder credentials detected – skipping actual import.");
                    return;
                }

                // Create Exchange client inside a using block
                try
                {
                    using (ExchangeClient client = new ExchangeClient(mailboxUri, username, password))
                    {
                        // Prepare a sample list of contacts to import
                        List<Contact> contacts = new List<Contact>
                        {
                            new Contact
                            {
                                GivenName = "John",
                                Surname = "Doe",
                                EmailAddresses = { new EmailAddress("john.doe@example.com", "John Doe") }
                            },
                            new Contact
                            {
                                GivenName = "Jane",
                                Surname = "Smith",
                                EmailAddresses = { new EmailAddress("jane.smith@example.com", "Jane Smith") }
                            }
                        };

                        // Instantiate importer and subscribe to progress events
                        BulkContactImporter importer = new BulkContactImporter(client);
                        importer.ProgressChanged += (sender, e) =>
                        {
                            Console.WriteLine($"Imported {e.Imported} of {e.Total} contacts.");
                        };

                        // Perform bulk import
                        importer.ImportContacts(contacts);
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Exchange operation failed: {ex.Message}");
                    return;
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Unexpected error: {ex.Message}");
            }
        }
    }
}
