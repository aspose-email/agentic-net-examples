using System;
using Aspose.Email;
using Aspose.Email.Clients.Exchange;
using Aspose.Email.Clients.Exchange.Dav;
using Aspose.Email.Mapi;

class Program
{
    static void Main()
    {
        try
        {
            // Placeholder credentials detection – avoid real network calls in CI
            string host = "exchange.example.com";
            string username = "user@example.com";
            string password = "password";

            if (host.Contains("example.com"))
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping execution.");
                return;
            }

            // Create Exchange client (DAV)
            using (ExchangeClient client = new ExchangeClient(host, username, password))
            {
                try
                {
                    // List messages in the Inbox folder
                    ExchangeMessageInfoCollection messages = client.ListMessages("Inbox");

                    foreach (var msgInfo in messages)
                    {
                        // Fetch the full MAPI message using its unique URI
                        using (MapiMessage mapiMessage = client.FetchMapiMessage(msgInfo.UniqueUri))
                        {
                            // Remove any voting buttons from the message
                            FollowUpManager.ClearVotingButtons(mapiMessage);
                            // (Optional) Persist changes back to the server if needed
                            // client.UpdateItem(mapiMessage, msgInfo.UniqueUri);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error processing messages: {ex.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unhandled exception: {ex.Message}");
        }
    }
}
