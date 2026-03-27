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
            // Service URL and credentials
            string serviceUrl = "https://exchange.example.com/EWS/Exchange.asmx";
            NetworkCredential credentials = new NetworkCredential("username", "password");

            // Create EWS client
            using (IEWSClient client = EWSClient.GetEWSClient(serviceUrl, credentials))
            {
                // Define the user configuration name (folder identifier and configuration name)
                UserConfigurationName configName = new UserConfigurationName("folderId", "configName");

                // Retrieve the user configuration
                UserConfiguration userConfig = client.GetUserConfiguration(configName);

                // Indicate success
                Console.WriteLine("User configuration retrieved successfully.");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
