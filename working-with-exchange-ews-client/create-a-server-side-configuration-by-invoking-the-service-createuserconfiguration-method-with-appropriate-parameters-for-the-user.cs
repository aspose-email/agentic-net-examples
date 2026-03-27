using System;
using System.Net;
using Aspose.Email;
using Aspose.Email.Clients.Exchange.WebService;

class Program
{
    static void Main()
    {
        // Top‑level exception guard
        try
        {
            // Server connection parameters
            string mailboxUri = "https://exchange.example.com/EWS/Exchange.asmx";
            string username = "user@example.com";
            string password = "password";

            // Create and dispose the EWS client safely
            using (IEWSClient client = EWSClient.GetEWSClient(mailboxUri, username, password))
            {
                // Build a user configuration name (configuration name + folder)
                UserConfigurationName configName = new UserConfigurationName("MyConfig", "Inbox");

                // Create the user configuration object
                UserConfiguration userConfig = new UserConfiguration(configName);

                // (Optional) Populate configuration data, e.g. XML settings
                // userConfig.XmlData = System.Text.Encoding.UTF8.GetBytes("<settings></settings>");

                // Invoke the service to create the configuration on the server
                client.CreateUserConfiguration(userConfig);

                Console.WriteLine("User configuration created successfully.");
            }
        }
        catch (Exception ex)
        {
            // Friendly error output
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
