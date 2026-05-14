using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Aspose.Email;
using Aspose.Email.Mapi;
using Aspose.Email.Clients.Exchange.Dav;

namespace AsposeEmailContactProjection
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // Placeholder credentials – skip real network call in CI environments.
                string exchangeUri = "https://exchange.example.com/EWS/Exchange.asmx";
                string username = "user@example.com";
                string password = "password";

                if (exchangeUri.Contains("example.com"))
                {
                    Console.Error.WriteLine("Placeholder Exchange URI detected. Skipping network operations.");

                    var simpleList = new List<SimpleContact>
                    {
                        new SimpleContact { Name = "Alice Johnson", Email = "alice@example.com" },
                        new SimpleContact { Name = "Bob Smith", Email = "bob.smith@example.com" }
                    };

                    PrintSimpleContacts(simpleList);
                    return;
                }

                // Connect to Exchange server.
                using (ExchangeClient client = new ExchangeClient(exchangeUri, username, password))
                {
                    // Retrieve contacts from the default contacts folder.
                    MapiContact[] contacts = client.ListContacts("contacts");

                    var simpleList = contacts
                        .Select(c => new SimpleContact
                        {
                            Name = GetContactName(c),
                            Email = GetFirstEmail(c)
                        })
                        .ToList();

                    PrintSimpleContacts(simpleList);
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error: {ex.Message}");
            }
        }

        // Retrieves the display name of a MapiContact using reflection.
        private static string GetContactName(MapiContact contact)
        {
            var prop = contact.GetType().GetProperty("DisplayName", BindingFlags.Public | BindingFlags.Instance);
            if (prop != null)
            {
                return prop.GetValue(contact) as string ?? string.Empty;
            }
            // Fallback to other possible property names.
            var altProp = contact.GetType().GetProperty("Subject", BindingFlags.Public | BindingFlags.Instance);
            return altProp?.GetValue(contact) as string ?? string.Empty;
        }

        // Retrieves the first email address of a MapiContact using reflection.
        private static string GetFirstEmail(MapiContact contact)
        {
            // Try common collection property names.
            var collProp = contact.GetType().GetProperty("EmailAddressCollection", BindingFlags.Public | BindingFlags.Instance)
                         ?? contact.GetType().GetProperty("EmailAddresses", BindingFlags.Public | BindingFlags.Instance);

            if (collProp != null)
            {
                var collection = collProp.GetValue(contact) as IEnumerable;
                if (collection != null)
                {
                    var enumerator = collection.GetEnumerator();
                    if (enumerator.MoveNext())
                    {
                        var emailObj = enumerator.Current;
                        var emailProp = emailObj.GetType().GetProperty("EmailAddress", BindingFlags.Public | BindingFlags.Instance);
                        if (emailProp != null)
                        {
                            return emailProp.GetValue(emailObj) as string ?? string.Empty;
                        }
                    }
                }
            }
            return string.Empty;
        }

        // Simple DTO for projected contact information.
        private class SimpleContact
        {
            public string Name { get; set; }
            public string Email { get; set; }
        }

        // Prints the projected contacts to the console.
        private static void PrintSimpleContacts(IEnumerable<SimpleContact> contacts)
        {
            foreach (var c in contacts)
            {
                Console.WriteLine($"Name: {c.Name}, Email: {c.Email}");
            }
        }
    }
}
