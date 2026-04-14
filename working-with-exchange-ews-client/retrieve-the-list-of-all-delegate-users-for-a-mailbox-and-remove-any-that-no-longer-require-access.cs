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
            // Mailbox to manage
            string mailbox = "user@example.com";


            // Skip external calls when placeholder credentials are used
            if (mailbox.Contains("example.com"))
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping external calls.");
                return;
            }

            // Credentials for EWS
            NetworkCredential credentials = new NetworkCredential("username", "password");

            // Create EWS client
            using (IEWSClient client = EWSClient.GetEWSClient("https://exchange.example.com/EWS/Exchange.asmx", credentials))
            {
                try
                {
                    // Retrieve current delegate users
                    ExchangeDelegateUserCollection delegateUsers = client.ListDelegates(mailbox);

                    // Iterate and remove delegates that match a removal condition
                    foreach (ExchangeDelegateUser delegateUser in delegateUsers)
                    {
                        // Example condition: remove delegates whose email ends with a specific domain
                        string delegateAddress = delegateUser.UserInfo.PrimarySmtpAddress;
                        if (delegateAddress != null && delegateAddress.EndsWith("@oldcompany.com", StringComparison.OrdinalIgnoreCase))
                        {
                            // Close access for the delegate
                            client.CloseAccess(delegateAddress, mailbox);
                            Console.WriteLine($"Removed delegate: {delegateAddress}");
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"EWS operation failed: {ex.Message}");
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
