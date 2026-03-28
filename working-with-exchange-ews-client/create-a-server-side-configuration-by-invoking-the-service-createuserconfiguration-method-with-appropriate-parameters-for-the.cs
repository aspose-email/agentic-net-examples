using System;
using System.Net;
using Aspose.Email;
using Aspose.Email.Clients.Exchange.WebService;

class Program
{
    static void Main()
    {
        try
        {
            // Initialize the EWS client with placeholder credentials
            string mailboxUri = "https://exchange.example.com/EWS/Exchange.asmx";
            string username = "user@example.com";
            string password = "password";

            using (IEWSClient client = EWSClient.GetEWSClient(mailboxUri, username, password))
            {
                // Define the user configuration name (name and folder identifier)
                UserConfigurationName configName = new UserConfigurationName("MyConfig", "Inbox");

                // Create a new UserConfiguration instance
                UserConfiguration userConfig = new UserConfiguration(configName);

                // Create the user configuration on the Exchange server
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
