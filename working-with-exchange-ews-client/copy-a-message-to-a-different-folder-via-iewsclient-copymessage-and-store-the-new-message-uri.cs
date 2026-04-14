using System;
using Aspose.Email;
using Aspose.Email.Clients.Exchange.WebService;

class Program
{
    static void Main()
    {
        try
        {
            // Initialize the EWS client
            string mailboxUri = "https://exchange.example.com/EWS/Exchange.asmx";
            string username = "username";
            string password = "password";

            using (IEWSClient client = EWSClient.GetEWSClient(mailboxUri, username, password))
            {
                // Source message URI and destination folder URI
                string sourceMessageUri = "https://exchange.example.com/EWS/Exchange.asmx/MessageId";
                string destinationFolderUri = "https://exchange.example.com/EWS/Exchange.asmx/Inbox";


                // Skip external calls when placeholder credentials are used
                if (mailboxUri.Contains("example.com") || username == "username" || password == "password" || sourceMessageUri.Contains("example.com") || destinationFolderUri.Contains("example.com"))
                {
                    Console.Error.WriteLine("Placeholder credentials detected. Skipping external calls.");
                    return;
                }

                try
                {
                    // Copy the message and obtain the new URI
                    string copiedMessageUri = client.CopyItem(sourceMessageUri, destinationFolderUri);
                    Console.WriteLine("Copied message URI: " + copiedMessageUri);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine("Error copying message: " + ex.Message);
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
