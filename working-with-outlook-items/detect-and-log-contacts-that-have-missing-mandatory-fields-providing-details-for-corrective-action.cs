using System;
using System.Collections.Generic;
using Aspose.Email;
using Aspose.Email.Clients.Google;
using Aspose.Email.PersonalInfo;

class Program
{
    static void Main()
    {
        try
        {
            // Placeholder credentials – replace with real values.
            string accessToken = "YOUR_ACCESS_TOKEN";
            string defaultEmail = "user@example.com";

            // Guard against placeholder credentials to avoid live network calls.
            if (accessToken.Contains("YOUR_") || defaultEmail.Contains("example.com"))
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping Gmail operations.");
                return;
            }

            // Create Gmail client.
            IGmailClient gmailClient = GmailClient.GetInstance(accessToken, defaultEmail);

            try
            {
                // Retrieve all contacts.
                Contact[] contacts = gmailClient.GetAllContacts();

                foreach (Contact contact in contacts)
                {
                    List<string> missingFields = new List<string>();

                    // Check mandatory fields.
                    if (contact.EmailAddresses == null || contact.EmailAddresses.Count == 0)
                        missingFields.Add("EmailAddresses");

                    if (string.IsNullOrWhiteSpace(contact.GivenName))
                        missingFields.Add("GivenName");

                    if (string.IsNullOrWhiteSpace(contact.Surname))
                        missingFields.Add("Surname");

                    if (missingFields.Count > 0)
                    {
                        Console.WriteLine($"Contact '{contact.DisplayName}' is missing mandatory fields: {string.Join(", ", missingFields)}");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Gmail operation failed: {ex.Message}");
                return;
            }
            finally
            {
                // Ensure the client is disposed.
                if (gmailClient is IDisposable disposableClient)
                    disposableClient.Dispose();
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
