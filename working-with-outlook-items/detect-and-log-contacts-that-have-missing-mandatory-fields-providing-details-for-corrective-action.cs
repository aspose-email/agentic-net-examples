using Aspose.Email;
using Aspose.Email.Clients.Google;
using Aspose.Email.PersonalInfo;
using System;
using System.Net;

class Program
{
    static void Main()
    {
        try
        {
            // Placeholder credentials – replace with real values.
            string clientId = "YOUR_CLIENT_ID";
            string clientSecret = "YOUR_CLIENT_SECRET";
            string refreshToken = "YOUR_REFRESH_TOKEN";

            // Guard against placeholder literals.
            if (clientId.Contains("YOUR_") || clientSecret.Contains("YOUR_") || refreshToken.Contains("YOUR_"))
            {
                Console.Error.WriteLine("Please provide valid Gmail client credentials before running the sample.");
                return;
            }

            // Create Gmail client.
            IGmailClient client;
            try
            {
                // The second argument is a proxy; pass null if not needed.
                client = GmailClient.GetInstance(clientId, null, clientSecret, refreshToken);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to create Gmail client: {ex.Message}");
                return;
            }

            // Fetch all contacts.
            Contact[] contacts;
            try
            {
                contacts = client.GetAllContacts();
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error retrieving contacts: {ex.Message}");
                return;
            }

            // Analyze contacts for missing mandatory fields.
            foreach (Contact contact in contacts)
            {
                // Assume Email and DisplayName are mandatory.
                bool missingEmail = contact.EmailAddresses == null || contact.EmailAddresses.Count == 0;
                bool missingDisplayName = string.IsNullOrWhiteSpace(contact.DisplayName);

                if (missingEmail || missingDisplayName)
                {
                    Console.WriteLine($"Contact ID: {contact.Id}");
                    if (missingEmail)
                        Console.WriteLine("  - Missing Email");
                    if (missingDisplayName)
                        Console.WriteLine("  - Missing Display Name");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
