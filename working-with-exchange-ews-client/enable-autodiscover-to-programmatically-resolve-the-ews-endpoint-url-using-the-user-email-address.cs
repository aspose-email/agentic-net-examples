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
            // User credentials and email address
            string userEmail = "user@example.com";
            string username = "user@example.com";
            string password = "password";

            // Initialize Autodiscover service and set credentials
            AutodiscoverService autodiscover = new AutodiscoverService();
            autodiscover.Credentials = new NetworkCredential(username, password);

            // Retrieve the Internal EWS URL for the given email address
            GetUserSettingsResponse settingsResponse = autodiscover.GetUserSettings(
                userEmail,
                UserSettingName.InternalEwsUrl);

            string ewsUrl = settingsResponse.Settings[UserSettingName.InternalEwsUrl] as string;
            if (string.IsNullOrEmpty(ewsUrl))
            {
                Console.Error.WriteLine("Failed to obtain EWS URL via Autodiscover.");
                return;
            }

            // Create the EWS client using the discovered URL
            using (IEWSClient client = EWSClient.GetEWSClient(ewsUrl, new NetworkCredential(username, password)))
            {
                // Example operation: list subjects of messages in the Inbox folder
                ExchangeMessageInfoCollection messages = client.ListMessages(client.MailboxInfo.InboxUri);
                foreach (ExchangeMessageInfo info in messages)
                {
                    Console.WriteLine($"Subject: {info.Subject}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
