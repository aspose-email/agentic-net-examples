using Aspose.Email.Clients.Exchange;
using Aspose.Email;
using System;
using Aspose.Email.Clients.Exchange.WebService;

namespace AsposeEmailUserConfigExample
{
    class Program
    {
        static void Main()
        {
            try
            {
                // Define connection parameters
                string mailboxUri = "https://mail.example.com/EWS/Exchange.asmx";
                string username = "user@example.com";
                string password = "password";


                // Skip external calls when placeholder credentials are used
                if (mailboxUri.Contains("example.com") || username.Contains("example.com") || password == "password")
                {
                    Console.Error.WriteLine("Placeholder credentials detected. Skipping external calls.");
                    return;
                }

                // Create and connect the EWS client
                using (IEWSClient client = EWSClient.GetEWSClient(mailboxUri, username, password))
                {
                    // Retrieve mailbox information to obtain a folder identifier (e.g., Inbox)
                    ExchangeMailboxInfo mailboxInfo = client.GetMailboxInfo();
                    string inboxFolderId = mailboxInfo.InboxUri;

                    // Define a user configuration name associated with the Inbox folder
                    UserConfigurationName configName = new UserConfigurationName("MyUserConfig", inboxFolderId);

                    // Create a new user configuration object
                    UserConfiguration userConfig = new UserConfiguration(configName);
                    // Example: add a simple key/value pair to the configuration
                    userConfig.Dictionary["SampleKey"] = "SampleValue";

                    // Create the user configuration on the server
                    client.CreateUserConfiguration(userConfig);
                    Console.WriteLine("User configuration created successfully.");

                    // Retrieve the created configuration to verify
                    UserConfiguration retrievedConfig = client.GetUserConfiguration(configName);
                    if (retrievedConfig != null && retrievedConfig.Dictionary.ContainsKey("SampleKey"))
                    {
                        Console.WriteLine("Retrieved configuration value: " + retrievedConfig.Dictionary["SampleKey"]);
                    }
                    else
                    {
                        Console.WriteLine("Failed to retrieve the created configuration.");
                    }
                }
            }
            catch (Exception ex)
            {
                // Log any unexpected errors without crashing the application
                Console.Error.WriteLine("Error: " + ex.Message);
            }
        }
    }
}
