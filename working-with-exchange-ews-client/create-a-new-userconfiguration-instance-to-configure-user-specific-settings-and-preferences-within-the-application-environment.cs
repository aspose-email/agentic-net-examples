using System;
using System.Net;
using Aspose.Email;
using Aspose.Email.Clients.Exchange.WebService;
using Aspose.Email.Clients.Exchange;

class Program
{
    static void Main()
    {
        try
        {
            // Mailbox URI and credentials for EWS connection
            string mailboxUri = "https://exchange.example.com/EWS/Exchange.asmx";
            NetworkCredential credentials = new NetworkCredential("username", "password");

            // Create the EWS client (IDisposable)
            using (IEWSClient client = EWSClient.GetEWSClient(mailboxUri, credentials))
            {
                // Define a user configuration name (configuration name and folder)
                UserConfigurationName configName = new UserConfigurationName("MyConfig", "Inbox");

                // Create a new UserConfiguration instance associated with the name
                UserConfiguration userConfig = new UserConfiguration(configName);

                // Example: add a custom setting to the configuration dictionary
                userConfig.Dictionary["MySetting"] = "Value";

                // Create the user configuration on the server
                client.CreateUserConfiguration(userConfig);

                Console.WriteLine("User configuration created successfully.");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
