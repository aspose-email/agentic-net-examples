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
            // Obtain OAuth token (placeholder value)
            string oauthToken = "access_token_placeholder";

            // Create EWS client and assign OAuth token to Credentials
            try
            {
                using (IEWSClient client = EWSClient.GetEWSClient("https://outlook.office365.com/EWS/Exchange.asmx", new NetworkCredential(string.Empty, string.Empty)))
                {
                    // Assign the token as the password in NetworkCredential
                    client.Credentials = new NetworkCredential(oauthToken, string.Empty);

                    // Example operation: list messages in the Inbox folder
                    try
                    {
                        var messages = client.ListMessages(client.MailboxInfo.InboxUri);
                        foreach (var info in messages)
                        {
                            Console.WriteLine(info.UniqueUri);
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Error during EWS operation: {ex.Message}");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to create or configure EWS client: {ex.Message}");
                return;
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
