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
            // Initialize EWS client with placeholder credentials
            IEWSClient client = EWSClient.GetEWSClient(
                "https://exchange.example.com/EWS/Exchange.asmx",
                new NetworkCredential("admin@example.com", "password"));

            using (client)
            {
                try
                {
                    // Impersonate another user
                    client.ImpersonateUser(ItemChoice.PrimarySmtpAddress, "impersonated@example.com");

                    // Example operation: list messages in the impersonated user's Inbox
                    foreach (var msgInfo in client.ListMessages())
                    {
                        Console.WriteLine($"Subject: {msgInfo.Subject}");
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Operation error: {ex.Message}");
                }
                finally
                {
                    // Reset impersonation if needed
                    client.ResetImpersonation();
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Initialization error: {ex.Message}");
        }
    }
}
