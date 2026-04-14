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
            // Mailbox credentials
            string mailboxEmail = "user@example.com";
            string username = "user@example.com";
            string password = "password";


            // Skip external calls when placeholder credentials are used
            if (mailboxEmail.Contains("example.com") || username.Contains("example.com") || password == "password")
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping external calls.");
                return;
            }

            // Use Autodiscover to obtain the current EWS URL.
            // The RedirectionUrlValidationCallback allows following redirects safely.
            AutodiscoverService autodiscover = new AutodiscoverService();
            autodiscover.RedirectionUrlValidationCallback = redirUrl =>
                Uri.IsWellFormedUriString(redirUrl, UriKind.Absolute);

            UserSettingName[] requestedSettings = { UserSettingName.InternalEwsUrl };
            var autodiscoverResponse = autodiscover.GetUserSettings(mailboxEmail, requestedSettings);

            if (!autodiscoverResponse.Settings.ContainsKey(UserSettingName.InternalEwsUrl))
            {
                Console.Error.WriteLine("Failed to obtain EWS URL via Autodiscover.");
                return;
            }

            string ewsUrl = autodiscoverResponse.Settings[UserSettingName.InternalEwsUrl] as string;
            if (string.IsNullOrEmpty(ewsUrl))
            {
                Console.Error.WriteLine("EWS URL returned by Autodiscover is empty.");
                return;
            }

            // Create the EWS client using the discovered URL.
            ICredentials credentials = new NetworkCredential(username, password);
            using (IEWSClient client = EWSClient.GetEWSClient(ewsUrl, credentials))
            {
                // Configure the client to attempt reconnection automatically.
                client.ReconnectCount = 3; // Number of reconnect attempts on connection breaks.

                // Example operation: list the inbox messages.
                ExchangeMessageInfoCollection messages = client.ListMessages("Inbox");
                Console.WriteLine($"Inbox contains {messages.Count} messages.");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
