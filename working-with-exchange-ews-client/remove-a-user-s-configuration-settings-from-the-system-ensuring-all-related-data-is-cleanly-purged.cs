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
            // Initialize EWS client (replace placeholders with real values)
            string serviceUrl = "https://exchange.example.com/EWS/Exchange.asmx";
            string username = "user@example.com";
            string password = "password";

            using (IEWSClient client = EWSClient.GetEWSClient(serviceUrl, new NetworkCredential(username, password)))
            {
                // Define the user configuration name and the folder where it is stored
                string configName = "MyUserConfig";
                string folderId = client.MailboxInfo.InboxUri; // Using Inbox as the folder

                UserConfigurationName userConfig = new UserConfigurationName(configName, folderId);

                // Delete the user configuration
                client.DeleteUserConfiguration(userConfig);
                Console.WriteLine("User configuration deleted successfully.");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
