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
            // Placeholder user email and credentials.
            string userEmail = "user@example.com";
            string username = "username";
            string password = "password";

            // If placeholders are detected, skip the network call.
            if (userEmail.Contains("example.com") || username == "username")
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping AutoDiscover call.");
                return;
            }

            // Initialize the Autodiscover service (does not implement IDisposable).
            AutodiscoverService autodiscover = new AutodiscoverService();

            // Set credentials for the Autodiscover request.
            autodiscover.Credentials = new NetworkCredential(username, password);

            // Retrieve the internal EWS URL for the specified user.
            GetUserSettingsResponse settingsResponse = autodiscover.GetUserSettings(
                userEmail,
                UserSettingName.InternalEwsUrl);

            // Extract the URL from the response.
            string ewsUrl = settingsResponse.Settings[UserSettingName.InternalEwsUrl] as string;
            if (string.IsNullOrEmpty(ewsUrl))
            {
                Console.Error.WriteLine("Failed to obtain EWS URL via AutoDiscover.");
                return;
            }

            // Create the EWS client using the discovered URL.
            IEWSClient client = null;
            try
            {
                client = EWSClient.GetEWSClient(ewsUrl, new NetworkCredential(username, password));
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error creating EWS client: {ex.Message}");
                return;
            }

            // Use the client (example: display the Inbox URI).
            try
            {
                Console.WriteLine($"Inbox URI: {client.MailboxInfo.InboxUri}");
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error using EWS client: {ex.Message}");
            }
            finally
            {
                // Ensure the client is disposed.
                if (client != null)
                {
                    client.Dispose();
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
