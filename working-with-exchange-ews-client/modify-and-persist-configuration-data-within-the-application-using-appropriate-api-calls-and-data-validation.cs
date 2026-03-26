using System;
using System.Net;
using Aspose.Email;
using Aspose.Email.Clients.Exchange;
using Aspose.Email.Clients.Exchange.WebService;

class Program
{
    static void Main()
    {
        var credential = new System.Net.NetworkCredential("username", "password", "domain");

        // Top‑level exception guard
        try
        {
            // EWS endpoint and credentials (placeholders)
            string mailboxUri = "https://exchange.example.com/EWS/Exchange.asmx";
            NetworkCredential credentials = new NetworkCredential("user@example.com", "password");

            // Create the EWS client via the factory method and ensure disposal
            using (IEWSClient client = EWSClient.GetEWSClient(mailboxUri, credentials))
            {
                // Connection safety guard
                try
                {
                    // Retrieve mailbox information
                    ExchangeMailboxInfo mailboxInfo = client.MailboxInfo;
                    Console.WriteLine("Inbox URI: " + mailboxInfo.InboxUri);

                    // Define a user configuration name in the Inbox folder
                    UserConfigurationName configName = new UserConfigurationName("MyConfig", mailboxInfo.InboxUri);

                    // Create a new user configuration with a simple key/value pair
                    UserConfiguration config = new UserConfiguration(configName);
                    config.Dictionary.Add("SampleKey", "SampleValue");

                    // Persist the configuration on the server
                    client.CreateUserConfiguration(config);
                    Console.WriteLine("User configuration created.");

                    // Retrieve the configuration to verify it was saved
                    UserConfiguration fetchedConfig = client.GetUserConfiguration(configName);
                    Console.WriteLine("Fetched config value: " + fetchedConfig.Dictionary["SampleKey"]);

                    // Update the configuration value
                    fetchedConfig.Dictionary["SampleKey"] = "UpdatedValue";
                    client.UpdateUserConfiguration(fetchedConfig);
                    Console.WriteLine("User configuration updated.");

                    // Clean up: delete the configuration
                    client.DeleteUserConfiguration(configName);
                    Console.WriteLine("User configuration deleted.");
                }
                catch (Exception connEx)
                {
                    // Handle connection‑related errors gracefully
                    Console.Error.WriteLine("EWS operation failed: " + connEx.Message);
                    return;
                }
            }
        }
        catch (Exception ex)
        {
            // Global exception handling
            Console.Error.WriteLine("Unexpected error: " + ex.Message);
        }
    }
}