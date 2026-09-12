using System;
using Aspose.Email.Clients;
using Aspose.Email.Clients.Exchange;
using Aspose.Email.Clients.Exchange.WebService;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            string clientId = "YOUR_CLIENT_ID";
            string clientSecret = "YOUR_CLIENT_SECRET";
            string refreshToken = "YOUR_REFRESH_TOKEN";
            string mailboxUri = "https://outlook.office365.com/EWS/Exchange.asmx";

            if (string.IsNullOrWhiteSpace(clientId) || clientId.StartsWith("YOUR_") ||
                string.IsNullOrWhiteSpace(clientSecret) || clientSecret.StartsWith("YOUR_") ||
                string.IsNullOrWhiteSpace(refreshToken) || refreshToken.StartsWith("YOUR_"))
            {
                Console.Error.WriteLine("Provide valid OAuth credentials.");
                return;
            }

            using (TokenProvider tokenProvider = TokenProvider.Outlook.GetInstance(clientId, clientSecret, refreshToken))
            {
                OAuthNetworkCredential credentials = new OAuthNetworkCredential(tokenProvider);
                using (IEWSClient client = EWSClient.GetEWSClient(mailboxUri, credentials))
                {
                    ExchangeMailboxInfo mailboxInfo = client.MailboxInfo;
                    Console.WriteLine("Mailbox URI: " + mailboxInfo.MailboxUri);
                    Console.WriteLine("Inbox URI: " + mailboxInfo.InboxUri);
                    Console.WriteLine("Drafts URI: " + mailboxInfo.DraftsUri);

                    ExchangeMessageInfoCollection messages = client.ListMessages(mailboxInfo.InboxUri);
                    foreach (ExchangeMessageInfo message in messages)
                    {
                        Console.WriteLine(message.Subject);
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine("Error: " + ex.Message);
        }
    }
}
