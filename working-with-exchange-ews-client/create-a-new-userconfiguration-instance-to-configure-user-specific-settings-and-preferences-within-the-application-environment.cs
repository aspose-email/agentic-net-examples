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
            // Initialize EWS client
            string mailboxUri = "https://exchange.example.com/EWS/Exchange.asmx";
            string username = "user@example.com";
            string password = "password";

            using (IEWSClient client = EWSClient.GetEWSClient(mailboxUri, username, password))
            {
                // Define the user configuration name (folder identifier and configuration name)
                UserConfigurationName configName = new UserConfigurationName("Inbox", "MyUserConfig");

                // Create a new UserConfiguration instance
                UserConfiguration userConfig = new UserConfiguration(configName);

                // Optionally, create the configuration on the server
                client.CreateUserConfiguration(userConfig);

                Console.WriteLine("User configuration created successfully.");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine(ex.Message);
        }
    }
}
