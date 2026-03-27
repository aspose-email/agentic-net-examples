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
            // Connection parameters (replace with real values as needed)
            string mailboxUri = "https://exchange.example.com/EWS/Exchange.asmx";
            string username = "user@example.com";
            string password = "password";

            // Credentials for authentication
            NetworkCredential credentials = new NetworkCredential(username, password);

            // Initialize the EWS client (implements IDisposable)
            using (IEWSClient client = EWSClient.GetEWSClient(mailboxUri, credentials))
            {
                // Define a user configuration name for the Inbox folder
                UserConfigurationName configName = new UserConfigurationName("MyConfig", "Inbox");

                // Create a new user configuration instance
                UserConfiguration userConfig = new UserConfiguration(configName);
                // Example: add a simple key/value pair if the API supports it
                // userConfig.Values["SampleKey"] = "SampleValue";

                // Create the configuration on the server
                client.CreateUserConfiguration(userConfig);
                Console.WriteLine("User configuration created.");

                // Retrieve the configuration from the server
                UserConfiguration fetchedConfig = client.GetUserConfiguration(configName);
                Console.WriteLine("User configuration retrieved.");

                // Update the configuration (modify values if needed)
                // fetchedConfig.Values["SampleKey"] = "NewValue";
                client.UpdateUserConfiguration(fetchedConfig);
                Console.WriteLine("User configuration updated.");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
