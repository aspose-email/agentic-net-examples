using System;
using System.IO;
using System.Collections.Generic;
using Aspose.Email;
using Aspose.Email.Mapi;

namespace AsposeEmailVotingReport
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // Input directory containing MSG files
                string inputFolder = "Messages";
                // Output CSV file path
                string outputCsv = "VotingReport.csv";

                // Verify input directory exists
                if (!Directory.Exists(inputFolder))
                {
                    Console.Error.WriteLine($"Input folder does not exist: {inputFolder}");
                    return;
                }

                // Get all MSG files in the directory
                string[] msgFiles = Directory.GetFiles(inputFolder, "*.msg");
                if (msgFiles == null || msgFiles.Length == 0)
                {
                    Console.Error.WriteLine($"No MSG files found in folder: {inputFolder}");
                    return;
                }

                // Prepare CSV writer
                try
                {
                    using (StreamWriter writer = new StreamWriter(outputCsv, false))
                    {
                        // Write CSV header
                        writer.WriteLine("FileName,VotingButtons");

                        // Process each MSG file
                        foreach (string msgPath in msgFiles)
                        {
                            // Guard each file existence (should be true from GetFiles, but double‑check)
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

                                Console.Error.WriteLine($"File not found: {msgPath}");
                                continue;
                            }

                            try
                            {
                                // Load the MSG file
                                using (MapiMessage message = MapiMessage.Load(msgPath))
                                {
                                    // Retrieve voting buttons (if any)
                                    string[] votingButtons = FollowUpManager.GetVotingButtons(message);
                                    string buttonList = string.Empty;
                                    if (votingButtons != null && votingButtons.Length > 0)
                                    {
                                        buttonList = string.Join(";", votingButtons);
                                    }

                                    // Write a CSV line for this message
                                    writer.WriteLine($"{Path.GetFileName(msgPath)},{buttonList}");
                                }
                            }
                            catch (Exception exFile)
                            {
                                Console.Error.WriteLine($"Error processing file '{msgPath}': {exFile.Message}");
                                // Continue with next file
                            }
                        }
                    }
                }
                catch (Exception exCsv)
                {
                    Console.Error.WriteLine($"Failed to write CSV file '{outputCsv}': {exCsv.Message}");
                    return;
                }

                Console.WriteLine($"Voting report generated successfully at: {outputCsv}");
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Unexpected error: {ex.Message}");
            }
        }
    }
}
