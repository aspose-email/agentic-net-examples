using System;
using System.Net;
using Aspose.Email;
using Aspose.Email.Clients.Exchange;
using Aspose.Email.Clients.Exchange.WebService;

namespace EmailAutoDiscoverSample
{
    class Program
    {
        static void Main()
        {
            try
            {
                // Credentials for authentication
                NetworkCredential credentials = new NetworkCredential("username", "password");

                // Initialize Autodiscover service and set credentials
                AutodiscoverService autodiscover = new AutodiscoverService();
                autodiscover.Credentials = credentials;

                // Retrieve the internal EWS URL for the mailbox
                GetUserSettingsResponse settingsResponse = autodiscover.GetUserSettings(
                    "user@example.com",
                    UserSettingName.InternalEwsUrl);

                string ewsUrl = settingsResponse.Settings[UserSettingName.InternalEwsUrl] as string;
                if (string.IsNullOrEmpty(ewsUrl))
                {
                    Console.Error.WriteLine("Failed to obtain EWS URL via Autodiscover.");
                    return;
                }

                // Create the EWS client using the discovered URL
                using (IEWSClient client = EWSClient.GetEWSClient(ewsUrl, credentials))
                {
                    // List messages from the Inbox folder
                    ExchangeMessageInfoCollection messages = client.ListMessages(client.MailboxInfo.InboxUri);
                    foreach (ExchangeMessageInfo info in messages)
                    {
                        Console.WriteLine(info.Subject);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error: {ex.Message}");
            }
        }
    }
}
