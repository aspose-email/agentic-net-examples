using Aspose.Email;
using Aspose.Email.Storage;
using Aspose.Email.Storage.Mbox;
using Aspose.Email.Storage.Pst;
using System;
using System.IO;
using System.Text.Json;

namespace ConvertThunderbirdMbox
{
    class Config
    {
        public string MboxFile { get; set; }
        public string PstFile { get; set; }
        public int? BatchSize { get; set; }
        public bool? RemoveSignature { get; set; }
    }

    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // Load configuration from appsettings.json (optional).
                Config config = LoadConfig("appsettings.json");

                string mboxPath = string.IsNullOrWhiteSpace(config?.MboxFile) ? "storage.mbox" : config.MboxFile;
                string pstPath = string.IsNullOrWhiteSpace(config?.PstFile) ? "output.pst" : config.PstFile;
                int batchSize = config?.BatchSize ?? 100; // placeholder for future use
                bool removeSignature = config?.RemoveSignature ?? false;

                // Guard input file existence.
                if (!File.Exists(mboxPath))
                {
                    Console.Error.WriteLine($"Input MBOX file not found: {mboxPath}");
                    return;
                }

                // Ensure output directory exists.
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

                // Set conversion options.
                MboxToPstConversionOptions options = new MboxToPstConversionOptions
                {
                    RemoveSignature = removeSignature
                };

                // Perform conversion inside a guarded block.
                try
                {
                    using (PersonalStorage pst = MailStorageConverter.MboxToPst(mboxPath, pstPath, options))
                    {
                        Console.WriteLine($"MBOX file '{mboxPath}' successfully converted to PST '{pstPath}'.");
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Conversion failed: {ex.Message}");
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Unexpected error: {ex.Message}");
            }
        }

        private static Config LoadConfig(string filePath)
        {
            try
            {
                if (!File.Exists(filePath))
                    return null;

                string json = File.ReadAllText(filePath);
                return JsonSerializer.Deserialize<Config>(json);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to load configuration: {ex.Message}");
                return null;
            }
        }
    }
}
