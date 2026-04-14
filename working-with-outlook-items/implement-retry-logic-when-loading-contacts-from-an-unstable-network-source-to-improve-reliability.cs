using System;
using System.Net;
using System.Threading;
using Aspose.Email;
using Aspose.Email.PersonalInfo;
using Aspose.Email.Clients.Google;

class Program
{
    static void Main()
    {
        try
        {
            // Placeholder credentials – replace with real values.
            string clientId = "your-client-id";
            string clientSecret = "your-client-secret";

            // Skip execution when placeholder credentials are detected.
            if (clientId.StartsWith("your-") || clientSecret.StartsWith("your-"))
            {
                Console.Error.WriteLine("Gmail client credentials are placeholders. Skipping network call.");
                return;
            }

            const int maxRetries = 3;
            const int delayMilliseconds = 2000;
            int attempt = 0;
            bool success = false;

            while (attempt < maxRetries && !success)
            {
                attempt++;
                try
                {
                    // Create Gmail client instance.
                    using (IGmailClient gmailClient = GmailClient.GetInstance(clientId, clientSecret))
                    {
                        // Fetch all contacts.
                        Contact[] contacts = gmailClient.GetAllContacts();

                        Console.WriteLine($"Retrieved {contacts.Length} contacts:");
                        foreach (Contact contact in contacts)
                        {
                            // Example: display the primary email address if available.
                            if (contact.EmailAddresses != null && contact.EmailAddresses.Count > 0)
                            {
                                Console.WriteLine(contact.EmailAddresses[0].Address);
                            }
                        }
                    }

                    success = true; // If we reach here, the operation succeeded.
                }
                catch (WebException webEx)
                {
                    Console.Error.WriteLine($"Attempt {attempt} failed: {webEx.Message}");
                    if (attempt < maxRetries)
                    {
                        Thread.Sleep(delayMilliseconds);
                        Console.WriteLine("Retrying...");
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Unexpected error on attempt {attempt}: {ex.Message}");
                    // Do not retry on unknown exceptions.
                    break;
                }
            }

            if (!success)
            {
                Console.Error.WriteLine("Failed to load contacts after multiple attempts.");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unhandled exception: {ex.Message}");
        }
    }
}
