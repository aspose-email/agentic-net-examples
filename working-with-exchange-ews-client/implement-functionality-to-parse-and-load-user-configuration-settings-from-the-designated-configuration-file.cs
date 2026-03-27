using System;
using System.IO;
using System.Text;
using System.Text.Json;

using Aspose.Email;

namespace ConfigLoaderSample
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // Path to the configuration file
                string configFilePath = "userConfig.json";

                // Ensure the configuration file exists
                if (!File.Exists(configFilePath))
                {
                    // Create a minimal placeholder configuration file
                    string placeholderJson = "{}";
                    try
                    {
                        File.WriteAllText(configFilePath, placeholderJson, Encoding.UTF8);
                    }
                    catch (Exception writeEx)
                    {
                        Console.Error.WriteLine($"Failed to create placeholder config file: {writeEx.Message}");
                        return;
                    }
                }

                // Load the configuration file content
                string configJson;
                try
                {
                    using (FileStream fileStream = new FileStream(configFilePath, FileMode.Open, FileAccess.Read, FileShare.Read))
                    using (StreamReader reader = new StreamReader(fileStream, Encoding.UTF8))
                    {
                        configJson = reader.ReadToEnd();
                    }
                }
                catch (Exception readEx)
                {
                    Console.Error.WriteLine($"Failed to read config file: {readEx.Message}");
                    return;
                }

                // Deserialize JSON into a strongly typed object
                ConfigSettings settings;
                try
                {
                    settings = JsonSerializer.Deserialize<ConfigSettings>(configJson);
                }
                catch (Exception jsonEx)
                {
                    Console.Error.WriteLine($"Failed to parse config JSON: {jsonEx.Message}");
                    return;
                }

                // Output loaded settings
                if (settings != null)
                {
                    Console.WriteLine("Configuration Settings:");
                    Console.WriteLine($"Server: {settings.Server}");
                    Console.WriteLine($"Port: {settings.Port}");
                    Console.WriteLine($"UseSsl: {settings.UseSsl}");
                    Console.WriteLine($"Username: {settings.Username}");
                }
                else
                {
                    Console.WriteLine("Configuration file is empty or contains no recognizable settings.");
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Unexpected error: {ex.Message}");
                // Gracefully exit without throwing
            }
        }
    }

    // Strongly typed representation of the configuration file
    public class ConfigSettings
    {
        public string Server { get; set; }
        public int Port { get; set; }
        public bool UseSsl { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
