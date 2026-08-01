using Aspose.Email;
using System;
using System.Net;
using Aspose.Email.Clients.Exchange.WebService;
using Aspose.Email.Clients.Exchange;

class Program
{
    static void Main()
    {
        try
        {
            // Author: Sample code to configure EWS authentication credentials
            // Define the EWS service URL (mailbox URI) and user credentials
            string mailboxUri = "https://outlook.office365.com/EWS/Exchange.asmx";
            string username = "user@example.com";
            string password = "password";


            // Skip external calls when placeholder credentials are used
            if (username.Contains("example.com") || password == "password")
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping external calls.");
                return;
            }

            // Create a NetworkCredential instance with the supplied username and password
            NetworkCredential credentials = new NetworkCredential(username, password);

            // Initialize the EWS client using the mailbox URI and credentials
            using (IEWSClient ewsClient = EWSClient.GetEWSClient(mailboxUri, credentials))
            {
                // Retrieve mailbox information to verify successful authentication
                ExchangeMailboxInfo mailboxInfo = ewsClient.GetMailboxInfo();
                Console.WriteLine("Connected successfully. Mailbox display name: " + mailboxInfo.MailboxUri);
            }
        }
        catch (Exception ex)
        {
            // Output any errors without crashing the application
            Console.Error.WriteLine("Error: " + ex.Message);
        }
    }
}
