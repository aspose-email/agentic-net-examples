using Aspose.Email;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using Aspose.Email.Mapi;

class Program
{
    static void Main()
    {
        try
        {
            // Input directory containing MSG files
            string inputDirectory = "MsgFiles";
            // Output JSON file path
            string outputFile = "aggregated_voting_buttons.json";

            // Verify input directory exists
            if (!Directory.Exists(inputDirectory))
            {
                Console.Error.WriteLine($"Error: Input directory not found – {inputDirectory}");
                return;
            }

            // Prepare aggregation dictionary
            Dictionary<string, string[]> aggregatedData = new Dictionary<string, string[]>();

            // Get all .msg files in the directory
            string[] msgFiles;
            try
            {
                msgFiles = Directory.GetFiles(inputDirectory, "*.msg");
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error accessing files: {ex.Message}");
                return;
            }

            foreach (string msgPath in msgFiles)
            {
                // Ensure the file exists before loading
                if (!File.Exists(msgPath))
                {
                try
                {
                    using (MapiMessage placeholder = new MapiMessage(
                        "from@example.com",
                        "to@example.com",
                        "Placeholder Subject",
                        "Placeholder body."))
                    {
                        placeholder.Save(msgPath);
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error creating placeholder MSG: {ex.Message}");
                    return;
                }

                    Console.Error.WriteLine($"Warning: File not found – {msgPath}");
                    continue;
                }

                // Load the MSG file and extract voting buttons
                try
                {
                    using (MapiMessage msg = MapiMessage.Load(msgPath))
                    {
                        string[] votingButtons = FollowUpManager.GetVotingButtons(msg);
                        aggregatedData[Path.GetFileName(msgPath)] = votingButtons;
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error processing '{msgPath}': {ex.Message}");
                }
            }

            // Serialize aggregation to JSON
            string json;
            try
            {
                json = JsonSerializer.Serialize(aggregatedData, new JsonSerializerOptions { WriteIndented = true });
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error serializing JSON: {ex.Message}");
                return;
            }

            // Ensure output directory exists
            string outputDir = Path.GetDirectoryName(outputFile);
            if (!string.IsNullOrEmpty(outputDir) && !Directory.Exists(outputDir))
            {
                try
                {
                    Directory.CreateDirectory(outputDir);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error creating output directory: {ex.Message}");
                    return;
                }
            }

            // Write JSON to file
            try
            {
                File.WriteAllText(outputFile, json);
                Console.WriteLine($"Aggregated voting buttons saved to '{outputFile}'.");
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error writing output file: {ex.Message}");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
