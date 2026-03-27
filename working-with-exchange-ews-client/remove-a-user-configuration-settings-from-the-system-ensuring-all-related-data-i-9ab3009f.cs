using System;
using System.Net;
using Aspose.Email;
using Aspose.Email.Clients.Exchange;
using Aspose.Email.Clients.Exchange.WebService;

class Program
{
    static void Main()
    {
        var credential = new System.Net.NetworkCredential("username", "password", "domain");

        try
        {
            // Mailbox URI and credentials (replace with real values)
            string mailboxUri = "https://exchange.example.com/EWS/Exchange.asmx";
            string username = "user@example.com";
            string password = "password";

            // Create the EWS client
            using (IEWSClient client = EWSClient.GetEWSClient(mailboxUri, username, password))
            {
                // Define the user configuration to delete (assumes it resides in the Inbox)
                UserConfigurationName configName = new UserConfigurationName("MyConfig", client.MailboxInfo.InboxUri);

                // Delete the configuration
                client.DeleteUserConfiguration(configName);
                Console.WriteLine("User configuration deleted successfully.");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}