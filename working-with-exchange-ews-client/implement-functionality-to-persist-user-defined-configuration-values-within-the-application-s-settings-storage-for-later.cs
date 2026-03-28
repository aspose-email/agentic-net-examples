using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.Json;

class Program
{
    static void Main()
    {
        try
        {
            // Define a folder to store configuration files
            string configDirectory = Path.Combine(Environment.CurrentDirectory, "Config");
            if (!Directory.Exists(configDirectory))
            {
                Directory.CreateDirectory(configDirectory);
            }

            // Full path to the settings file
            string configFilePath = Path.Combine(configDirectory, "settings.json");

            // Prepare configuration values to persist
            Dictionary<string, string> settingsToSave = new Dictionary<string, string>();
            settingsToSave["ServerUrl"] = "https://example.com/ews";
            settingsToSave["Username"] = "user@example.com";
            settingsToSave["Password"] = "P@ssw0rd";

            // Serialize the dictionary to JSON
            string jsonString = JsonSerializer.Serialize(settingsToSave, new JsonSerializerOptions { WriteIndented = true });

            // Write the JSON to the file (guarded with try/catch)
            try
            {
                using (FileStream fileStream = new FileStream(configFilePath, FileMode.Create, FileAccess.Write, FileShare.None))
                using (StreamWriter writer = new StreamWriter(fileStream, Encoding.UTF8))
                {
                    writer.Write(jsonString);
                }
            }
            catch (Exception ioEx)
            {
                Console.Error.WriteLine($"Failed to write configuration file: {ioEx.Message}");
                return;
            }

            // Ensure the file exists before attempting to read
            if (!File.Exists(configFilePath))
            {
                Console.Error.WriteLine("Configuration file not found.");
                return;
            }

            // Load the configuration values back into a dictionary
            Dictionary<string, string> loadedSettings;
            try
            {
                using (FileStream fileStream = new FileStream(configFilePath, FileMode.Open, FileAccess.Read, FileShare.Read))
                using (StreamReader reader = new StreamReader(fileStream, Encoding.UTF8))
                {
                    string readJson = reader.ReadToEnd();
                    loadedSettings = JsonSerializer.Deserialize<Dictionary<string, string>>(readJson);
                }
            }
            catch (Exception ioEx)
            {
                Console.Error.WriteLine($"Failed to read configuration file: {ioEx.Message}");
                return;
            }

            // Demonstrate usage of the loaded settings
            if (loadedSettings != null)
            {
                foreach (KeyValuePair<string, string> kvp in loadedSettings)
                {
                    Console.WriteLine($"{kvp.Key}: {kvp.Value}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
