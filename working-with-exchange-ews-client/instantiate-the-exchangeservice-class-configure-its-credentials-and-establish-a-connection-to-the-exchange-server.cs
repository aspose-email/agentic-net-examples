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
            // Define connection parameters
            string mailboxUri = "https://exchange.example.com/EWS/Exchange.asmx";
            string username = "user@example.com";
            string password = "password";

            // Create and configure the EWS client
            using (IEWSClient client = EWSClient.GetEWSClient(mailboxUri, username, password))
            {
                try
                {
                    // Optionally set additional client properties
                    client.Credentials = new NetworkCredential(username, password);
                    client.UseDateInLogFileName = true;

                    // Test the connection by retrieving server version info
                    string versionInfo = client.GetVersionInfo();
                    Console.WriteLine("Connected to Exchange Server. Version: " + versionInfo);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine("Error during client operation: " + ex.Message);
                    return;
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine("Unexpected error: " + ex.Message);
        }
    }
}
