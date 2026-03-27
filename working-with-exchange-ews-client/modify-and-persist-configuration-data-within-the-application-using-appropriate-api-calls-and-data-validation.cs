using System;
using System.Net;
using Aspose.Email;
using Aspose.Email.Clients.Exchange.WebService;
using Aspose.Email.Clients.Exchange;

namespace AsposeEmailConfigSample
{
    class Program
    {
        static void Main()
        {
            try
            {
                // Define mailbox URI and credentials
                string mailboxUri = "https://exchange.example.com/EWS/Exchange.asmx";
                NetworkCredential credentials = new NetworkCredential("username", "password");

                // Create EWS client safely
                try
                {
                    using (IEWSClient client = EWSClient.GetEWSClient(mailboxUri, credentials))
                    {
                        // Validate configuration name
                        string configName = "MyConfig";
                        if (string.IsNullOrWhiteSpace(configName))
                        {
                            Console.Error.WriteLine("Configuration name cannot be empty.");
                            return;
                        }

                        // Use Inbox folder URI for storing the configuration
                        string folderUri = client.MailboxInfo.InboxUri;
                        if (string.IsNullOrWhiteSpace(folderUri))
                        {
                            Console.Error.WriteLine("Unable to determine Inbox folder URI.");
                            return;
                        }

                        // Create a configuration identifier
                        UserConfigurationName configIdentifier = new UserConfigurationName(configName, folderUri);

                        // Prepare the user configuration object
                        UserConfiguration userConfig = new UserConfiguration(configIdentifier);
                        // Add sample key/value data
                        userConfig.Dictionary["SampleKey"] = "SampleValue";

                        // Persist the configuration on the server
                        try
                        {
                            client.CreateUserConfiguration(userConfig);
                            Console.WriteLine("User configuration created successfully.");
                        }
                        catch (Exception ex)
                        {
                            Console.Error.WriteLine($"Failed to create configuration: {ex.Message}");
                        }

                        // Retrieve and display the stored configuration for verification
                        try
                        {
                            UserConfiguration retrievedConfig = client.GetUserConfiguration(configIdentifier);
                            if (retrievedConfig != null && retrievedConfig.Dictionary.ContainsKey("SampleKey"))
                            {
                                Console.WriteLine($"Retrieved value: {retrievedConfig.Dictionary["SampleKey"]}");
                            }
                            else
                            {
                                Console.WriteLine("Configuration retrieved but key not found.");
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.Error.WriteLine($"Failed to retrieve configuration: {ex.Message}");
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to connect to EWS service: {ex.Message}");
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Unexpected error: {ex.Message}");
            }
        }
    }
}
