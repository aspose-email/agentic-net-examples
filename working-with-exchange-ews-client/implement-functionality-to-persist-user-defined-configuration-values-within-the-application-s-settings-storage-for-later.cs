using System;
using Aspose.Email;
using Aspose.Email.Clients.Exchange.WebService;

class Program
{
    static void Main()
    {
        try
        {
            // Placeholder connection settings
            string ewsUrl = "https://example.com/EWS/Exchange.asmx";
            string username = "user@example.com";
            string password = "password";

            // Skip execution when placeholders are detected
            if (ewsUrl.Contains("example.com") || username.Contains("example.com"))
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping EWS operations.");
                return;
            }

            // Create the EWS client safely
            using (IEWSClient client = EWSClient.GetEWSClient(ewsUrl, username, password))
            {
                // Build a user configuration name (name + folder identifier)
                UserConfigurationName configName = new UserConfigurationName("MyConfig", client.MailboxInfo.InboxUri);

                // Create a new user configuration and set some data
                UserConfiguration configToCreate = new UserConfiguration(configName);
                configToCreate.BinaryData = new byte[] { 0x01, 0x02, 0x03 };

                // Persist the configuration on the server
                client.CreateUserConfiguration(configToCreate);
                Console.WriteLine("User configuration created successfully.");

                // Retrieve the stored configuration
                UserConfiguration fetchedConfig = client.GetUserConfiguration(configName);
                Console.WriteLine("Fetched configuration binary length: " + fetchedConfig.BinaryData.Length);
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine("Error: " + ex.Message);
        }
    }
}
