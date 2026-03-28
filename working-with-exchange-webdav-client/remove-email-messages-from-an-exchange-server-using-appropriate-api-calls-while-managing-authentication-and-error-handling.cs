using Aspose.Email.Clients.Exchange;
using System;
using System.Net;
using Aspose.Email;
using Aspose.Email.Clients.Exchange.Dav;

namespace AsposeEmailExample
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // Exchange server connection settings
                string mailboxUri = "https://exchange.example.com/EWS/Exchange.asmx";
                string username = "user@example.com";
                string password = "password";

                // Create and use the Exchange client inside a using block to ensure disposal
                using (ExchangeClient client = new ExchangeClient(mailboxUri, new NetworkCredential(username, password)))
                {
                    try
                    {
                        // List messages in the Inbox folder
                        ExchangeMessageInfoCollection messages = client.ListMessages("Inbox");

                        // Delete each message by its unique URI
                        foreach (ExchangeMessageInfo info in messages)
                        {
                            // Ensure the UniqueUri is not null or empty before attempting deletion
                            if (!string.IsNullOrEmpty(info.UniqueUri))
                            {
                                client.DeleteMessage(info.UniqueUri);
                                Console.WriteLine($"Deleted message: {info.Subject}");
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        // Handle errors that occur during listing or deletion
                        Console.Error.WriteLine($"Operation error: {ex.Message}");
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle errors that occur during client creation/connection
                Console.Error.WriteLine($"Connection error: {ex.Message}");
            }
        }
    }
}
