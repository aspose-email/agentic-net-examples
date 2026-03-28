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
            NetworkCredential credentials = new NetworkCredential("username", "password");

            // Discover the EWS endpoint URL for the given email address
            AutodiscoverService autodiscover = new AutodiscoverService();
            autodiscover.Credentials = credentials;

            GetUserSettingsResponse settingsResponse = autodiscover.GetUserSettings(
                userEmail,
                UserSettingName.InternalEwsUrl);

            // Extract the discovered URL
            string ewsUrl = settingsResponse.Settings[UserSettingName.InternalEwsUrl] as string;

            if (string.IsNullOrEmpty(ewsUrl))
            {
                Console.Error.WriteLine("Failed to discover the EWS URL for the user.");
                return;
            }

            // Create the EWS client using the discovered URL
            try
            {
                using (IEWSClient client = EWSClient.GetEWSClient(ewsUrl, credentials))
                {
                    // Example operation: display the Inbox URI
                    Console.WriteLine("Connected to EWS. Inbox URI: " + client.MailboxInfo.InboxUri);
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine("Error creating or using EWS client: " + ex.Message);
                return;
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine("Unexpected error: " + ex.Message);
        }
    }
}
