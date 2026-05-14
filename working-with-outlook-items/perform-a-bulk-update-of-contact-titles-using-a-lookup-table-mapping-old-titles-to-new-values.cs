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
            // Placeholder credentials and service URL
            string serviceUrl = "https://exchange.example.com/EWS/Exchange.asmx";
            string username = "user@example.com";
            string password = "password";

            // Guard against placeholder values to avoid real network calls
            if (serviceUrl.Contains("example.com") || username.Contains("example.com") || password == "password")
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping contact update operation.");
                return;
            }

            // Lookup table: old title -> new title
            var titleLookup = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
            {
                { "Senior Engineer", "Lead Engineer" },
                { "Assistant Manager", "Associate Manager" },
                { "Intern", "Junior Associate" }
            };

            // Create EWS client
            IEWSClient client;
            try
            {
                client = EWSClient.GetEWSClient(serviceUrl, username, password);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to create EWS client: {ex.Message}");
                return;
            }

            // Ensure client is disposed
            using (client as IDisposable)
            {
                // Retrieve contacts from the default contacts folder
                IEnumerable<Contact> contacts;
                try
                {
                    contacts = client.GetContacts("contacts");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to retrieve contacts: {ex.Message}");
                    return;
                }

                // Process each contact
                foreach (Contact contact in contacts)
                {
                    // Use JobTitle as the title field
                    string currentTitle = contact.JobTitle;

                    if (!string.IsNullOrEmpty(currentTitle) && titleLookup.TryGetValue(currentTitle, out string newTitle))
                    {
                        contact.JobTitle = newTitle;

                        // Save the updated contact back to Exchange
                        try
                        {
                            client.UpdateContact(contact);
                            Console.WriteLine($"Updated contact '{contact.DisplayName}' title from '{currentTitle}' to '{newTitle}'.");
                        }
                        catch (Exception ex)
                        {
                            Console.Error.WriteLine($"Failed to update contact '{contact.DisplayName}': {ex.Message}");
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
