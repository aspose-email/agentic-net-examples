using System;
using System.IO;
using System.Collections.Generic;
using Aspose.Email;
using Aspose.Email.Mapi;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            // Define file paths
            string inputMsgPath = "input.msg";
            string outputMsgPath = "output.msg";
            string configPath = "voting_buttons.txt";

            // Verify input MSG file exists
            if (!File.Exists(inputMsgPath))
            {
                try
                {
                    using (MapiMessage placeholder = new MapiMessage(
                        "from@example.com",
                        "to@example.com",
                        "Placeholder Subject",
                        "Placeholder body."))
                    {
                        placeholder.Save(inputMsgPath);
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error creating placeholder MSG: {ex.Message}");
                    return;
                }

                Console.Error.WriteLine($"Input MSG file not found: {inputMsgPath}");
                return;
            }

            // Verify configuration file exists; create a minimal placeholder if missing
            if (!File.Exists(configPath))
            {
                try
                {
                    File.WriteAllLines(configPath, new[] { "Approve", "Reject" });
                    Console.WriteLine($"Configuration file created with default buttons at: {configPath}");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create configuration file: {ex.Message}");
                    return;
                }
            }

            // Read voting button names from configuration
            string[] buttonLines;
            try
            {
                buttonLines = File.ReadAllLines(configPath);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to read configuration file: {ex.Message}");
                return;
            }

            List<string> votingButtons = new List<string>();
            foreach (string line in buttonLines)
            {
                string trimmed = line.Trim();
                if (!string.IsNullOrEmpty(trimmed))
                {
                    votingButtons.Add(trimmed);
                }
            }

            // Load the MSG file
            using (MapiMessage message = MapiMessage.Load(inputMsgPath))
            {
                // Clear existing voting buttons
                FollowUpManager.ClearVotingButtons(message);

                // Add voting buttons from configuration
                foreach (string button in votingButtons)
                {
                    try
                    {
                        FollowUpManager.AddVotingButton(message, button);
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Failed to add voting button '{button}': {ex.Message}");
                    }
                }

                // Ensure output directory exists
                string outputDir = Path.GetDirectoryName(outputMsgPath);
                if (!string.IsNullOrEmpty(outputDir) && !Directory.Exists(outputDir))
                {
                    try
                    {
                        Directory.CreateDirectory(outputDir);
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Failed to create output directory: {ex.Message}");
                        return;
                    }
                }

                // Save the updated MSG file
                try
                {
                    message.Save(outputMsgPath);
                    Console.WriteLine($"Updated MSG saved to: {outputMsgPath}");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to save updated MSG: {ex.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
