using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using Aspose.Email;
using Aspose.Email.Clients.Exchange.WebService;

namespace AsposeEmailUserConfigSample
{
    class Program
    {
        static void Main()
        {
            // Exchange service credentials (replace with real values)
            string serviceUrl = "https://exchange.example.com/EWS/Exchange.asmx";
            string username = "user@example.com";
            string password = "password";

            // Define user configuration name and folder
            string configName = "MyConfig";
            string folderId = "Inbox";

            // Skip external calls when placeholder credentials are used
            if (serviceUrl.Contains("example.com") || username.Contains("example.com") || password == "password")
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping external calls.");
                return;
            }

            // Prepare output path for local persistence
            string outputDirectory = Path.Combine(Environment.CurrentDirectory, "Output");
            string outputPath = Path.Combine(outputDirectory, "UserConfig.json");

            // Ensure the output directory exists (validation rule)
            if (!Directory.Exists(outputDirectory))
            {
                Directory.CreateDirectory(outputDirectory);
            }

            try
            {
                // Create EWS client
                using (IEWSClient client = EWSClient.GetEWSClient(serviceUrl, username, password))
                {
                    // Create a UserConfigurationName instance
                    UserConfigurationName userConfigName = new UserConfigurationName(configName, folderId);

                    // Initialize a new UserConfiguration
                    UserConfiguration userConfig = new UserConfiguration(userConfigName);

                    // Add custom key‑value pairs (Dictionary<object, object>)
                    IDictionary<object, object> dict = userConfig.Dictionary;
                    dict["Theme"] = "Dark";
                    dict["PageSize"] = "20";

                    // Create the configuration on the server
                    client.CreateUserConfiguration(userConfig);

                    // Retrieve the configuration back from the server
                    UserConfiguration fetchedConfig = client.GetUserConfiguration(userConfigName);

                    // Convert fetched dictionary to Dictionary<string,string> for JSON serialization
                    var stringDict = new Dictionary<string, string>();
                    foreach (KeyValuePair<object, object> kvp in fetchedConfig.Dictionary)
                    {
                        string key = kvp.Key?.ToString() ?? string.Empty;
                        string value = kvp.Value?.ToString() ?? string.Empty;
                        stringDict[key] = value;
                    }

                    // Serialize the dictionary to JSON and save locally
                    string json = JsonSerializer.Serialize(stringDict, new JsonSerializerOptions { WriteIndented = true });
                    File.WriteAllText(outputPath, json);
                    Console.WriteLine($"User configuration saved to {outputPath}");

                    // Update a value and push the change to the server
                    fetchedConfig.Dictionary["PageSize"] = "50";
                    client.UpdateUserConfiguration(fetchedConfig);
                    Console.WriteLine("User configuration updated on the server.");

                    // Clean up: delete the configuration from the server
                    client.DeleteUserConfiguration(userConfigName);
                    Console.WriteLine("User configuration deleted from the server.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }
    }
}
