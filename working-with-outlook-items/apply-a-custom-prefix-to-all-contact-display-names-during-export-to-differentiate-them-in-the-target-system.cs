using Aspose.Email.Mapi;
using System;
using System.Collections.Generic;
using Aspose.Email;
using Aspose.Email.PersonalInfo;
using Aspose.Email.Clients.Exchange.Dav;

namespace AsposeEmailContactPrefixExample
{
    class Program
    {
        static void Main()
        {
            try
            {
                // Placeholder credentials – skip real network call in CI environments
                string exchangeUri = "https://exchange.example.com/ews/Exchange.asmx";
                string username = "username";
                string password = "password";

                if (exchangeUri.Contains("example.com"))
                {
                    Console.Error.WriteLine("Placeholder Exchange URI detected. Skipping network operation.");
                    return;
                }

                // Connect to Exchange server
                using (ExchangeClient client = new ExchangeClient(exchangeUri, username, password))
                {
                    // List contacts from a folder (e.g., "Contacts")
                    string contactsFolderUri = "/contacts";
                    MapiContact[] mapiContacts;

                    try
                    {
                        mapiContacts = client.ListContacts(contactsFolderUri);
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Failed to list contacts: {ex.Message}");
                        return;
                    }

                    // Convert to Contact objects and apply custom prefix to DisplayName
                    List<Contact> contacts = new List<Contact>();
                    const string prefix = "CustomPrefix_";

                    foreach (MapiContact mapiContact in mapiContacts)
                    {
                        // Implicit conversion from MapiContact to Contact
                        Contact contact = (Contact)mapiContact;

                        // Apply prefix
                        if (!string.IsNullOrEmpty(contact.DisplayName))
                        {
                            contact.DisplayName = prefix + contact.DisplayName;
                        }
                        else
                        {
                            contact.DisplayName = prefix + "Unnamed";
                        }

                        contacts.Add(contact);
                    }

                    // Export contacts (example: write display names to console)
                    foreach (Contact contact in contacts)
                    {
                        Console.WriteLine($"Exported Contact: {contact.DisplayName}");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Unexpected error: {ex.Message}");
            }
        }
    }
}
