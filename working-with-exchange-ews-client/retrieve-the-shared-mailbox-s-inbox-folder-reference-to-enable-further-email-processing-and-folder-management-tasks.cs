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
            // Author note: Example to obtain the Inbox folder URI of a shared mailbox using EWS.
            // Define EWS service URL and credentials.
            string ewsUrl = "https://mail.example.com/EWS/Exchange.asmx";
            string userName = "user@example.com";
            string password = "password";


            // Skip external calls when placeholder credentials are used
            if (ewsUrl.Contains("example.com") || userName.Contains("example.com") || password == "password")
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping external calls.");
                return;
            }

            // Create network credentials.
            NetworkCredential credentials = new NetworkCredential(userName, password);

            // Initialize the EWS client. The variable name 'client' is preserved throughout.
            using (IEWSClient client = EWSClient.GetEWSClient(ewsUrl, credentials))
            {
                // Retrieve mailbox information.
                ExchangeMailboxInfo mailboxInfo = client.GetMailboxInfo();

                // Get the Inbox folder URI.
                string inboxFolderUri = mailboxInfo.InboxUri;

                Console.WriteLine("Inbox folder URI: " + inboxFolderUri);

                // The inboxFolderUri can now be used for further email processing.
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine("Error: " + ex.Message);
        }
    }
}
