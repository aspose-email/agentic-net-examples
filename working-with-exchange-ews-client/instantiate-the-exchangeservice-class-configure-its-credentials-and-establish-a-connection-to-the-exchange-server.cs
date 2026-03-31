using System;
using Aspose.Email;
using Aspose.Email.Clients.Exchange.WebService;

class Program
{
    static void Main()
    {
        try
        {
            // Placeholder credentials – skip real connection in sample environments
            string mailboxUri = "https://exchange.example.com/EWS/Exchange.asmx";
            string username = "username";
            string password = "password";

            bool isPlaceholder = mailboxUri.Contains("example.com") ||
                                  username.Equals("username", StringComparison.OrdinalIgnoreCase) ||
                                  password.Equals("password", StringComparison.OrdinalIgnoreCase);

            if (isPlaceholder)
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping connection to Exchange server.");
                return;
            }

            // Create and configure the EWS client
            using (IEWSClient client = EWSClient.GetEWSClient(mailboxUri, username, password))
            {
                try
                {
                    // Attempt a simple operation to verify the connection
                    string versionInfo = client.GetVersionInfo();
                    Console.WriteLine($"Connected to Exchange server. Version: {versionInfo}");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to connect or authenticate: {ex.Message}");
                    return;
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
