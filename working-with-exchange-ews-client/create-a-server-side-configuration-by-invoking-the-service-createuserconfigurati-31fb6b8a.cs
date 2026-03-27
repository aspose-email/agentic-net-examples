using System;
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
            // Define mailbox URI and credentials (replace with real values)
            string mailboxUri = "https://exchange.example.com/EWS/Exchange.asmx";
            string username = "user@example.com";
            string password = "password";

            // Create the EWS client inside a using block to ensure disposal
            try
            {
                using (IEWSClient client = EWSClient.GetEWSClient(mailboxUri, username, password))
                {
                    // Create a UserConfigurationName (name and folder identifier)
                    UserConfigurationName configName = new UserConfigurationName("MyConfig", "Inbox");

                    // Create a UserConfiguration instance using the name
                    UserConfiguration userConfig = new UserConfiguration(configName);

                    // Optionally set XML data or other properties on the configuration
                    // Here we set an empty XML payload as a placeholder
                    userConfig.XmlData = new byte[0];

                    // Create the user configuration on the server
                    try
                    {
                        client.CreateUserConfiguration(userConfig);
                        Console.WriteLine("User configuration created successfully.");
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine("Error creating user configuration: " + ex.Message);
                        return;
                    }

                    // Retrieve the configuration to verify it was created
                    try
                    {
                        UserConfiguration fetchedConfig = client.GetUserConfiguration(configName);
                        Console.WriteLine("User configuration retrieved successfully.");
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine("Error retrieving user configuration: " + ex.Message);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine("Failed to initialize EWS client: " + ex.Message);
                return;
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine("Unexpected error: " + ex.Message);
        }
    }
}