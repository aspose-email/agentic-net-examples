using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Mapi;

class Program
{
    static void Main()
    {
        try
        {
            string inputPath = "input.msg";
            string outputPath = "output.msg";

            // Ensure input file exists; create a minimal placeholder if missing
            if (!File.Exists(inputPath))
            {
                try
                {
                    string inputDir = Path.GetDirectoryName(inputPath);
                    if (!string.IsNullOrEmpty(inputDir) && !Directory.Exists(inputDir))
                    {
                        Directory.CreateDirectory(inputDir);
                    }

                    using (MapiMessage placeholder = new MapiMessage("from@example.com", "to@example.com", "Placeholder Subject", "Placeholder body"))
                    {
                        placeholder.Save(inputPath);
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create placeholder MSG file: {ex.Message}");
                    return;
                }
            }

            // Load the MSG message
            try
            {
                using (MapiMessage message = MapiMessage.Load(inputPath))
                {
                    // Set a high‑importance flag
                    FollowUpManager.SetFlag(message, "High Importance");

                    // Configure reminder (e.g., 1 day from now)
                    FollowUpOptions options = new FollowUpOptions();
                    options.ReminderTime = DateTime.Now.AddDays(1);
                    FollowUpManager.SetOptions(message, options);

                    // Ensure output directory exists
                    try
                    {
                        string outputDir = Path.GetDirectoryName(outputPath);
                        if (!string.IsNullOrEmpty(outputDir) && !Directory.Exists(outputDir))
                        {
                            Directory.CreateDirectory(outputDir);
                        }
                    }
                    catch (Exception dirEx)
                    {
                        Console.Error.WriteLine($"Failed to ensure output directory: {dirEx.Message}");
                        return;
                    }

                    // Save the modified message
                    try
                    {
                        message.Save(outputPath);
                        Console.WriteLine($"Message saved with flag and reminder to '{outputPath}'.");
                    }
                    catch (Exception saveEx)
                    {
                        Console.Error.WriteLine($"Failed to save MSG file: {saveEx.Message}");
                    }
                }
            }
            catch (Exception loadEx)
            {
                Console.Error.WriteLine($"Failed to load MSG file: {loadEx.Message}");
                return;
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
