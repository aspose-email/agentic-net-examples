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
            // Service URL and credentials for the Exchange server
            string serviceUrl = "https://exchange.example.com/EWS/Exchange.asmx";
            NetworkCredential credentials = new NetworkCredential("username", "password");

            // Create the EWS client using the factory method
            using (IEWSClient client = EWSClient.GetEWSClient(serviceUrl, credentials))
            {
                try
                {
                    // Impersonate the shared mailbox using its SMTP address
                    client.ImpersonateUser(ItemChoice.SmtpAddress, "sharedmailbox@example.com");
                    Console.WriteLine("Impersonation set successfully.");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Impersonation failed: {ex.Message}");
                    return;
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
