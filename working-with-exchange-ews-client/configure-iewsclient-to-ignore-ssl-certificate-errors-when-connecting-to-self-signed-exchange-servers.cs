using Aspose.Email;
using System;
using System.Net;
using Aspose.Email.Clients.Exchange.WebService;

class Program
{
    static void Main()
    {
        try
        {
            // Mailbox URI and credentials – replace with real values.
            string mailboxUri = "https://your.exchange.server/EWS/Exchange.asmx";
            string username = "your_username";
            string password = "your_password";

            // Guard: skip network call when placeholders are still present.
            if (mailboxUri.Contains("your.") || username.Contains("your_") || password.Contains("your_"))
            {
                Console.WriteLine("Please provide valid mailbox URI, username and password before running the sample.");
                return;
            }

            // Ignore SSL certificate errors (useful for self‑signed certificates).
            ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };

            // Create the EWS client. The variable name 'client' must be preserved.
            using (IEWSClient client = EWSClient.GetEWSClient(mailboxUri, username, password))
            {
                // Simple operation to verify the connection.
                string versionInfo = client.GetVersionInfo();
                Console.WriteLine("Exchange server version: " + versionInfo);
            }
        }
        catch (Exception ex)
        {
            // Friendly error output – no unhandled exceptions.
            Console.Error.WriteLine(ex.Message);
        }
    }
}
