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
            // Mailbox connection settings
            string mailboxUri = "https://exchange.example.com/EWS/Exchange.asmx";
            string username = "user@example.com";
            string password = "password";

            // Create the EWS client
            using (IEWSClient client = EWSClient.GetEWSClient(mailboxUri, username, password))
            {
                try
                {
                    // Define the user configuration to delete (name and folder)
                    UserConfigurationName configName = new UserConfigurationName("MyConfig", "Inbox");

                    // Delete the configuration
                    client.DeleteUserConfiguration(configName);
                    Console.WriteLine("User configuration deleted successfully.");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Operation error: {ex.Message}");
                    return;
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unhandled exception: {ex.Message}");
        }
    }
}
