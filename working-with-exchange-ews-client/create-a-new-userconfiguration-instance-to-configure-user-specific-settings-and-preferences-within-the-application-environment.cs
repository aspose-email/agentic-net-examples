using System;
using System.Net;
using Aspose.Email;
using Aspose.Email.Clients.Exchange.WebService;
using Aspose.Email.Clients.Exchange;

namespace AsposeEmailUserConfigSample
{
    class Program
    {
        static void Main(string[] args)
        {
        var credential = new System.Net.NetworkCredential("username", "password", "domain");

            try
            {
                // Define the EWS service URL and credentials
                string mailboxUri = "https://exchange.example.com/EWS/Exchange.asmx";
                NetworkCredential credentials = new NetworkCredential("username", "password");

                // Create the EWS client
                using (IEWSClient client = EWSClient.GetEWSClient(mailboxUri, credentials))
                {
                    // Define the configuration name (name and folder)
                    UserConfigurationName configName = new UserConfigurationName("MyConfig", "Inbox");

                    // Create a new user configuration instance
                    UserConfiguration userConfig = new UserConfiguration(configName);

                    // (Optional) Set configuration data here, e.g., XML data, binary data, etc.
                    // userConfig.XmlData = new byte[0];

                    // Create the configuration on the server
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
}