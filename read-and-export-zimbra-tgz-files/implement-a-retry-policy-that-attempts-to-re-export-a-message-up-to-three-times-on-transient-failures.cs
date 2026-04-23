using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Storage.Zimbra;

class Program
{
    static void Main()
    {
        try
        {
            // Input TGZ file path (replace with actual path or keep placeholder)
            string tgzPath = "input.tgz";
            // Output directory for exported messages
            string outputDirectory = "ExportedMessages";

            // Guard input file existence
            if (!File.Exists(tgzPath))
            {
                Console.Error.WriteLine($"Input file not found: {tgzPath}");
                return;
            }

            // Ensure output directory exists
            try
            {
                if (!Directory.Exists(outputDirectory))
                {
                    Directory.CreateDirectory(outputDirectory);
                }
            }
            catch (Exception dirEx)
            {
                Console.Error.WriteLine($"Failed to create output directory: {dirEx.Message}");
                return;
            }

            // Retry policy: up to three attempts on transient timeout failures
            const int maxAttempts = 3;
            for (int attempt = 1; attempt <= maxAttempts; attempt++)
            {
                try
                {
                    using (TgzReader reader = new TgzReader(tgzPath))
                    {
                        // Export all messages to the output directory
                        reader.ExportTo(outputDirectory);
                    }

                    // Success, exit retry loop
                    Console.WriteLine("Export completed successfully.");
                    break;
                }
                catch (Aspose.Email.TimeoutException timeoutEx)
                {
                    // Transient timeout – decide whether to retry
                    if (attempt == maxAttempts)
                    {
                        Console.Error.WriteLine($"Export failed after {maxAttempts} attempts: {timeoutEx.Message}");
                        return;
                    }
                    else
                    {
                        Console.Error.WriteLine($"Attempt {attempt} timed out: {timeoutEx.Message}. Retrying...");
                    }
                }
                catch (Exception ex)
                {
                    // Non‑transient error – abort retries
                    Console.Error.WriteLine($"Export failed: {ex.Message}");
                    return;
                }
            }
        }
        catch (Exception unexpected)
        {
            Console.Error.WriteLine($"Unexpected error: {unexpected.Message}");
        }
    }
}
