using Aspose.Email;
using System;
using System.IO;
using System.Text.Json;
using Aspose.Email.Storage;
using Aspose.Email.Storage.Pst;

class Program
{
    // Configuration model matching appsettings.json
    private class Config
    {
        public int BatchSize { get; set; } = 1000;          // Number of messages per batch (not directly used by API)
        public bool RemoveSignature { get; set; } = false; // Whether to strip signatures during conversion
    }

    static void Main(string[] args)
    {
        try
        {
            // Expecting two command‑line arguments: input MBOX path and output PST path
            if (args.Length < 2)
            {
                Console.Error.WriteLine("Usage: ConvertMboxToPst <mboxPath> <pstPath>");
                return;
            }

            string mboxPath = args[0];
            string pstPath = args[1];
            string configPath = "appsettings.json";

            // Guard input MBOX file existence
            if (!File.Exists(mboxPath))
            {
                Console.Error.WriteLine($"MBOX file not found: {mboxPath}");
                return;
            }

            // Ensure output directory exists
            string pstDirectory = Path.GetDirectoryName(pstPath);
            if (!string.IsNullOrEmpty(pstDirectory) && !Directory.Exists(pstDirectory))
            {
                try
                {
                    Directory.CreateDirectory(pstDirectory);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create output directory: {ex.Message}");
                    return;
                }
            }

            // Load configuration (create minimal placeholder if missing)
            Config config;
            if (!File.Exists(configPath))
            {
                // Create default config file
                config = new Config();
                try
                {
                    string defaultJson = JsonSerializer.Serialize(config, new JsonSerializerOptions { WriteIndented = true });
                    File.WriteAllText(configPath, defaultJson);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to write default config: {ex.Message}");
                    return;
                }
            }
            else
            {
                try
                {
                    string json = File.ReadAllText(configPath);
                    config = JsonSerializer.Deserialize<Config>(json) ?? new Config();
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to read config: {ex.Message}");
                    return;
                }
            }

            // Prepare conversion options
            MboxToPstConversionOptions options = new MboxToPstConversionOptions
            {
                RemoveSignature = config.RemoveSignature
            };

            // Perform conversion inside a try/catch to capture any Aspose.Email errors
            try
            {
                // The API handles the conversion internally; batch size is not a direct parameter,
                // but could be used for custom processing if needed.
                MailStorageConverter.MboxToPst(mboxPath, pstPath, options);
                Console.WriteLine($"Conversion completed successfully. PST saved to: {pstPath}");
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Conversion failed: {ex.Message}");
            }
        }
        catch (Exception ex)
        {
            // Top‑level guard
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
