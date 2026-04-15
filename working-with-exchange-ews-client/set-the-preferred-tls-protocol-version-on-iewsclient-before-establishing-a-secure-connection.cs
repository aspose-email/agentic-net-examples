using System;
using System.Net;
using Aspose.Email.Clients.Exchange.WebService;

class Program
{
    static void Main()
    {
        try
        {
            // Mailbox URI and credentials (replace with real values)
            string mailboxUri = "https://outlook.office365.com/EWS/Exchange.asmx";
            string username = "username@example.com";
            string password = "password";

            // Guard against placeholder credentials
            if (string.IsNullOrWhiteSpace(username) || username.Contains("username", StringComparison.OrdinalIgnoreCase) ||
                string.IsNullOrWhiteSpace(password) || password.Contains("password", StringComparison.OrdinalIgnoreCase))
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping network operation.");
                return;
            }

            // Optional for older .NET Framework scenarios.
            // Usually not needed on modern .NET.
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

            // Create the EWS client
            using (IEWSClient client = EWSClient.GetEWSClient(mailboxUri, username, password))
            {
                // Simple validation by reading server metadata
                Console.WriteLine("Exchange Server Version: " + client.ServerVersion);
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine("Error: " + ex.Message);
        }
    }
}
