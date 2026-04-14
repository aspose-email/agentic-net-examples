using Aspose.Email.PersonalInfo;
using System;
using System.Net;
using Aspose.Email;
using Aspose.Email.Clients.Exchange.WebService;

class Program
{
    static void Main()
    {
        try
        {
            // EWS endpoint and credentials
            string mailboxUri = "https://mail.example.com/EWS/Exchange.asmx";
            string username = "user@example.com";
            string password = "password";


            // Skip external calls when placeholder credentials are used
            if (mailboxUri.Contains("example.com") || username.Contains("example.com") || password == "password")
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping external calls.");
                return;
            }

            // Create the EWS client
            IEWSClient client;
            try
            {
                client = EWSClient.GetEWSClient(mailboxUri, username, password);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to create EWS client: {ex.Message}");
                return;
            }

            using (client)
            {
                // Retrieve mailboxes
                Contact[] mailboxes;
                try
                {
                    mailboxes = client.GetMailboxes();
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to retrieve mailboxes: {ex.Message}");
                    return;
                }

                // Display mailbox email addresses
                foreach (Contact contact in mailboxes)
                {
                    if (contact.EmailAddresses != null && contact.EmailAddresses.Count > 0)
                    {
                        Console.WriteLine(contact.EmailAddresses[0].Address);
                    }
                    else
                    {
                        Console.WriteLine("No email address available");
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
