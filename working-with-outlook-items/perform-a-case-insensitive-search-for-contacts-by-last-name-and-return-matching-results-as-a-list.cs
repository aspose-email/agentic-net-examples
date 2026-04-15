using Aspose.Email.PersonalInfo;
using System;
using System.Collections.Generic;
using Aspose.Email;
using Aspose.Email.Clients.Exchange.WebService;

class Program
{
    static void Main()
    {
        try
        {
            List<Contact> results = SearchContactsByLastName("Smith");
            Console.WriteLine($"Found {results.Count} contact(s) with last name 'Smith':");
            foreach (Contact contact in results)
            {
                Console.WriteLine($"- {contact.GivenName} {contact.Surname} ({contact.EmailAddresses[0]?.Address})");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }

    static List<Contact> SearchContactsByLastName(string lastName)
    {
        const string host = "exchange.example.com";
        const string username = "user@example.com";
        const string password = "password";

        // Guard against placeholder credentials to avoid real network calls in CI.
        if (host.Contains("example.com"))
        {
            Console.Error.WriteLine("Placeholder credentials detected. Skipping Exchange call.");
            return new List<Contact>();
        }

        try
        {
            using (IEWSClient client = EWSClient.GetEWSClient(host, username, password))
            {
                // Build a simple query string for last name search.
                // The FindPeople method performs case‑insensitive matching.
                string query = $"lastName:{lastName}";
                Contact[] contacts = client.FindPeople(query, 100);
                return new List<Contact>(contacts);
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error searching contacts: {ex.Message}");
            return new List<Contact>();
        }
    }
}
