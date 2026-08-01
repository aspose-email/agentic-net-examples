using Aspose.Email;
using System;
using System.Net;
using Aspose.Email.Clients.Exchange.WebService;

namespace AsposeEmailEwsAuthSample
{
    // Author: Aspose.Email .NET sample
    class Program
    {
        static void Main()
        {
            try
            {
                // Define authentication parameters
                string mailboxUri = "https://mail.example.com/EWS/Exchange.asmx";
                string username = "user@example.com";
                string password = "P@ssw0rd";

                // Create the EWS client with credentials
                IEWSClient client = EWSClient.GetEWSClient(mailboxUri, username, password);

                // The client is now authenticated and ready for further operations
                Console.WriteLine("EWS client successfully authenticated.");

                // Dispose the client when done
                if (client is IDisposable disposableClient)
                {
                    disposableClient.Dispose();
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error: {ex.Message}");
                // Gracefully exit without rethrowing
            }
        }
    }
}
