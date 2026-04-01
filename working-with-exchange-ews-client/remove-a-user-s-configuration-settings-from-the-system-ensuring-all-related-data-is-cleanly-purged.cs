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
            // Placeholder credentials – skip actual network call in CI environments
            string mailboxUri = "https://exchange.example.com/EWS/Exchange.asmx";
            string username = "username";
            string password = "password";

            if (mailboxUri.Contains("example.com"))
            {
                Console.WriteLine("Placeholder credentials detected. Skipping EWS operation.");
                return;
            }

            // Create the EWS client
            using (IEWSClient client = EWSClient.GetEWSClient(mailboxUri, username, password))
            {
                // Build the user configuration identifier (name + folder ID)
                // Using the Inbox folder as the associated folder for the configuration
                string configName = "MyUserConfig";
                string folderId = client.MailboxInfo.InboxUri;
                UserConfigurationName userConfig = new UserConfigurationName(configName, folderId);

                // Delete the user configuration
                client.DeleteUserConfiguration(userConfig);
                Console.WriteLine($"User configuration '{configName}' deleted successfully.");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
