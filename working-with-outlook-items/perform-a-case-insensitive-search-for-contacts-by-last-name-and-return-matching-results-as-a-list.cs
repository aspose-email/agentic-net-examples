using Aspose.Email;
using Aspose.Email.Clients.Exchange.WebService;
using Aspose.Email.PersonalInfo;
using System;
using System.Collections.Generic;

class Program
{
    static void Main()
    {
        try
        {
            // Placeholder credentials – replace with real values.
            string host = "https://exchange.example.com/EWS/Exchange.asmx";
            string username = "user@example.com";
            string password = "password";

            // Guard against placeholder credentials to avoid live network calls in CI.
            if (string.IsNullOrWhiteSpace(host) ||
                host.Contains("example.com", StringComparison.OrdinalIgnoreCase) ||
                username.Contains("example.com", StringComparison.OrdinalIgnoreCase) ||
                password == "password")
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping contact search.");
                return;
            }

            // The last name to search for (case‑insensitive).
            string searchLastName = "Smith";

            // Create and use the EWS client.
            using (IEWSClient client = EWSClient.GetEWSClient(host, username, password))
            {
                // Retrieve contacts from the default Contacts folder.
                Contact[] allContacts = client.GetContacts("contacts");

                // Filter contacts by last name (case‑insensitive).
                List<Contact> matchingContacts = new List<Contact>();
                foreach (Contact contact in allContacts)
                {
                    // In Aspose.Email, the property for last name is Surname.
                    if (!string.IsNullOrEmpty(contact.Surname) &&
                        string.Equals(contact.Surname, searchLastName, StringComparison.OrdinalIgnoreCase))
                    {
                        matchingContacts.Add(contact);
                    }
                }

                // Output the results.
                Console.WriteLine($"Found {matchingContacts.Count} contact(s) with last name \"{searchLastName}\":");
                foreach (Contact contact in matchingContacts)
                {
                    string email = contact.EmailAddresses.Count > 0 ? contact.EmailAddresses[0].Address : "No email";
                    Console.WriteLine($"- {contact.DisplayName} ({email})");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unhandled exception: {ex.Message}");
        }
    }
}
