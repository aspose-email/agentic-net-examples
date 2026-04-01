using System;
using System.IO;
using System.Text;
using System.Text.Json;
using System.Collections.Generic;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            string configFilePath = "userconfig.json";

            // Ensure the configuration file exists; create a minimal placeholder if missing
            if (!File.Exists(configFilePath))
            {
                try
                {
                    using (FileStream createStream = File.Create(configFilePath))
                    {
                        byte[] placeholder = Encoding.UTF8.GetBytes("{ }");
                        createStream.Write(placeholder, 0, placeholder.Length);
                    }
                    Console.WriteLine($"Placeholder configuration file created at {configFilePath}.");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create placeholder config file: {ex.Message}");
                    return;
                }
            }

            // Load configuration settings from the file
            Dictionary<string, string> settings = null;
            try
            {
                using (FileStream readStream = File.OpenRead(configFilePath))
                {
                    settings = JsonSerializer.Deserialize<Dictionary<string, string>>(readStream);
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to read configuration file: {ex.Message}");
                return;
            }

            if (settings == null)
            {
                Console.Error.WriteLine("Configuration file is empty or contains invalid JSON.");
                return;
            }

            // Output loaded settings
            foreach (KeyValuePair<string, string> entry in settings)
            {
                Console.WriteLine($"{entry.Key} = {entry.Value}");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
