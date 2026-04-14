using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Mapi;

namespace VotingResultsProcessor
{
    public class Program
    {
        public static void Main()
        {
            try
            {
                // Directory containing the MSG files with voting results
                string folderPath = "VotingMessages";

                // Verify that the directory exists
                if (!Directory.Exists(folderPath))
                {
                    Console.Error.WriteLine($"Error: Directory not found – {folderPath}");
                    return;
                }

                // Process each MSG file in the directory
                string[] msgFiles = Directory.GetFiles(folderPath, "*.msg");
                foreach (string msgFilePath in msgFiles)
                {
                    // Verify that the file exists before attempting to load it
                    if (!File.Exists(msgFilePath))
                    {
                try
                {
                    using (MapiMessage placeholder = new MapiMessage(
                        "from@example.com",
                        "to@example.com",
                        "Placeholder Subject",
                        "Placeholder body."))
                    {
                        placeholder.Save(msgFilePath);
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error creating placeholder MSG: {ex.Message}");
                    return;
                }

                        Console.Error.WriteLine($"Error: File not found – {msgFilePath}");
                        continue;
                    }

                    try
                    {
                        // Load the MSG file into a MapiMessage object
                        using (MapiMessage message = MapiMessage.Load(msgFilePath))
                        {
                            // Retrieve the voting buttons (options) defined for the message
                            string[] votingButtons = FollowUpManager.GetVotingButtons(message);

                            // Output the voting options for demonstration purposes
                            Console.WriteLine($"Message: {Path.GetFileName(msgFilePath)}");
                            Console.WriteLine("Voting options:");
                            if (votingButtons != null && votingButtons.Length > 0)
                            {
                                foreach (string button in votingButtons)
                                {
                                    Console.WriteLine($" - {button}");
                                }
                            }
                            else
                            {
                                Console.WriteLine(" - (No voting buttons defined)");
                            }

                            // TODO: Insert code here to read actual voting results
                            // and update the central database table accordingly.
                            // Example (pseudo‑code):
                            // Database.UpdateVotingResults(message.Subject, votingResults);
                        }
                    }
                    catch (Exception ex)
                    {
                        // Handle errors that occur while processing an individual MSG file
                        Console.Error.WriteLine($"Error processing file '{msgFilePath}': {ex.Message}");
                    }
                }
            }
            catch (Exception ex)
            {
                // Top‑level exception guard
                Console.Error.WriteLine($"Unhandled error: {ex.Message}");
            }
        }
    }
}
