using System;
using System.Net;
using Aspose.Email;
using Aspose.Email.Clients.Exchange;
using Aspose.Email.Clients.Exchange.WebService;

class Program
{
    static void Main()
    {
        try
        {
            // Placeholder credentials – replace with real values for actual execution
            string serviceUrl = "https://exchange.example.com/EWS/Exchange.asmx";
            string username = "user@example.com";
            string password = "password";

            // Skip real network call when placeholders are detected
            if (serviceUrl.Contains("example.com"))
            {
                Console.WriteLine("Placeholder credentials detected. Skipping Exchange connection.");
                return;
            }

            NetworkCredential credentials = new NetworkCredential(username, password);

            // Create the EWS client using the factory method
            using (IEWSClient client = EWSClient.GetEWSClient(serviceUrl, credentials))
            {
                // Retrieve message infos from the Inbox folder
                ExchangeMessageInfoCollection messageInfos = client.ListMessages(client.MailboxInfo.InboxUri);

                // Define the subject keyword to filter messages
                string subjectKeyword = "Invoice";

                foreach (ExchangeMessageInfo info in messageInfos)
                {
                    // Filter based on the Subject property (case‑insensitive)
                    if (info.Subject != null && info.Subject.IndexOf(subjectKeyword, StringComparison.OrdinalIgnoreCase) >= 0)
                    {
                        // Fetch the full message using the unique URI
                        MailMessage fullMessage = client.FetchMessage(info.UniqueUri);

                        Console.WriteLine($"Subject : {fullMessage.Subject}");
                        Console.WriteLine($"From    : {fullMessage.From}");
                        Console.WriteLine($"Received: {info.InternalDate}");
                        Console.WriteLine(new string('-', 40));

                        fullMessage.Dispose();
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
