using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.Json;

namespace ConfigLoader
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                string configPath = "config.json";

                // Ensure the configuration file exists
                if (!File.Exists(configPath))
                {
                    // Create a minimal placeholder configuration file
                    using (FileStream createStream = File.Create(configPath))
                    {
                        byte[] placeholderBytes = Encoding.UTF8.GetBytes("{}");
                        createStream.Write(placeholderBytes, 0, placeholderBytes.Length);
                    }
                    Console.WriteLine("Created placeholder config file.");
                }

                // Read the configuration file content
                string jsonContent;
                using (FileStream readStream = File.OpenRead(configPath))
                using (StreamReader reader = new StreamReader(readStream))
                {
                    jsonContent = reader.ReadToEnd();
                }

                // Parse JSON into a dictionary of settings
                Dictionary<string, string> settings = JsonSerializer.Deserialize<Dictionary<string, string>>(jsonContent);

                if (settings != null)
                {
                    foreach (KeyValuePair<string, string> entry in settings)
                    {
                        Console.WriteLine($"{entry.Key}: {entry.Value}");
                    }
                }
                else
                {
                    Console.WriteLine("No settings found in the configuration file.");
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error: {ex.Message}");
                return;
            }
        }
    }
}
