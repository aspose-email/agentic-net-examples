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
            // Initialize the EWS client with placeholder credentials.
            string mailboxUri = "https://exchange.example.com/EWS/Exchange.asmx";
            NetworkCredential credentials = new NetworkCredential("username", "password");

            using (IEWSClient client = EWSClient.GetEWSClient(mailboxUri, credentials))
            {
                // Enable logging (runtime tracing) by specifying a log file.
                client.LogFileName = "exchange_trace.log";
                client.UseDateInLogFileName = true; // optional: include date in log file name

                // Example operation to verify the client works.
                try
                {
                    string versionInfo = client.GetVersionInfo();
                    Console.WriteLine($"Exchange server version: {versionInfo}");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error during operation: {ex.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
