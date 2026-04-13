using System;
using System.IO;
using System.Linq;
using Aspose.Email;
using Aspose.Email.Mapi;

class Program
{
    static void Main()
    {
        try
        {
            string inputMessagePath = "input.msg";
            string outputFilePath = "voting_buttons.txt";

            // Ensure input file exists; create a minimal placeholder if missing
            if (!File.Exists(inputMessagePath))
            {
                try
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(inputMessagePath) ?? string.Empty);
                    using (MapiMessage placeholder = new MapiMessage("sender@example.com", "recipient@example.com", "Subject", "Body"))
                    {
                        placeholder.Save(inputMessagePath);
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create placeholder message: {ex.Message}");
                    return;
                }
            }

            // Load the message and retrieve voting buttons
            string[] votingButtons;
            try
            {
                using (MapiMessage message = MapiMessage.Load(inputMessagePath))
                {
                    votingButtons = FollowUpManager.GetVotingButtons(message);
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to load message or retrieve voting buttons: {ex.Message}");
                return;
            }

            // Sort the buttons alphabetically
            if (votingButtons != null && votingButtons.Length > 0)
            {
                Array.Sort(votingButtons);
            }

            // Ensure output directory exists
            try
            {
                string outputDir = Path.GetDirectoryName(outputFilePath);
                if (!string.IsNullOrEmpty(outputDir) && !Directory.Exists(outputDir))
                {
                    Directory.CreateDirectory(outputDir);
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to ensure output directory: {ex.Message}");
                return;
            }

            // Write sorted buttons to the output file
            try
            {
                using (StreamWriter writer = new StreamWriter(outputFilePath, false))
                {
                    if (votingButtons != null)
                    {
                        foreach (string button in votingButtons)
                        {
                            writer.WriteLine(button);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to write voting buttons to file: {ex.Message}");
                return;
            }

            Console.WriteLine("Voting buttons have been saved successfully.");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
