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
            // Exchange server details
            string mailboxUri = "https://exchange.example.com/EWS/Exchange.asmx";
            string username = "user@example.com";
            string password = "password";


            // Skip external calls when placeholder credentials are used
            if (mailboxUri.Contains("example.com") || username.Contains("example.com") || password == "password")
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping external calls.");
                return;
            }

            // Attempt to connect using EWS
            try
            {
                using (IEWSClient ewsClient = EWSClient.GetEWSClient(mailboxUri, username, password))
                {
                    // Simple operation to verify the connection
                    string versionInfo = ewsClient.GetVersionInfo();
                    Console.WriteLine($"EWS connection successful. Server version: {versionInfo}");
                }
            }
            catch (Exception ewsEx)
            {
                Console.Error.WriteLine($"EWS connection failed: {ewsEx.Message}");
                Console.Error.WriteLine("Fallback to IMAP client could be implemented here.");
                // IMAP fallback logic would go here, but IMAP usage is omitted to comply with EWS sample rules.
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
