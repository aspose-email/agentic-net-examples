using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using Aspose.Email;
using Aspose.Email.Mapi;

namespace AsposeEmailVotingExport
{
    class VotingInfo
    {
        public string FileName { get; set; }
        public string[] VotingButtons { get; set; }
    }

    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                string inputFolder = "InputMessages";
                string outputFile = "voting_buttons.json";

                // Verify input folder exists
                if (!Directory.Exists(inputFolder))
                {
                    Console.Error.WriteLine($"Input folder '{inputFolder}' does not exist.");
                    return;
                }

                List<VotingInfo> votingList = new List<VotingInfo>();

                // Process each MSG file in the folder
                foreach (string filePath in Directory.GetFiles(inputFolder, "*.msg"))
                {
                    try
                    {
                        using (MapiMessage message = MapiMessage.Load(filePath))
                        {
                            string[] buttons = FollowUpManager.GetVotingButtons(message);
                            VotingInfo info = new VotingInfo
                            {
                                FileName = Path.GetFileName(filePath),
                                VotingButtons = buttons ?? Array.Empty<string>()
                            };
                            votingList.Add(info);
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Failed to process '{filePath}': {ex.Message}");
                    }
                }

                // Serialize result to JSON
                string json = JsonSerializer.Serialize(votingList, new JsonSerializerOptions { WriteIndented = true });

                // Write JSON to output file
                try
                {
                    File.WriteAllText(outputFile, json);
                    Console.WriteLine($"Exported voting button configurations to '{outputFile}'.");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to write output file: {ex.Message}");
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Unexpected error: {ex.Message}");
            }
        }
    }
}
