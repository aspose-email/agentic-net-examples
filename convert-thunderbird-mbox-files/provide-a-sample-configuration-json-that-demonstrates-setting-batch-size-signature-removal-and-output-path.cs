using System;
using System.IO;
using System.Text;
using System.Text.Json;

namespace Sample
{
    class Program
    {
        static void Main()
        {
            try
            {
                string configDirectory = "Config";
                string configFilePath = Path.Combine(configDirectory, "conversionConfig.json");

                // Ensure the configuration directory exists
                if (!Directory.Exists(configDirectory))
                {
                    Directory.CreateDirectory(configDirectory);
                }

                // Prepare configuration data
                ConversionConfig config = new ConversionConfig
                {
                    BatchSize = 100,
                    RemoveSignature = true,
                    OutputPath = "Output/Converted.pst"
                };

                // Serialize configuration to JSON
                JsonSerializerOptions jsonOptions = new JsonSerializerOptions { WriteIndented = true };
                string json = JsonSerializer.Serialize(config, jsonOptions);

                // Write JSON to file with proper error handling
                try
                {
                    using (FileStream fileStream = new FileStream(configFilePath, FileMode.Create, FileAccess.Write, FileShare.None))
                    using (StreamWriter writer = new StreamWriter(fileStream, Encoding.UTF8))
                    {
                        writer.Write(json);
                    }

                    Console.WriteLine($"Configuration saved to: {configFilePath}");
                }
                catch (Exception ioEx)
                {
                    Console.Error.WriteLine($"Failed to write configuration file: {ioEx.Message}");
                    return;
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Unexpected error: {ex.Message}");
            }
        }

        private class ConversionConfig
        {
            public int BatchSize { get; set; }
            public bool RemoveSignature { get; set; }
            public string OutputPath { get; set; }
        }
    }
}
