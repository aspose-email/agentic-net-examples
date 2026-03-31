using System;
using Aspose.Email;
using Aspose.Email.Clients.Exchange.WebService;

class Program
{
    static void Main()
    {
        try
        {
            // Placeholder connection details.
            string mailboxUri = "https://example.com/EWS/Exchange.asmx";
            string username = "username";
            string password = "password";

            // Guard against executing with placeholder credentials.
            if (mailboxUri.Contains("example.com"))
            {
                Console.WriteLine("Placeholder credentials detected. Skipping EWS operations.");
                return;
            }

            // Create the EWS client.
            using (IEWSClient client = EWSClient.GetEWSClient(mailboxUri, username, password))
            {
                // Define the user configuration name (folder and config name).
                UserConfigurationName configName = new UserConfigurationName("Inbox", "MyUserConfig");

                // Create a new UserConfiguration instance.
                UserConfiguration userConfig = new UserConfiguration(configName);

                // Example: set a custom property (optional).
                // userConfig.Values["CustomKey"] = "CustomValue";

                // Create the configuration on the server.
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
