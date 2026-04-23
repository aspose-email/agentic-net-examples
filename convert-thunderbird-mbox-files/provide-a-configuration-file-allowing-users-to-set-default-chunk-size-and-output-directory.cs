using System;
using System.IO;
using System.Text.Json;

namespace AsposeEmailConfigSample
{
    public class AppConfig
    {
        public int ChunkSize { get; set; }
        public string OutputDirectory { get; set; }
    }

    class Program
    {
        static void Main()
        {
            try
            {
                // Define default configuration values
                int defaultChunkSize = 1024 * 1024; // 1 MB
                string defaultOutputDir = Path.Combine(Environment.CurrentDirectory, "Output");

                // Ensure the output directory exists
                if (!Directory.Exists(defaultOutputDir))
                {
                    try
                    {
                        Directory.CreateDirectory(defaultOutputDir);
                    }
                    catch (Exception dirEx)
                    {
                        Console.Error.WriteLine($"Failed to create output directory: {dirEx.Message}");
                        return;
                    }
                }

                // Create configuration object
                AppConfig config = new AppConfig
                {
                    ChunkSize = defaultChunkSize,
                    OutputDirectory = defaultOutputDir
                };

                // Serialize configuration to JSON
                string json = JsonSerializer.Serialize(config, new JsonSerializerOptions { WriteIndented = true });

                // Define configuration file path
                string configFilePath = Path.Combine(Environment.CurrentDirectory, "appconfig.json");

                // Write configuration file
                try
                {
                    using (FileStream fs = new FileStream(configFilePath, FileMode.Create, FileAccess.Write, FileShare.None))
                    using (StreamWriter writer = new StreamWriter(fs))
                    {
                        writer.Write(json);
                    }
                }
                catch (Exception ioEx)
                {
                    Console.Error.WriteLine($"Failed to write configuration file: {ioEx.Message}");
                    return;
                }

                Console.WriteLine($"Configuration file created at: {configFilePath}");
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Unexpected error: {ex.Message}");
            }
        }
    }
}
