using Aspose.Email;
using System;
using System.Net;
using Aspose.Email.Clients.Exchange.WebService;

namespace AsposeEmailUserConfigurationSample
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // Initialize EWS client (replace with actual mailbox URI and credentials)
                string mailboxUri = "https://outlook.office365.com/EWS/Exchange.asmx";
                string username = "user@example.com";
                string password = "password";


                // Skip external calls when placeholder credentials are used
                if (username.Contains("example.com") || password == "password")
                {
                    Console.Error.WriteLine("Placeholder credentials detected. Skipping external calls.");
                    return;
                }

                // Create the IEWSClient instance
                using (IEWSClient ewsClient = EWSClient.GetEWSClient(mailboxUri, username, password))
                {
                    // Define a user configuration name (use default enum value if specific name is unknown)
                    UserConfigurationName configName = default(UserConfigurationName);

                    // Create a new UserConfiguration instance
                    UserConfiguration userConfig = new UserConfiguration(configName);

                    // Create the user configuration on the server
                    ewsClient.CreateUserConfiguration(userConfig);
                    Console.WriteLine("User configuration created.");

                    // Retrieve the created configuration
                    UserConfiguration fetchedConfig = ewsClient.GetUserConfiguration(configName);
                    Console.WriteLine("User configuration retrieved.");

                    // Update the configuration as needed (modify properties here if required)
                    ewsClient.UpdateUserConfiguration(fetchedConfig);
                    Console.WriteLine("User configuration updated.");
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error: {ex.Message}");
            }
        }
    }
}
