using System;
using System.Net;
using Aspose.Email;
using Aspose.Email.Clients.Exchange.WebService;

class Program
{
    static void Main()
    {
        // Top‑level exception guard
        try
        {
            // Placeholder connection details – replace with real values when needed
            string mailboxUri = "https://example.com/EWS/Exchange.asmx";
            string username = "user@example.com";
            string password = "password";

            // Skip real network calls when placeholders are detected
            if (mailboxUri.Contains("example.com"))
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping EWS operations.");
                return;
            }

            // Create the EWS client safely
            IEWSClient client = null;
            try
            {
                client = EWSClient.GetEWSClient(mailboxUri, username, password);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to create EWS client: {ex.Message}");
                return;
            }

            // Ensure the client is disposed
            using (client)
            {
                // Build a user configuration name (folderId can be any valid folder identifier, e.g., "Inbox")
                UserConfigurationName configName = new UserConfigurationName("MyConfig", "Inbox");

                // Create the user configuration object
                UserConfiguration userConfig = new UserConfiguration(configName);

                // Example: store simple XML data in the configuration (optional)
                // string xml = "<settings><option name=\"example\" value=\"true\"/></settings>";
                // userConfig.XmlData = System.Text.Encoding.UTF8.GetBytes(xml);

                // Attempt to create the configuration on the server
                try
                {
                    client.CreateUserConfiguration(userConfig);
                    Console.WriteLine("User configuration created successfully.");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create user configuration: {ex.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
