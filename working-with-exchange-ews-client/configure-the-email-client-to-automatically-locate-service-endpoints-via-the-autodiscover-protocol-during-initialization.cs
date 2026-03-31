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
            // Placeholder credentials – skip real network call in CI environments
            string domain = "example.com";
            string userSmtpAddress = "user@example.com";
            string password = "password";

            if (domain.Contains("example") || userSmtpAddress.Contains("example") || password == "password")
            {
                Console.Error.WriteLine("Placeholder credentials detected – skipping AutoDiscover and client connection.");
                return;
            }

            // Initialize AutoDiscover service for the domain
            AutodiscoverService autodiscover = new AutodiscoverService(domain);
            autodiscover.Credentials = new NetworkCredential(userSmtpAddress, password);

            // Retrieve EWS URLs (internal and external)
            GetUserSettingsResponse settings = autodiscover.GetUserSettings(
                userSmtpAddress,
                UserSettingName.InternalEwsUrl,
                UserSettingName.ExternalEwsUrl);

            // Choose the first available URL
            string ewsUrl = null;
            if (settings != null && settings.Settings != null)
            {
                if (settings.Settings.ContainsKey(UserSettingName.InternalEwsUrl))
                    ewsUrl = settings.Settings[UserSettingName.InternalEwsUrl] as string;
                else if (settings.Settings.ContainsKey(UserSettingName.ExternalEwsUrl))
                    ewsUrl = settings.Settings[UserSettingName.ExternalEwsUrl] as string;
            }

            if (string.IsNullOrEmpty(ewsUrl))
            {
                Console.Error.WriteLine("Unable to locate EWS endpoint via AutoDiscover.");
                return;
            }

            // Create the EWS client using the discovered URL
            IEWSClient client = null;
            try
            {
                client = EWSClient.GetEWSClient(ewsUrl, new NetworkCredential(userSmtpAddress, password));
                // Example operation: display the mailbox URI
                Console.WriteLine("Connected to mailbox URI: " + client.MailboxInfo.InboxUri);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine("Failed to create or connect EWS client: " + ex.Message);
                return;
            }
            finally
            {
                // Ensure the client is disposed
                if (client != null)
                    client.Dispose();
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine("Unexpected error: " + ex.Message);
        }
    }
}
