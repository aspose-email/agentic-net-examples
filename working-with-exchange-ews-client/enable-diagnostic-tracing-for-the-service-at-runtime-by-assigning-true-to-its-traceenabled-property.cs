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
            string mailboxUri = "https://exchange.example.com/EWS/Exchange.asmx";
            string username = "username";
            string password = "password";

            // Guard against placeholder credentials
            if (mailboxUri.Contains("example.com") || username == "username" || password == "password")
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping execution.");
                return;
            }

            using (IEWSClient client = EWSClient.GetEWSClient(mailboxUri, username, password))
            {
                // Enable diagnostic tracing (property not defined on the interface, accessed via dynamic)
                ((dynamic)client).TraceEnabled = true;

                // Sample operation to verify the client works
                string versionInfo = client.GetVersionInfo();
                Console.WriteLine("Exchange version: " + versionInfo);
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine(ex.Message);
        }
    }
}
