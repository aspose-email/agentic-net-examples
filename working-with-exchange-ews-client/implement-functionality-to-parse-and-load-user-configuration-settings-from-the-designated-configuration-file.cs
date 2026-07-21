using System;
using System.IO;
using System.Text.Json;

// Author: Aspose.Email example author

namespace ConfigLoaderExample
{
    // Define a class that represents the configuration settings
    public class UserSettings
    {
        public string Username { get; set; }
        public string Email { get; set; }
        public int RefreshIntervalMinutes { get; set; }
    }

    public class Program
    {
        public static void Main(string[] args)
        {
            try
            {
                // Path to the configuration file (can be passed as an argument or use a default)
                string configFilePath = args.Length > 0 ? args[0] : "userconfig.json";

                // Ensure the directory exists
                string configDirectory = Path.GetDirectoryName(configFilePath);
                if (!string.IsNullOrEmpty(configDirectory) && !Directory.Exists(configDirectory))
                {
                    try
                    {
                        Directory.CreateDirectory(configDirectory);
                    }
                    catch (Exception dirEx)
                    {
                        Console.Error.WriteLine($"Failed to create configuration directory: {dirEx.Message}");
                        return;
                    }
                }

                // Guard file existence; create a minimal placeholder if missing
                if (!File.Exists(configFilePath))
                {
                    try
                    {
                        UserSettings placeholder = new UserSettings
                        {
                            Username = "defaultUser",
                            Email = "user@example.com",
                            RefreshIntervalMinutes = 15
                        };
                        string placeholderJson = JsonSerializer.Serialize(placeholder, new JsonSerializerOptions { WriteIndented = true });
                        File.WriteAllText(configFilePath, placeholderJson);
                        Console.WriteLine($"Configuration file not found. Created placeholder at '{configFilePath}'.");
                    }
                    catch (Exception createEx)
                    {
                        Console.Error.WriteLine($"Failed to create placeholder configuration file: {createEx.Message}");
                        return;
                    }
                }

                // Load and parse the configuration file
                UserSettings loadedSettings;
                try
                {
                    string jsonContent = File.ReadAllText(configFilePath);
                    loadedSettings = JsonSerializer.Deserialize<UserSettings>(jsonContent);
                    if (loadedSettings == null)
                    {
                        Console.Error.WriteLine("Configuration file is empty or malformed.");
                        return;
                    }
                }
                catch (Exception readEx)
                {
                    Console.Error.WriteLine($"Error reading or parsing configuration file: {readEx.Message}");
                    return;
                }

                // Use the loaded settings (example output)
                Console.WriteLine("Loaded user configuration:");
                Console.WriteLine($"Username: {loadedSettings.Username}");
                Console.WriteLine($"Email: {loadedSettings.Email}");
                Console.WriteLine($"Refresh Interval (minutes): {loadedSettings.RefreshIntervalMinutes}");
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Unexpected error: {ex.Message}");
            }
        }
    }
}
