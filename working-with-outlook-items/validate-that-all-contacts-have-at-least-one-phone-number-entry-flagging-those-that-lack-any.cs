using Aspose.Email.PersonalInfo;
using System;
using Aspose.Email;
using Aspose.Email.Clients.Google;

class Program
{
    static void Main()
    {
        try
        {
            // Placeholder credentials – replace with real values or skip execution.
            string accessToken = "YOUR_ACCESS_TOKEN";
            string defaultEmail = "user@example.com";

            if (string.IsNullOrWhiteSpace(accessToken) || accessToken == "YOUR_ACCESS_TOKEN")
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping contact validation.");
                return;
            }

            // Create Gmail client instance.
            IGmailClient gmailClient = GmailClient.GetInstance(accessToken, defaultEmail);

            try
            {
                // Retrieve all contacts.
                Contact[] contacts = gmailClient.GetAllContacts();

                if (contacts == null || contacts.Length == 0)
                {
                    Console.WriteLine("No contacts found.");
                    return;
                }

                // Validate each contact for at least one phone number.
                foreach (Contact contact in contacts)
                {
                    // PhoneNumbers is a collection of PhoneNumber objects.
                    int phoneCount = contact.PhoneNumbers?.Count ?? 0;

                    if (phoneCount == 0)
                    {
                        Console.WriteLine($"Contact '{contact.DisplayName}' (Email: {contact.EmailAddresses?.ToString() ?? "N/A"}) has no phone numbers.");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error while processing contacts: {ex.Message}");
            }
            finally
            {
                // Ensure the client is disposed.
                if (gmailClient is IDisposable disposableClient)
                {
                    disposableClient.Dispose();
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
