using System;
using System.IO;
using System.Net;
using Aspose.Email;
using Aspose.Email.Clients.Exchange.WebService;

class Program
{
    static void Main()
    {
        var credential = new System.Net.NetworkCredential("username", "password", "domain");

        try
        {
            // Define connection parameters
            string mailboxUri = "https://exchange.example.com/EWS/Exchange.asmx";
            string username = "user@example.com";
            string password = "password";

            // Ensure the output directory exists
            string outputDir = "Output";
            if (!Directory.Exists(outputDir))
            {
                Directory.CreateDirectory(outputDir);
            }

            // Path for the saved configuration file
            string configFilePath = Path.Combine(outputDir, "UserConfig.xml");

            // Create and use the EWS client
            using (IEWSClient client = EWSClient.GetEWSClient(mailboxUri, new NetworkCredential(username, password)))
            {
                // Define a user configuration name (folder name and configuration name)
                UserConfigurationName configName = new UserConfigurationName("MyConfig", "Inbox");

                // Create a new user configuration
                UserConfiguration userConfig = new UserConfiguration(configName);
                // Example: set a simple XML data (could be any settings)
                string xmlContent = "<Settings><Option name=\"Sample\" value=\"True\"/></Settings>";
                userConfig.XmlData = System.Text.Encoding.UTF8.GetBytes(xmlContent);

                // Create the configuration on the server
                client.CreateUserConfiguration(userConfig);

                // Retrieve the configuration back from the server
                UserConfiguration fetchedConfig = client.GetUserConfiguration(configName);

                // Save the fetched configuration to a local file
                try
                {
                    using (FileStream fs = new FileStream(configFilePath, FileMode.Create, FileAccess.Write))
                    {
                        fs.Write(fetchedConfig.XmlData, 0, fetchedConfig.XmlData.Length);
                    }
                    Console.WriteLine($"User configuration saved to: {configFilePath}");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error writing configuration file: {ex.Message}");
                    return;
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}