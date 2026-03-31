using System;
using System.IO;
using System.Text.Json;
using Aspose.Email;

namespace ConfigSample
{
    class Program
    {
        static void Main()
        {
            try
            {
                string configPath = "config.json";

                // Ensure the configuration file exists; create a minimal placeholder if missing
                if (!File.Exists(configPath))
                {
                    try
                    {
                        var defaultConfig = new AppConfig
                        {
                            ApplicationName = "MyApp",
                            MaxItems = 100,
                            EnableFeatureX = true
                        };
                        string json = JsonSerializer.Serialize(defaultConfig, new JsonSerializerOptions { WriteIndented = true });
                        File.WriteAllText(configPath, json);
                        Console.WriteLine("Created default configuration file.");
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Failed to create default config: {ex.Message}");
                        return;
                    }
                }

                // Load existing configuration
                AppConfig config;
                try
                {
                    string json = File.ReadAllText(configPath);
                    config = JsonSerializer.Deserialize<AppConfig>(json);
                    if (config == null)
                    {
                        Console.Error.WriteLine("Configuration file is empty or malformed.");
                        return;
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to read configuration: {ex.Message}");
                    return;
                }

                // Modify configuration with validation
                Console.WriteLine($"Current MaxItems: {config.MaxItems}");
                Console.Write("Enter new MaxItems (positive integer): ");
                string input = Console.ReadLine();
                if (int.TryParse(input, out int newMax) && newMax > 0)
                {
                    config.MaxItems = newMax;
                }
                else
                {
                    Console.Error.WriteLine("Invalid input. MaxItems must be a positive integer.");
                    return;
                }

                // Persist updated configuration
                try
                {
                    string updatedJson = JsonSerializer.Serialize(config, new JsonSerializerOptions { WriteIndented = true });
                    File.WriteAllText(configPath, updatedJson);
                    Console.WriteLine("Configuration updated successfully.");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to write configuration: {ex.Message}");
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Unexpected error: {ex.Message}");
            }
        }

        // Simple configuration data contract
        private class AppConfig
        {
            public string ApplicationName { get; set; }
            public int MaxItems { get; set; }
            public bool EnableFeatureX { get; set; }
        }
    }
}
