using System;
using System.Collections.Generic;
using System.IO;
using Aspose.Email;
using Aspose.Email.Mapi;

class Program
{
    static void Main()
    {
        try
        {
            // Output file path
            string outputFilePath = "voting_buttons.txt";

            // Ensure the directory exists
            string outputDirectory = Path.GetDirectoryName(outputFilePath);
            if (!string.IsNullOrEmpty(outputDirectory) && !Directory.Exists(outputDirectory))
            {
                Directory.CreateDirectory(outputDirectory);
            }

            // Create a sample MAPI message and add voting buttons
            using (MapiMessage sampleMessage = new MapiMessage("sender@example.com", "recipient@example.com", "Sample Subject", "Sample body"))
            {
                FollowUpManager.AddVotingButton(sampleMessage, "Approve");
                FollowUpManager.AddVotingButton(sampleMessage, "Reject");
                FollowUpManager.AddVotingButton(sampleMessage, "Maybe");

                // Retrieve voting buttons
                string[] votingButtons = FollowUpManager.GetVotingButtons(sampleMessage);

                // Sort alphabetically (case‑insensitive)
                List<string> buttonList = new List<string>(votingButtons);
                buttonList.Sort(StringComparer.OrdinalIgnoreCase);

                // Write sorted list to the file
                try
                {
                    using (StreamWriter writer = new StreamWriter(outputFilePath, false))
                    {
                        foreach (string button in buttonList)
                        {
                            writer.WriteLine(button);
                        }
                    }
                    Console.WriteLine($"Voting buttons saved to '{outputFilePath}'.");
                }
                catch (Exception ioEx)
                {
                    Console.Error.WriteLine($"Error writing to file: {ioEx.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
