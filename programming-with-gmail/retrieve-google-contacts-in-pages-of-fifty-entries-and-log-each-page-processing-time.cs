using Aspose.Email.PersonalInfo;
using System;
using System.Diagnostics;
using Aspose.Email;
using Aspose.Email.Clients.Google;

class Program
{
    static void Main()
    {
        try
        {
            // Placeholder credentials – replace with real values or skip execution.
            string clientId = "your_client_id";
            string clientSecret = "your_client_secret";
            string refreshToken = "your_refresh_token";
            string userEmail = "user@example.com";

            // Guard against placeholder credentials to avoid live network calls.
            if (clientId.StartsWith("your_") || clientSecret.StartsWith("your_") || refreshToken.StartsWith("your_"))
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping Gmail contacts retrieval.");
                return;
            }

            // Create Gmail client instance.
            IGmailClient gmailClient;
            try
            {
                gmailClient = GmailClient.GetInstance(clientId, clientSecret, refreshToken, userEmail);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to create Gmail client: {ex.Message}");
                return;
            }

            // Ensure the client is disposed properly.
            using (gmailClient)
            {
                Contact[] allContacts;
                try
                {
                    allContacts = gmailClient.GetAllContacts();
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error retrieving contacts: {ex.Message}");
                    return;
                }

                const int pageSize = 50;
                int pageNumber = 0;

                for (int i = 0; i < allContacts.Length; i += pageSize)
                {
                    pageNumber++;
                    int remaining = allContacts.Length - i;
                    int currentPageSize = remaining < pageSize ? remaining : pageSize;

                    Contact[] page = new Contact[currentPageSize];
                    Array.Copy(allContacts, i, page, 0, currentPageSize);

                    Stopwatch stopwatch = new Stopwatch();
                    stopwatch.Start();

                    // Example processing: log each contact's primary email address.
                    foreach (Contact contact in page)
                    {
                        if (contact.EmailAddresses.Count > 0)
                        {
                            Console.WriteLine(contact.EmailAddresses[0].Address);
                        }
                    }

                    stopwatch.Stop();
                    Console.WriteLine($"Page {pageNumber}: Processed {currentPageSize} contacts in {stopwatch.ElapsedMilliseconds} ms.");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
