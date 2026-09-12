using Aspose.Email;
using System;
using System.Net;
using Aspose.Email.Clients.Exchange;
using Aspose.Email.Clients.Exchange.WebService;

class Program
{
    static void Main()
    {
        try
        {
            // User's email address – replace with actual value.
            string userEmail = "user@example.com";

            // Create network credentials (username, password, domain).
            NetworkCredential credentials = new NetworkCredential("username", "password", "domain");

            // Guard: skip external calls when placeholder values are present.
            bool hasPlaceholder = userEmail.Contains("example.com") ||
                                  credentials.UserName.Equals("username", StringComparison.OrdinalIgnoreCase) ||
                                  credentials.Password.Equals("password", StringComparison.OrdinalIgnoreCase) ||
                                  credentials.Domain.Equals("domain", StringComparison.OrdinalIgnoreCase);

            if (hasPlaceholder)
            {
                Console.WriteLine("Placeholder credentials detected. Skipping network operations.");
                return;
            }

            // Resolve the EWS endpoint URL using AutoDiscover.
            // Aspose.Email provides an overload that performs autodiscover when only the email and credentials are supplied.
            using (IEWSClient client = EWSClient.GetEWSClient(userEmail, credentials))
            {
                // Example operation: retrieve mailbox information.
                ExchangeMailboxInfo mailboxInfo = client.GetMailboxInfo();
                Console.WriteLine("Inbox URI: " + mailboxInfo.InboxUri);
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine("Error: " + ex.Message);
        }
    }
}
