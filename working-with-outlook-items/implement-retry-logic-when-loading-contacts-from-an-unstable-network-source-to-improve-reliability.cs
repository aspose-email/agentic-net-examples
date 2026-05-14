using System;
using System.Net;
using System.Threading;
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
            string clientId = "YOUR_CLIENT_ID";
            string clientSecret = "YOUR_CLIENT_SECRET";
            string refreshToken = "YOUR_REFRESH_TOKEN";

            // Guard against executing network calls with placeholder credentials.
            if (clientId.StartsWith("YOUR_") || clientSecret.StartsWith("YOUR_") || refreshToken.StartsWith("YOUR_"))
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping contact retrieval.");
                return;
            }

            // Create Gmail client instance. Pass null for proxy (no proxy).
            IGmailClient gmailClient = GmailClient.GetInstance(clientId, null, clientSecret, refreshToken);

            // Retry logic parameters.
            const int maxAttempts = 3;
            const int delayMilliseconds = 2000;
            Contact[] contacts = null;

            for (int attempt = 1; attempt <= maxAttempts; attempt++)
            {
                try
                {
                    contacts = gmailClient.GetAllContacts();
                    // Successful retrieval, exit retry loop.
                    break;
                }
                catch (WebException ex)
                {
                    Console.Error.WriteLine($"Attempt {attempt} failed: {ex.Message}");
                    if (attempt == maxAttempts)
                    {
                        Console.Error.WriteLine("Maximum retry attempts reached. Unable to load contacts.");
                        return;
                    }
                    // Wait before next retry.
                    Thread.Sleep(delayMilliseconds);
                }
            }

            // Process retrieved contacts.
            if (contacts != null && contacts.Length > 0)
            {
                Console.WriteLine($"Retrieved {contacts.Length} contacts:");
                foreach (Contact contact in contacts)
                {
                    string email = contact.EmailAddresses?.Count > 0 ? contact.EmailAddresses[0]?.Address : "No Email";
                    Console.WriteLine($"- {contact.GivenName} {contact.Surname} ({email})");
                }
            }
            else
            {
                Console.WriteLine("No contacts were retrieved.");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
